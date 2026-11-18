using OpenCvSharp;
using OpenCvSharp.Extensions;
using YMH219_FaceRecognition.Models;
using YMH219_FaceRecognition.Services;

namespace YMH219_FaceRecognition;

public partial class Form1 : Form
{
    private FaceDetectionService? _detector;
    private FaceRecognitionService? _recognizer;
    private readonly CameraService _camera = new();

    private Mat? _currentFrame;
    private Mat? _lastCroppedFace;
    private readonly List<DetectedFace> _currentDetections = new();
    private readonly object _frameLock = new();

    public Form1()
    {
        InitializeComponent();

        InitializeOpenCvServices();
        _camera.FrameReceived += Camera_FrameReceived;
    }

    private void InitializeOpenCvServices()
    {
        try
        {
            _detector = new FaceDetectionService();
            _recognizer = new FaceRecognitionService();

            // Örnek veri setini oluştur ve yükle
            string sampleFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample_data");
            SampleDataGenerator.GenerateDefaultSamples(sampleFolder);

            int loaded = _recognizer.LoadSamplesFromDirectory(sampleFolder, _detector);
            UpdateSamplesList();

            lblStatus.Text = $"○ Hazır (Kayıtlı Örnek: {loaded} adet)";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"OpenCV servisleri başlatılırken hata oluştu:\n{ex.Message}",
                "Model Hatası",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void UpdateSamplesList()
    {
        lstSamples.Items.Clear();
        if (_recognizer == null) return;

        foreach (var sample in _recognizer.Samples)
        {
            string fileName = Path.GetFileName(sample.SourcePath);
            lstSamples.Items.Add($"{sample.PersonName} ({fileName})");
        }
    }

    #region Kamera Olayları

    private void BtnStartCamera_Click(object? sender, EventArgs e)
    {
        try
        {
            _camera.Start(0);
            btnStartCamera.Enabled = false;
            btnStopCamera.Enabled = true;
            btnSelectImage.Enabled = false;

            lblStatus.Text = "● KAMERA AKTİF (Yüz Aranıyor...)";
            lblStatus.BackColor = Color.FromArgb(220, 245, 220);
            lblStatus.ForeColor = Color.DarkGreen;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Kamera Başlatılamadı:\n{ex.Message}\n\nİpucu: Bilgisayarınızda bağlı bir web kamerası yoksa, 'Görüntü / Resim Seç' butonunu kullanarak fotoğraflar üzerinden yüz algılama ve tanıma yapabilirsiniz.",
                "Kamera Bulunamadı",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            btnStartCamera.Enabled = true;
            btnStopCamera.Enabled = false;
            btnSelectImage.Enabled = true;
        }
    }

    private void BtnStopCamera_Click(object? sender, EventArgs e)
    {
        _camera.Stop();
        btnStartCamera.Enabled = true;
        btnStopCamera.Enabled = false;
        btnSelectImage.Enabled = true;

        lblStatus.Text = "○ Kamera Durduruldu";
        lblStatus.BackColor = Color.FromArgb(235, 235, 235);
        lblStatus.ForeColor = Color.DimGray;
    }

    private void Camera_FrameReceived(Mat frame)
    {
        if (IsDisposed)
        {
            frame.Dispose();
            return;
        }

        ProcessAndDisplayFrame(frame);
    }

    #endregion

    #region Görüntü / Resim Seçme

    private void BtnSelectImage_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tüm Dosyalar (*.*)|*.*",
            Title = "Yüz Algılama İçin Bir Fotoğraf Seçin",
            InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample_data")
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var imageMat = Cv2.ImRead(ofd.FileName, ImreadModes.Color);
                if (imageMat.Empty())
                {
                    MessageBox.Show("Seçilen resim dosyası okunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                lblStatus.Text = $"🖼️ Resim Yüklendi: {Path.GetFileName(ofd.FileName)}";
                lblStatus.BackColor = Color.FromArgb(235, 245, 255);
                lblStatus.ForeColor = Color.DarkBlue;

                ProcessAndDisplayFrame(imageMat);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Resim işlenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    #endregion

    #region Yüz Algılama, Tanıma ve Çizim

    private void ProcessAndDisplayFrame(Mat frame)
    {
        if (_detector == null || _recognizer == null)
        {
            frame.Dispose();
            return;
        }

        lock (_frameLock)
        {
            _currentFrame?.Dispose();
            _currentFrame = frame.Clone();

            _currentDetections.Clear();

            // 1. Yüzleri algıla
            var faceRects = _detector.DetectFaces(frame);

            // 2. Her algılanan yüzü tanı ve işaretle
            for (int i = 0; i < faceRects.Length; i++)
            {
                var rect = faceRects[i];

                // Yüz bölgesini güvenli sınırlarla kırp
                rect.X = Math.Max(0, rect.X);
                rect.Y = Math.Max(0, rect.Y);
                rect.Width = Math.Min(rect.Width, frame.Width - rect.X);
                rect.Height = Math.Min(rect.Height, frame.Height - rect.Y);

                if (rect.Width <= 0 || rect.Height <= 0) continue;

                using var cropped = new Mat(frame, rect);

                // Yüzü tanı
                var (label, confidence) = _recognizer.Recognize(cropped);

                var detection = new DetectedFace
                {
                    Bounds = rect,
                    Label = label,
                    Confidence = confidence,
                    CroppedFace = cropped.Clone()
                };

                _currentDetections.Add(detection);

                // Çizim: Yeşil dikdörtgen
                var boxColor = label != "Bilinmiyor" ? Scalar.LimeGreen : Scalar.Gold;
                Cv2.Rectangle(frame, rect, boxColor, 2);

                // Çizim: Etiket arka planı ve metin
                string tagText = $"{label} (%{confidence:0.0})";
                int baseLine = 0;
                var textSize = Cv2.GetTextSize(tagText, HersheyFonts.HersheySimplex, 0.55, 1, out baseLine);
                var textOrigin = new OpenCvSharp.Point(rect.X, Math.Max(rect.Y - 8, textSize.Height + 5));

                var bgRect = new Rect(textOrigin.X, textOrigin.Y - textSize.Height - 3, textSize.Width + 6, textSize.Height + 6);
                Cv2.Rectangle(frame, bgRect, boxColor, -1);
                Cv2.PutText(frame, tagText, new OpenCvSharp.Point(textOrigin.X + 3, textOrigin.Y), HersheyFonts.HersheySimplex, 0.55, Scalar.Black, 1, LineTypes.AntiAlias);
            }

            // UI'ya aktar
            var displayBitmap = BitmapConverter.ToBitmap(frame);
            frame.Dispose();

            if (IsDisposed)
            {
                displayBitmap.Dispose();
                return;
            }

            BeginInvoke(() =>
            {
                var oldBmp = pbVideo.Image;
                pbVideo.Image = displayBitmap;
                oldBmp?.Dispose();

                UpdateResultsUI();
            });
        }
    }

    private void UpdateResultsUI()
    {
        lblFaceCount.Text = $"Algılanan Yüz Sayısı: {_currentDetections.Count}";

        lvwFaces.BeginUpdate();
        lvwFaces.Items.Clear();

        foreach (var det in _currentDetections)
        {
            var lvi = new ListViewItem(det.Label);
            lvi.SubItems.Add($"%{det.Confidence:0.0}");
            lvi.SubItems.Add($"[{det.Bounds.X},{det.Bounds.Y},{det.Bounds.Width},{det.Bounds.Height}]");

            if (det.Label != "Bilinmiyor")
                lvi.ForeColor = Color.DarkGreen;
            else
                lvi.ForeColor = Color.DarkGoldenrod;

            lvwFaces.Items.Add(lvi);
        }

        lvwFaces.EndUpdate();

        // Kırpılan ilk yüzün önizlemesini göster
        if (_currentDetections.Count > 0 && _currentDetections[0].CroppedFace != null)
        {
            var faceMat = _currentDetections[0].CroppedFace!;
            _lastCroppedFace?.Dispose();
            _lastCroppedFace = faceMat.Clone();

            var oldPreview = pbCroppedFace.Image;
            pbCroppedFace.Image = BitmapConverter.ToBitmap(faceMat);
            oldPreview?.Dispose();
        }
    }

    #endregion

    #region İsteğe Bağlı Ek İşlevler

    private void BtnSaveFace_Click(object? sender, EventArgs e)
    {
        if (_lastCroppedFace == null || _lastCroppedFace.Empty())
        {
            MessageBox.Show("Kaydedilecek algılanmış bir yüz bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            string targetFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saved_faces");
            string label = _currentDetections.Count > 0 ? _currentDetections[0].Label : "Yuz";
            string savedPath = FaceRecognitionService.SaveCroppedFace(_lastCroppedFace, targetFolder, label);

            MessageBox.Show($"Yüz başarıyla kaydedildi:\n{savedPath}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Yüz kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnReloadSamples_Click(object? sender, EventArgs e)
    {
        if (_detector == null || _recognizer == null) return;

        string sampleFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample_data");
        int count = _recognizer.LoadSamplesFromDirectory(sampleFolder, _detector);
        UpdateSamplesList();
        MessageBox.Show($"Örnekler yeniden yüklendi. Toplam: {count} adet referans yüz hazır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    #endregion

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _camera.Stop();
        _detector?.Dispose();
        _recognizer?.Dispose();
        _currentFrame?.Dispose();
        _lastCroppedFace?.Dispose();
    }
}
