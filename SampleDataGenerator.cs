using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace YMH219_FaceRecognition;

public static class SampleDataGenerator
{
    public static void GenerateDefaultSamples(string folderPath)
    {
        Directory.CreateDirectory(folderPath);

        // 1. Kişi: Ali (Kısa saçlı, mavi arka planlı örnekler)
        string ali1 = Path.Combine(folderPath, "Ali_1.jpg");
        string ali2 = Path.Combine(folderPath, "Ali_2.jpg");
        if (!File.Exists(ali1)) CreateFaceImage(ali1, "Ali", Color.FromArgb(210, 160, 120), isMale: true);
        if (!File.Exists(ali2)) CreateFaceImage(ali2, "Ali", Color.FromArgb(215, 165, 125), isMale: true);

        // 2. Kişi: Ayse (Uzun saçlı, açık tenli örnekler)
        string ayse1 = Path.Combine(folderPath, "Ayse_1.jpg");
        string ayse2 = Path.Combine(folderPath, "Ayse_2.jpg");
        if (!File.Exists(ayse1)) CreateFaceImage(ayse1, "Ayse", Color.FromArgb(240, 195, 165), isMale: false);
        if (!File.Exists(ayse2)) CreateFaceImage(ayse2, "Ayse", Color.FromArgb(245, 200, 170), isMale: false);
    }

    private static void CreateFaceImage(string path, string name, Color skinColor, bool isMale)
    {
        int w = 240;
        int h = 260;
        using var bmp = new Bitmap(w, h);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Arka plan
        Color bg = isMale ? Color.FromArgb(220, 230, 242) : Color.FromArgb(245, 230, 235);
        g.Clear(bg);

        // Saç (Arka)
        if (!isMale)
        {
            using var hairBrush = new SolidBrush(Color.FromArgb(50, 30, 20));
            g.FillEllipse(hairBrush, 35, 30, 170, 200);
        }

        // Boyun
        using (var neckBrush = new SolidBrush(Color.FromArgb(skinColor.R - 20, skinColor.G - 20, skinColor.B - 20)))
        {
            g.FillRectangle(neckBrush, 95, 160, 50, 60);
        }

        // Yüz (Oval)
        using (var faceBrush = new SolidBrush(skinColor))
        using (var borderPen = new Pen(Color.FromArgb(140, 90, 60), 1.5f))
        {
            g.FillEllipse(faceBrush, 55, 45, 130, 155);
            g.DrawEllipse(borderPen, 55, 45, 130, 155);
        }

        // Saç (Ön / Üst)
        using (var frontHairBrush = new SolidBrush(isMale ? Color.FromArgb(40, 25, 15) : Color.FromArgb(60, 35, 20)))
        {
            if (isMale)
            {
                g.FillEllipse(frontHairBrush, 50, 30, 140, 60);
            }
            else
            {
                g.FillPie(frontHairBrush, 45, 30, 150, 90, 180, 180);
            }
        }

        // Kaşlar (Haar cascade için koyu yatay bantlar kritik önem taşır)
        using (var browBrush = new SolidBrush(Color.FromArgb(40, 25, 15)))
        {
            g.FillRectangle(browBrush, 75, 90, 30, 6);
            g.FillRectangle(browBrush, 135, 90, 30, 6);
        }

        // Gözler (Beyaz + koyu gözbebeği)
        using (var eyeWhite = new SolidBrush(Color.White))
        using (var eyePupil = new SolidBrush(Color.FromArgb(20, 20, 20)))
        using (var eyePen = new Pen(Color.FromArgb(40, 25, 15), 1.5f))
        {
            // Sol göz
            g.FillEllipse(eyeWhite, 78, 102, 24, 14);
            g.DrawEllipse(eyePen, 78, 102, 24, 14);
            g.FillEllipse(eyePupil, 85, 104, 10, 10);

            // Sağ göz
            g.FillEllipse(eyeWhite, 138, 102, 24, 14);
            g.DrawEllipse(eyePen, 138, 102, 24, 14);
            g.FillEllipse(eyePupil, 145, 104, 10, 10);
        }

        // Burun
        using (var nosePen = new Pen(Color.FromArgb(160, 110, 80), 2f))
        {
            g.DrawLine(nosePen, 120, 108, 120, 135);
            g.DrawLine(nosePen, 115, 135, 125, 135);
        }

        // Ağız
        using (var mouthBrush = new SolidBrush(isMale ? Color.FromArgb(170, 75, 75) : Color.FromArgb(200, 60, 80)))
        using (var mouthPen = new Pen(Color.FromArgb(110, 40, 40), 1.5f))
        {
            g.FillPie(mouthBrush, 98, 150, 44, 20, 0, 180);
            g.DrawArc(mouthPen, 98, 150, 44, 20, 0, 180);
        }

        // Etiket
        using (var font = new Font("Segoe UI", 9, FontStyle.Bold))
        {
            g.DrawString($"Örnek: {name}", font, Brushes.DimGray, 10, 10);
        }

        bmp.Save(path, ImageFormat.Jpeg);
    }
}
