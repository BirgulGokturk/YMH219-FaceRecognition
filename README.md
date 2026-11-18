# YMH219 - OpenCV ile Yüz Algılama ve Tanıma

Bu proje, Fırat Üniversitesi **Nesne Tabanlı Programlama (YMH 219)** dersi kapsamında Windows Forms ortamında C# ve OpenCV (`OpenCvSharp4`) kütüphanesi kullanılarak geliştirilmiş yüz algılama (Face Detection) ve yüz tanıma (Face Recognition) masaüstü uygulamasıdır.

---

## 📌 Projenin Amacı ve Kısa Açıklama

Projenin temel amacı; görüntü işleme ve bilgisayarla görme (Computer Vision) kavramlarını nesne yönelimli mimariyle uygulamak, Haar Cascade sınıflandırıcısı ile yüz bölgesini gerçek zamanlı tespit etmek ve yerel örnek veri kümesi üzerinden kişi tanıma/etiketleme demonstrasyonu gerçekleştirmektir. Uygulama hem **canlı kamera akışından** hem de **bilgisayardan seçilen resim dosyalarından** görüntü işleme yapabilmektedir.

---

## 🛡️ Kapsam ve Güvenlik Sınırı

> **Önemli Güvenlik ve Etik Bildirimi (Föy Yönergesi):**
> Bu çalışma yalnızca ders kapsamında yerel ve gönüllü örneklerle yapılmalıdır. Kamuya açık alanlarda izinsiz kimlik takibi, gizli görüntü toplama veya gerçek kişileri uzaktan izlemeye yönelik kullanım **kesinlikle yapılmamalıdır**.

---

## 🛠️ Kullanılan Teknolojiler ve Kütüphaneler

- **Programlama Dili:** C# (.NET 10.0 / Windows Desktop SDK)
- **Görüntü İşleme Kütüphanesi:** OpenCvSharp4 (v4.13.0)
  - `OpenCvSharp4` (Çekirdek OpenCV motoru)
  - `OpenCvSharp4.Windows` (Yerel Windows C++ kütüphane bağlayıcıları)
  - `OpenCvSharp4.Extensions` (OpenCV Mat ve Windows Forms Bitmap dönüşümü)
- **Algoritma ve Modeller:** Haar Cascade Sınıflandırıcısı (`haarcascade_frontalface_default.xml`)
- **Arayüz:** Windows Forms (WinForms, PictureBox, ListView, SplitContainer)
- **Sürüm Kontrolü:** Git & GitHub

---

## 🚀 Kurulum ve Çalıştırma Adımları

1. Proje dizinine gidin:
   ```bash
   cd YMH219-FaceRecognition
   ```
2. Projeyi derleyin ve çalıştırın:
   ```bash
   dotnet build
   dotnet run
   ```
   *Veya `YMH219_FaceRecognition.sln` dosyasını Visual Studio ile açarak F5 tuşu ile başlatabilirsiniz.*

### Test ve Kullanım:
- **Resim Dosyası ile Test (Kamera Olmayan Cihazlar İçin):**
  - "🖼️ Görüntü / Resim Seç" butonuna tıklayın.
  - Açılan pencerede hazır gelen `sample_data/` klasöründeki fotoğraflardan birini (örn: `Ali_1.jpg` veya `Ayse_1.jpg`) seçin.
  - Yüz anında yeşil dikdörtgenle işaretlenecek, kişi adı ve güven skoru görüntülenecektir.
- **Kamera ile Canlı Test (Webcam Varsa):**
  - "📷 Kamera Başlat" butonuna tıklayarak web kameranızı açabilir ve anlık yüz takibi yapabilirsiniz.

---

## ⚙️ Uygulamanın Temel Özellikleri

