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
        /// Belirtilen klasördeki kişi fotoğraflarını tarayarak veri setini yükler.
        /// Klasör yapısı: sample_data/KisiAdi/resim1.jpg veya sample_data/KisiAdi_1.jpg
        /// </summary>
        public int LoadSamplesFromDirectory(string directoryPath, FaceDetectionService detector)
        {
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
                    using var img = Cv2.ImRead(file, ImreadModes.Color);
                    if (img.Empty()) continue;

                    string personName = Path.GetFileNameWithoutExtension(file).Split('_')[0];

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
        /// Algılanan yüzü yerel bir klasöre PNG/JPG olarak kaydeder.
        /// </summary>
        public static string SaveCroppedFace(Mat face, string targetFolder, string label)
        {
            Directory.CreateDirectory(targetFolder);
            string fileName = $"{label}_{DateTime.Now:yyyyMMdd_HHmmssfff}.jpg";
            string fullPath = Path.Combine(targetFolder, fileName);
            Cv2.ImWrite(fullPath, face);
            return fullPath;
        }

        public void Dispose()
        {
            foreach (var s in _samples)
            {
                s.NormalizedFace.Dispose();
            }
            _samples.Clear();
        }
    }
}
