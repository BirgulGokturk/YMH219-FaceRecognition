using OpenCvSharp;
using YMH219_FaceRecognition.Models;

namespace YMH219_FaceRecognition.Services
{
    /// <summary>
    /// Tespit edilen yüzleri yerel örnek kümesiyle karşılaştırarak etiket ve güven skoru üreten servis.
    /// </summary>
    public class FaceRecognitionService : IDisposable
    {
        private readonly List<FaceSample> _samples = new();
        public IReadOnlyList<FaceSample> Samples => _samples.AsReadOnly();

        public double RecognitionThreshold { get; set; } = 45.0; // Minimum benzerlik yüzdesi

        /// <summary>
        /// Örnek kümesinden bir yüz örneği ekler.
        /// </summary>
        public void AddSample(string personName, Mat image)
        {
            var normalized = NormalizeFace(image);
            _samples.Add(new FaceSample(personName, normalized));
        }

        /// <summary>
        /// Belirtilen klasördeki kişi fotoğraflarını tarayarak veri setini (yeniden) yükler.
        /// Önceden yüklenmiş örnekler silinir; böylece tekrar tarandığında örnekler çoğalmaz.
        /// Klasör yapısı: sample_data/KisiAdi/resim1.jpg veya sample_data/KisiAdi_1.jpg
        /// </summary>
        public int LoadSamplesFromDirectory(string directoryPath, FaceDetectionService detector)
        {
            ClearSamples();

            if (!Directory.Exists(directoryPath)) return 0;

            int loadedCount = 0;
            var imageFiles = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".png", StringComparison.OrdinalIgnoreCase));

            foreach (var file in imageFiles)
            {
                try
                {
                    using var img = ReadImage(file);
                    if (img.Empty()) continue;

                    string personName = GetPersonName(file, directoryPath);

                    // Resimdeki yüzü tespit et
                    var faces = detector.DetectFaces(img);
                    if (faces.Length > 0)
                    {
                        using var cropped = new Mat(img, faces[0]);
                        var normalized = NormalizeFace(cropped);
                        _samples.Add(new FaceSample(personName, normalized, file));
                        loadedCount++;
                    }
                    else
                    {
                        // Yüz doğrudan kırpılmış verilmişse tamamını kullan
                        var normalized = NormalizeFace(img);
                        _samples.Add(new FaceSample(personName, normalized, file));
                        loadedCount++;
                    }
                }
                catch { }
            }

