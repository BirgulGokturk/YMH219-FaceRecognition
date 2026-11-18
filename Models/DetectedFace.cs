using OpenCvSharp;

namespace YMH219_FaceRecognition.Models
{
    /// <summary>
    /// Görüntü üzerinde tespit edilen ve tanınan bir yüzün bilgilerini saklayan model.
    /// </summary>
    public class DetectedFace
    {
        public Rect Bounds { get; set; }
        public string Label { get; set; } = "Bilinmiyor";
        public double Confidence { get; set; } // 0 - 100 arası yüzde güven skoru
        public Mat? CroppedFace { get; set; }

        public string DisplayText => $"{Label} (%{Confidence:0.0})";

        public override string ToString()
        {
            return $"Konum: [X:{Bounds.X}, Y:{Bounds.Y}, W:{Bounds.Width}, H:{Bounds.Height}] | {DisplayText}";
        }
    }
}
