using OpenCvSharp;

namespace YMH219_FaceRecognition.Services
{
    /// <summary>
    /// Kamera bağlantısını ve video akışını asenkron olarak yöneten servis.
    /// </summary>
    public class CameraService : IDisposable
    {
        private VideoCapture? _capture;
        private CancellationTokenSource? _cts;
        private Task? _captureTask;

        public bool IsRunning => _capture != null && _capture.IsOpened() && _cts != null && !_cts.IsCancellationRequested;

        public event Action<Mat>? FrameReceived;
        public event Action<string>? ErrorOccurred;

        /// <summary>
        /// Kamerayı başlatır. Kamera cihazı bulunamazsa hata fırlatır.
        /// </summary>
        public void Start(int cameraIndex = 0)
        {
            if (IsRunning) return;

            try
            {
                _capture = new VideoCapture(cameraIndex);

                if (!_capture.IsOpened())
                {
                    _capture.Dispose();
                    _capture = null;
                    throw new InvalidOperationException("Kamera cihazı bulunamadı veya açılamadı. Lütfen bilgisayarınıza bir web kamerası bağlı olduğundan emin olunuz veya 'Resim Seç' seçeneğini kullanınız.");
                }

                _cts = new CancellationTokenSource();
                var token = _cts.Token;

                _captureTask = Task.Run(() => CaptureLoop(token), token);
            }
            catch (Exception ex)
            {
                Stop();
                ErrorOccurred?.Invoke(ex.Message);
                throw;
            }
        }

        private void CaptureLoop(CancellationToken token)
        {
            using var frame = new Mat();

            while (!token.IsCancellationRequested && _capture != null && _capture.IsOpened())
            {
                try
                {
                    if (_capture.Read(frame) && !frame.Empty())
                    {
                        // UI thread'ine iletmek üzere karenin kopyasını gönder
                        FrameReceived?.Invoke(frame.Clone());
                    }
                    Thread.Sleep(33); // Yaklaşık 30 FPS
                }
                catch
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Kamera akışını durdurur.
        /// </summary>
        public void Stop()
        {
            _cts?.Cancel();
            try { _captureTask?.Wait(500); } catch { }

            _capture?.Release();
            _capture?.Dispose();
            _capture = null;
            _cts?.Dispose();
            _cts = null;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