            return loadedCount;
        }

        /// <summary>
        /// Dosyanın hangi kişiye ait olduğunu bulur: alt klasördeyse klasör adı
        /// (sample_data/Ali/resim1.jpg → "Ali"), değilse dosya adının '_' öncesi
        /// (sample_data/Ali_1.jpg → "Ali") kullanılır.
        /// </summary>
        private static string GetPersonName(string filePath, string rootDirectory)
        {
            string parentFolder = Path.GetFullPath(Path.GetDirectoryName(filePath) ?? rootDirectory);
            string root = Path.GetFullPath(rootDirectory);

            if (!string.Equals(parentFolder.TrimEnd('\\', '/'), root.TrimEnd('\\', '/'), StringComparison.OrdinalIgnoreCase))
            {
                return Path.GetFileName(parentFolder);
            }

            return Path.GetFileNameWithoutExtension(filePath).Split('_')[0];
        }

        /// <summary>
        /// Resmi dosyadan okur. Cv2.ImRead Windows'ta Türkçe karakter içeren yolları
        /// (ör. C:\Users\Birgül\...) okuyamayabildiği için dosya önce byte dizisi olarak okunur.
        /// </summary>
        public static Mat ReadImage(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            return Cv2.ImDecode(bytes, ImreadModes.Color);
        }

        /// <summary>
        /// Yüklü tüm örnekleri bellekten siler.
        /// </summary>
        public void ClearSamples()
        {
            foreach (var s in _samples)
            {
                s.NormalizedFace.Dispose();
            }
            _samples.Clear();
        }

        /// <summary>
        /// Verilen yüz görüntüsünü kayıtlı örneklerle karşılaştırarak en olası kişiyi ve güven skorunu tahmin eder.
        /// </summary>
        public (string Label, double Confidence) Recognize(Mat croppedFace)
        {
            if (_samples.Count == 0)
            {
                return ("Kayıtlı Örnek Yok", 0.0);
            }

            using var normalized = NormalizeFace(croppedFace);

            string bestMatchLabel = "Bilinmiyor";
            double bestScore = 0.0;

            foreach (var sample in _samples)
            {
                double score = CalculateSimilarity(normalized, sample.NormalizedFace);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMatchLabel = sample.PersonName;
                }
            }

            if (bestScore < RecognitionThreshold)
            {
                return ("Bilinmiyor", bestScore);
            }

            return (bestMatchLabel, bestScore);
        }

        /// <summary>
        /// İki normalleştirilmiş yüz matrisi arasındaki benzerlik skorunu (0 - 100) hesaplar.
        /// </summary>
        private double CalculateSimilarity(Mat face1, Mat face2)
        {
            using var res = new Mat();
            // Normalleştirilmiş çapraz korelasyon
            Cv2.MatchTemplate(face1, face2, res, TemplateMatchModes.CCoeffNormed);
            Cv2.MinMaxLoc(res, out _, out double maxVal);

            // -1 ile +1 arası değeri %0 - %100 aralığına haritala
            double similarity = Math.Clamp(maxVal, 0.0, 1.0) * 100.0;
            return similarity;
        }

        /// <summary>
        /// Yüz görüntüsünü sabit 100x100 boyutuna ve gri seviye histogram eşitlenmiş hale getirir.
        /// </summary>
        public static Mat NormalizeFace(Mat face)
        {
            var gray = new Mat();
            if (face.Channels() > 1)
            {
                Cv2.CvtColor(face, gray, ColorConversionCodes.BGR2GRAY);
            }
            else
            {
                face.CopyTo(gray);
            }

            var resized = new Mat();
            Cv2.Resize(gray, resized, new OpenCvSharp.Size(100, 100));
            Cv2.EqualizeHist(resized, resized);
            gray.Dispose();

            return resized;
        }

        /// <summary>
        /// Kişi adının örnek dosya adında kullanılabilir olup olmadığını kontrol eder.
        /// Dosya adı "KisiAdi_zaman.jpg" biçiminde olduğu için ad yalnızca harf ve rakamdan oluşmalıdır.
        /// </summary>
        public static bool IsValidPersonName(string? name)
        {
            return !string.IsNullOrWhiteSpace(name)
                && name.Length <= 30
                && name.All(char.IsLetterOrDigit);
        }

        /// <summary>
        /// Yüz görüntüsünü örnek klasörüne kaydeder ve hemen tanıma kümesine ekler.
        /// Kaydedilen dosya, uygulama yeniden açıldığında da örnek olarak yüklenir.
        /// </summary>
        public string AddSampleAndSave(string personName, Mat face, string sampleFolder)
        {
            if (!IsValidPersonName(personName))
            {
                throw new ArgumentException("Kişi adı yalnızca harf ve rakamlardan oluşmalıdır.", nameof(personName));
            }

            string savedPath = SaveCroppedFace(face, sampleFolder, personName);
            _samples.Add(new FaceSample(personName, NormalizeFace(face), savedPath));
            return savedPath;
        }

        /// <summary>
        /// Algılanan yüzü yerel bir klasöre JPG olarak kaydeder.
        /// Cv2.ImWrite Türkçe karakterli yollarda başarısız olabildiği için görüntü önce
        /// belleğe JPG olarak kodlanır, ardından .NET dosya işlemleriyle yazılır.
        /// </summary>
        public static string SaveCroppedFace(Mat face, string targetFolder, string label)
        {
            Directory.CreateDirectory(targetFolder);
            string fileName = $"{label}_{DateTime.Now:yyyyMMdd_HHmmssfff}.jpg";
            string fullPath = Path.Combine(targetFolder, fileName);

            Cv2.ImEncode(".jpg", face, out byte[] jpgBytes);
            File.WriteAllBytes(fullPath, jpgBytes);
            return fullPath;
        }

        public void Dispose()
        {
            ClearSamples();
        }
    }
}
