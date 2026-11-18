using OpenCvSharp;

namespace YMH219_FaceRecognition.Services
{
    /// <summary>
    /// OpenCV Haar Cascade sınıflandırıcısını kullanarak görüntü üzerindeki yüzleri tespit eden servis.
    /// </summary>
    public class FaceDetectionService : IDisposable
    {
        private CascadeClassifier? _cascade;
        private readonly string _modelPath;

        public bool IsLoaded => _cascade != null && !_cascade.Empty();

        public FaceDetectionService(string? customModelPath = null)
        {
            if (string.IsNullOrWhiteSpace(customModelPath))
            {
                // Önce uygulama dizinindeki models klasörüne bak
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string localPath = Path.Combine(baseDir, "models", "haarcascade_frontalface_default.xml");

                if (File.Exists(localPath))
                {
                    _modelPath = localPath;
                }
                else
                {
                    // Proje kök dizini fallback
                    _modelPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "models", "haarcascade_frontalface_default.xml"));
                }
            }
            else
            {
                _modelPath = customModelPath;
            }

            LoadModel();
        }

        private void LoadModel()
        {
            if (!File.Exists(_modelPath))
            {
                throw new FileNotFoundException(
                    $"Yüz algılama modeli (Haar Cascade XML) bulunamadı:\n{_modelPath}\n\nLütfen models/haarcascade_frontalface_default.xml dosyasının mevcut olduğundan emin olun.");
            }

            _cascade = new CascadeClassifier(_modelPath);
            if (_cascade.Empty())
            {
                throw new InvalidOperationException("Haar Cascade modeli yüklenemedi veya bozuk.");
            }
        }

        /// <summary>
        /// Verilen görüntü üzerindeki yüz bölgelerini tespit eder.
        /// </summary>
        public Rect[] DetectFaces(Mat frame)
        {
            if (_cascade == null || _cascade.Empty())
            {
                throw new InvalidOperationException("Yüz algılama modeli henüz yüklenmedi.");
            }

            using var gray = new Mat();
            if (frame.Channels() > 1)
            {
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
            }
            else
            {
                frame.CopyTo(gray);
            }

            // Kontrast iyileştirme
            Cv2.EqualizeHist(gray, gray);

            // Birden fazla yüzü algıla
            return _cascade.DetectMultiScale(
                image: gray,
                scaleFactor: 1.15,
                minNeighbors: 4,
                flags: HaarDetectionTypes.ScaleImage,
                minSize: new OpenCvSharp.Size(40, 40));
        }

        public void Dispose()
        {
            _cascade?.Dispose();
            _cascade = null;
        }
    }
}
