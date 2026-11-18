namespace YMH219_FaceRecognition;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.Panel pnlTop;
    private System.Windows.Forms.Button btnStartCamera;
    private System.Windows.Forms.Button btnStopCamera;
    private System.Windows.Forms.Button btnSelectImage;
    private System.Windows.Forms.Button btnSaveFace;
    private System.Windows.Forms.Label lblStatus;

    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.PictureBox pbVideo;

    private System.Windows.Forms.GroupBox grpResults;
    private System.Windows.Forms.Label lblFaceCount;
    private System.Windows.Forms.ListView lvwFaces;
    private System.Windows.Forms.ColumnHeader colLabel;
    private System.Windows.Forms.ColumnHeader colConfidence;
    private System.Windows.Forms.ColumnHeader colBounds;

    private System.Windows.Forms.GroupBox grpPreview;
    private System.Windows.Forms.PictureBox pbCroppedFace;

    private System.Windows.Forms.GroupBox grpSamples;
    private System.Windows.Forms.ListBox lstSamples;
    private System.Windows.Forms.Button btnReloadSamples;
    private System.Windows.Forms.Panel pnlAddSample;
    private System.Windows.Forms.TextBox txtSampleName;
    private System.Windows.Forms.Button btnAddSample;

    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel lblFooterModel;
    private System.Windows.Forms.ToolStripStatusLabel lblFooterSafety;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        pnlTop = new System.Windows.Forms.Panel();
        btnStartCamera = new System.Windows.Forms.Button();
        btnStopCamera = new System.Windows.Forms.Button();
        btnSelectImage = new System.Windows.Forms.Button();
        btnSaveFace = new System.Windows.Forms.Button();
        lblStatus = new System.Windows.Forms.Label();

        splitMain = new System.Windows.Forms.SplitContainer();
        pbVideo = new System.Windows.Forms.PictureBox();

        grpResults = new System.Windows.Forms.GroupBox();
        lblFaceCount = new System.Windows.Forms.Label();
        lvwFaces = new System.Windows.Forms.ListView();
        colLabel = new System.Windows.Forms.ColumnHeader();
        colConfidence = new System.Windows.Forms.ColumnHeader();
        colBounds = new System.Windows.Forms.ColumnHeader();

        grpPreview = new System.Windows.Forms.GroupBox();
        pbCroppedFace = new System.Windows.Forms.PictureBox();

        grpSamples = new System.Windows.Forms.GroupBox();
        lstSamples = new System.Windows.Forms.ListBox();
        btnReloadSamples = new System.Windows.Forms.Button();
        pnlAddSample = new System.Windows.Forms.Panel();
        txtSampleName = new System.Windows.Forms.TextBox();
        btnAddSample = new System.Windows.Forms.Button();

        statusStrip = new System.Windows.Forms.StatusStrip();
        lblFooterModel = new System.Windows.Forms.ToolStripStatusLabel();
        lblFooterSafety = new System.Windows.Forms.ToolStripStatusLabel();

        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pbVideo).BeginInit();
        grpResults.SuspendLayout();
        grpPreview.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pbCroppedFace).BeginInit();
        grpSamples.SuspendLayout();
        pnlAddSample.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();

        // 
        // pnlTop
        // 
        pnlTop.BackColor = Color.FromArgb(245, 248, 250);
        pnlTop.BorderStyle = BorderStyle.FixedSingle;
        pnlTop.Controls.Add(btnStartCamera);
        pnlTop.Controls.Add(btnStopCamera);
        pnlTop.Controls.Add(btnSelectImage);
        pnlTop.Controls.Add(btnSaveFace);
        pnlTop.Controls.Add(lblStatus);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Name = "pnlTop";
        pnlTop.Size = new Size(960, 50);
        pnlTop.TabIndex = 0;

        // 
        // btnStartCamera
        // 
        btnStartCamera.BackColor = Color.FromArgb(46, 139, 87);
        btnStartCamera.FlatStyle = FlatStyle.Flat;
        btnStartCamera.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnStartCamera.ForeColor = Color.White;
        btnStartCamera.Location = new Point(10, 8);
        btnStartCamera.Name = "btnStartCamera";
        btnStartCamera.Size = new Size(130, 32);
        btnStartCamera.TabIndex = 0;
        btnStartCamera.Text = "📷 Kamera Başlat";
        btnStartCamera.UseVisualStyleBackColor = false;
        btnStartCamera.Click += BtnStartCamera_Click;

        // 
        // btnStopCamera
        // 
        btnStopCamera.BackColor = Color.FromArgb(205, 92, 92);
        btnStopCamera.Enabled = false;
        btnStopCamera.FlatStyle = FlatStyle.Flat;
        btnStopCamera.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnStopCamera.ForeColor = Color.White;
        btnStopCamera.Location = new Point(146, 8);
        btnStopCamera.Name = "btnStopCamera";
        btnStopCamera.Size = new Size(130, 32);
        btnStopCamera.TabIndex = 1;
        btnStopCamera.Text = "⏹ Kamera Durdur";
        btnStopCamera.UseVisualStyleBackColor = false;
        btnStopCamera.Click += BtnStopCamera_Click;

        // 
        // btnSelectImage
        // 
        btnSelectImage.BackColor = Color.FromArgb(70, 130, 180);
        btnSelectImage.FlatStyle = FlatStyle.Flat;
        btnSelectImage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSelectImage.ForeColor = Color.White;
        btnSelectImage.Location = new Point(282, 8);
        btnSelectImage.Name = "btnSelectImage";
        btnSelectImage.Size = new Size(140, 32);
        btnSelectImage.TabIndex = 2;
        btnSelectImage.Text = "🖼️ Görüntü / Resim Seç";
        btnSelectImage.UseVisualStyleBackColor = false;
        btnSelectImage.Click += BtnSelectImage_Click;

        // 
        // btnSaveFace
        // 
        btnSaveFace.BackColor = Color.FromArgb(240, 240, 240);
        btnSaveFace.FlatStyle = FlatStyle.Flat;
        btnSaveFace.Font = new Font("Segoe UI", 9F);
        btnSaveFace.Location = new Point(428, 8);
        btnSaveFace.Name = "btnSaveFace";
        btnSaveFace.Size = new Size(130, 32);
        btnSaveFace.TabIndex = 3;
        btnSaveFace.Text = "💾 Algılanan Yüzü Kaydet";
        btnSaveFace.UseVisualStyleBackColor = true;
        btnSaveFace.Click += BtnSaveFace_Click;

        // 
        // lblStatus
        // 
        lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblStatus.BackColor = Color.FromArgb(235, 235, 235);
        lblStatus.BorderStyle = BorderStyle.FixedSingle;
        lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblStatus.ForeColor = Color.DimGray;
        lblStatus.Location = new Point(680, 9);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(265, 30);
        lblStatus.Text = "○ BEKLEMEDE (Kamera veya Resim Seçin)";
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;

        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.FixedPanel = FixedPanel.Panel2;
        splitMain.Location = new Point(0, 50);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(pbVideo);
        splitMain.Panel1.Padding = new Padding(6);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(grpResults);
        splitMain.Panel2.Controls.Add(grpPreview);
        splitMain.Panel2.Controls.Add(grpSamples);
        splitMain.Panel2.Padding = new Padding(4);
        splitMain.Size = new Size(960, 545);
        splitMain.SplitterDistance = 640;
        splitMain.TabIndex = 1;

        // 
        // pbVideo
        // 
        pbVideo.BackColor = Color.FromArgb(20, 20, 20);
        pbVideo.BorderStyle = BorderStyle.Fixed3D;
        pbVideo.Dock = DockStyle.Fill;
        pbVideo.Location = new Point(6, 6);
        pbVideo.Name = "pbVideo";
        pbVideo.Size = new Size(628, 533);
        pbVideo.SizeMode = PictureBoxSizeMode.Zoom;
        pbVideo.TabIndex = 0;
        pbVideo.TabStop = false;

        // 
        // grpResults
        // 
        grpResults.Controls.Add(lblFaceCount);
        grpResults.Controls.Add(lvwFaces);
        grpResults.Dock = DockStyle.Top;
        grpResults.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpResults.Location = new Point(4, 4);
        grpResults.Name = "grpResults";
        grpResults.Padding = new Padding(6);
        grpResults.Size = new Size(308, 190);
        grpResults.TabIndex = 0;
        grpResults.TabStop = false;
        grpResults.Text = "🎯 Yüz Tanıma Sonuçları";

        // 
        // lblFaceCount
        // 
        lblFaceCount.AutoSize = true;
        lblFaceCount.Font = new Font("Segoe UI", 9F);
        lblFaceCount.ForeColor = Color.DarkSlateBlue;
        lblFaceCount.Location = new Point(9, 22);
        lblFaceCount.Name = "lblFaceCount";
        lblFaceCount.Size = new Size(130, 15);
        lblFaceCount.TabIndex = 0;
        lblFaceCount.Text = "Algılanan Yüz Sayısı: 0";

        // 
        // lvwFaces
        // 
        lvwFaces.Columns.AddRange(new ColumnHeader[] {
            colLabel,
            colConfidence,
            colBounds
        });
        lvwFaces.Dock = DockStyle.Bottom;
        lvwFaces.Font = new Font("Segoe UI", 8.5F);
        lvwFaces.FullRowSelect = true;
        lvwFaces.GridLines = true;
        lvwFaces.Location = new Point(6, 45);
        lvwFaces.Name = "lvwFaces";
        lvwFaces.Size = new Size(296, 139);
        lvwFaces.TabIndex = 1;
        lvwFaces.View = View.Details;

        colLabel.Text = "Tanınan Kişi";
        colLabel.Width = 95;
        colConfidence.Text = "Güven";
        colConfidence.Width = 60;
        colBounds.Text = "Konum [X,Y,W,H]";
        colBounds.Width = 135;

        // 
        // grpPreview
        // 
        grpPreview.Controls.Add(pbCroppedFace);
        grpPreview.Dock = DockStyle.Fill;
        grpPreview.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpPreview.Location = new Point(4, 194);
        grpPreview.Name = "grpPreview";
        grpPreview.Padding = new Padding(6);
        grpPreview.Size = new Size(308, 167);
        grpPreview.TabIndex = 1;
        grpPreview.TabStop = false;
        grpPreview.Text = "👤 Kırpılan Yüz Önizlemesi";

        pbCroppedFace.BackColor = Color.FromArgb(240, 240, 240);
        pbCroppedFace.BorderStyle = BorderStyle.FixedSingle;
        pbCroppedFace.Dock = DockStyle.Fill;
        pbCroppedFace.Location = new Point(6, 22);
        pbCroppedFace.Name = "pbCroppedFace";
        pbCroppedFace.Size = new Size(296, 139);
        pbCroppedFace.SizeMode = PictureBoxSizeMode.Zoom;
        pbCroppedFace.TabIndex = 0;
        pbCroppedFace.TabStop = false;

        // 
        // grpSamples
        // 
        grpSamples.Controls.Add(lstSamples);
        grpSamples.Controls.Add(btnReloadSamples);
        grpSamples.Controls.Add(pnlAddSample);
        grpSamples.Dock = DockStyle.Bottom;
        grpSamples.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpSamples.Location = new Point(4, 326);
        grpSamples.Name = "grpSamples";
        grpSamples.Padding = new Padding(6);
        grpSamples.Size = new Size(308, 215);
        grpSamples.TabIndex = 2;
        grpSamples.TabStop = false;
        grpSamples.Text = "📁 Kayıtlı Örnekler (sample_data/)";

        lstSamples.Dock = DockStyle.Fill;
        lstSamples.Font = new Font("Segoe UI", 8.5F);
        lstSamples.FormattingEnabled = true;
        lstSamples.ItemHeight = 13;
        lstSamples.Location = new Point(6, 22);
        lstSamples.Name = "lstSamples";
        lstSamples.Size = new Size(296, 122);
        lstSamples.TabIndex = 0;

        btnReloadSamples.Dock = DockStyle.Bottom;
        btnReloadSamples.Font = new Font("Segoe UI", 8.5F);
        btnReloadSamples.Location = new Point(6, 144);
        btnReloadSamples.Name = "btnReloadSamples";
        btnReloadSamples.Size = new Size(296, 30);
        btnReloadSamples.TabIndex = 1;
        btnReloadSamples.Text = "🔄 Örnekleri Yeniden Tara";
        btnReloadSamples.UseVisualStyleBackColor = true;
        btnReloadSamples.Click += BtnReloadSamples_Click;

        //
        // pnlAddSample (Örnek Ekle: kişi adı + buton)
        //
        pnlAddSample.Controls.Add(txtSampleName);
        pnlAddSample.Controls.Add(btnAddSample);
        pnlAddSample.Dock = DockStyle.Bottom;
        pnlAddSample.Location = new Point(6, 179);
        pnlAddSample.Name = "pnlAddSample";
        pnlAddSample.Padding = new Padding(0, 4, 0, 0);
        pnlAddSample.Size = new Size(296, 30);
        pnlAddSample.TabIndex = 2;

        txtSampleName.Dock = DockStyle.Fill;
        txtSampleName.Font = new Font("Segoe UI", 9F);
        txtSampleName.MaxLength = 30;
        txtSampleName.Name = "txtSampleName";
        txtSampleName.PlaceholderText = "Kişi adı (ör. Birgul)";
        txtSampleName.TabIndex = 0;

        btnAddSample.BackColor = Color.FromArgb(46, 139, 87);
        btnAddSample.Dock = DockStyle.Right;
        btnAddSample.FlatStyle = FlatStyle.Flat;
        btnAddSample.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnAddSample.ForeColor = Color.White;
        btnAddSample.Name = "btnAddSample";
        btnAddSample.Size = new Size(110, 26);
        btnAddSample.TabIndex = 1;
        btnAddSample.Text = "➕ Örnek Ekle";
        btnAddSample.UseVisualStyleBackColor = false;
        btnAddSample.Click += BtnAddSample_Click;

        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] {
            lblFooterModel,
            lblFooterSafety
        });
        statusStrip.Location = new Point(0, 595);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(960, 22);
        statusStrip.TabIndex = 2;

        lblFooterModel.Name = "lblFooterModel";
        lblFooterModel.Size = new Size(420, 17);
        lblFooterModel.Spring = true;
        lblFooterModel.Text = "Model: haarcascade_frontalface_default.xml | OpenCV: OpenCvSharp4";
        lblFooterModel.TextAlign = ContentAlignment.MiddleLeft;

        lblFooterSafety.ForeColor = Color.DarkSlateGray;
        lblFooterSafety.Name = "lblFooterSafety";
        lblFooterSafety.Size = new Size(420, 17);
        lblFooterSafety.Spring = true;
        lblFooterSafety.Text = "Kapsam: Yalnızca ders kapsamında yerel ve gönüllü örneklerle kullanılır.";
        lblFooterSafety.TextAlign = ContentAlignment.MiddleRight;

        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 617);
        Controls.Add(splitMain);
        Controls.Add(pnlTop);
        Controls.Add(statusStrip);
        MinimumSize = new Size(820, 540);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "YMH219 - OpenCV ile Yüz Algılama ve Tanıma";
        FormClosing += Form1_FormClosing;
        pnlTop.ResumeLayout(false);
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pbVideo).EndInit();
        grpResults.ResumeLayout(false);
        grpResults.PerformLayout();
        grpPreview.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pbCroppedFace).EndInit();
        grpSamples.ResumeLayout(false);
        pnlAddSample.ResumeLayout(false);
        pnlAddSample.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
