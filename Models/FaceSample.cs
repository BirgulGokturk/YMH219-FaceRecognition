using OpenCvSharp;

namespace YMH219_FaceRecognition.Models
{
    /// <summary>
    /// Tanıma veri tabanındaki kayıtlı bir kişiye ait referans yüz örneği.
    /// </summary>
    public class FaceSample
    {
        public string PersonName { get; set; } = string.Empty;
        public Mat NormalizedFace { get; set; } // 100x100 gri seviye normalleştirilmiş yüz
        public string SourcePath { get; set; } = string.Empty;

        public FaceSample(string personName, Mat normalizedFace, string sourcePath = "")
        {
            PersonName = personName;
            NormalizedFace = normalizedFace;
            SourcePath = sourcePath;
        }
    }
}