### 1. Asgari Gereksinimler
- **Kamera ve Dosya Desteği:** Canlı webcam görüntüsü veya bilgisayardan seçilen herhangi bir fotoğraf (`.jpg`, `.png`, `.bmp`) PictureBox üzerinde görüntülenir.
- **Yüz Bölgesi İşaretleme:** Algılanan yüzler yeşil sınırlayıcı dikdörtgen (`Cv2.Rectangle`) ile işaretlenir.
- **Etiket ve Güven Skoru:** Tanınan kişinin adı ve yüzde benzerlik/güven skoru (`% Confidence`) yüzün üstünde ve sağdaki sonuç tablosunda listelenir.
- **Yerel Örnek Küme (En Az İki Sınıf):** `sample_data/` klasörü içinde iki farklı örnek sınıf (`Ali` ve `Ayse`) tanımlanmıştır.
- **Hata ve Donanım Kontrolü:** Bilgisayarda kamera bulunamadığında veya model dosyası eksik olduğunda uygulama çökmez, kullanıcıya açıklayıcı hata ve yönlendirme mesajı gösterir.

### 2. İsteğe Bağlı / Ek Geliştirmeler (Bonus)
- 💾 **Algılanan Yüzü Kaydetme (Face Cropping):** "💾 Algılanan Yüzü Kaydet" butonu ile tespit edilen yüz kırpılarak `saved_faces/` klasörüne zaman damgalı olarak arşivlenebilir.
- 👥 **Çoklu Yüz Algılama:** Aynı karede birden fazla yüz varsa her biri ayrı ayrı dikdörtgen içine alınır ve etiketlenir.
- 🔄 **Örnekleri Yeniden Tarama:** "🔄 Örnekleri Yeniden Tara" butonu ile `sample_data/` klasörüne sonradan eklenen yeni fotoğraflar anında hafızaya dahil edilir.

---

## 📸 Ekran Görüntüleri

| Örnek Kişi 1 (Ali - Yüz Tespiti ve Tanıma) | Örnek Kişi 2 (Ayşe - Yüz Tespiti ve Tanıma) |
|:---:|:---:|
| ![Ekran Görüntüsü 1](screenshots/ekran1.png) | ![Ekran Görüntüsü 2](screenshots/ekran2.png) |

---

## 🏗️ Nesne Tabanlı Mimari

```
YMH219-FaceRecognition/
├── Models/
│   ├── DetectedFace.cs        # Tespit edilen yüz koordinatları, etiket ve güven skoru
│   └── FaceSample.cs          # Tanıma veri setindeki referans yüz modeli
├── Services/
│   ├── FaceDetectionService.cs    # Haar Cascade yüz tespit sınıfı
│   ├── FaceRecognitionService.cs  # Şablon/öznitelik eşleme ve güven skoru hesaplayıcı
│   └── CameraService.cs           # Asenkron VideoCapture ve donanım kontrol servisi
├── models/
│   └── haarcascade_frontalface_default.xml # OpenCV yüz tespit modeli
├── sample_data/
│   ├── Ali_1.jpg              # Örnek sınıf 1 referans yüzleri
│   ├── Ali_2.jpg
│   ├── Ayse_1.jpg             # Örnek sınıf 2 referans yüzleri
│   └── Ayse_2.jpg
├── screenshots/
│   ├── ekran1.png
│   └── ekran2.png
├── Form1.cs                   # Form olayları, görüntü işleme akışı ve UI bağlama
├── Form1.Designer.cs          # Görüntü paneli, sonuç listesi ve butonlar
├── SampleDataGenerator.cs     # Otomatik test ve örnek veri oluşturucu
├── Program.cs                 # Giriş noktası
├── YMH219_FaceRecognition.csproj
└── YMH219_FaceRecognition.sln
```

---

## 📝 Bilinen Sınırlamalar / Geliştirme Notları

- Işıklandırma koşulları, aşırı yüz eğimi ve kamera çözünürlüğü tespit ve tanıma oranını etkileyebilir.
- Gelecek versiyonlarda derin öğrenme tabanlı modeller (DNN / Dlib / FaceNet) entegre edilebilir.

---

## 👤 Öğrenci Bilgileri

- **Adı Soyadı:** Birgül Göktürk
- **Öğrenci Numarası:** 250541094
- **Ders:** Nesne Tabanlı Programlama (YMH 219)
- **Dönem:** 2026-2027 Güz
