namespace parser_HH
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            btnStartParse = new Button();
            txtURL = new TextBox();
            btnOpenLink = new Button();
            label0 = new Label();
            label1 = new Label();
            label2 = new Label();
            btnCheck = new Button();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            SuspendLayout();
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            webView.BackColor = Color.Silver;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Location = new Point(0, 98);
            webView.Name = "webView";
            webView.Size = new Size(802, 437);
            webView.TabIndex = 0;
            webView.ZoomFactor = 1D;
            webView.NavigationCompleted += webView_NavigationCompleted;
            // 
            // btnStartParse
            // 
            btnStartParse.BackColor = Color.Green;
            btnStartParse.Cursor = Cursors.Hand;
            btnStartParse.FlatStyle = FlatStyle.Flat;
            btnStartParse.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnStartParse.ForeColor = Color.White;
            btnStartParse.Location = new Point(12, 62);
            btnStartParse.Name = "btnStartParse";
            btnStartParse.Size = new Size(191, 30);
            btnStartParse.TabIndex = 1;
            btnStartParse.Text = "Начать сбор данных";
            btnStartParse.UseVisualStyleBackColor = false;
            btnStartParse.Click += btnStartParse_Click;
            // 
            // txtURL
            // 
            txtURL.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtURL.Location = new Point(12, 29);
            txtURL.Name = "txtURL";
            txtURL.Size = new Size(629, 27);
            txtURL.TabIndex = 2;
            txtURL.Text = resources.GetString("txtURL.Text");
            // 
            // btnOpenLink
            // 
            btnOpenLink.BackColor = Color.RoyalBlue;
            btnOpenLink.Cursor = Cursors.Hand;
            btnOpenLink.FlatStyle = FlatStyle.Flat;
            btnOpenLink.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnOpenLink.ForeColor = Color.White;
            btnOpenLink.Location = new Point(644, 28);
            btnOpenLink.Margin = new Padding(0);
            btnOpenLink.Name = "btnOpenLink";
            btnOpenLink.Size = new Size(141, 29);
            btnOpenLink.TabIndex = 3;
            btnOpenLink.Text = "Открыть ссылку";
            btnOpenLink.UseVisualStyleBackColor = false;
            btnOpenLink.Click += btnOpenLink_Click;
            // 
            // label0
            // 
            label0.AutoSize = true;
            label0.ForeColor = Color.FromArgb(224, 224, 224);
            label0.Location = new Point(12, 11);
            label0.Name = "label0";
            label0.Size = new Size(133, 15);
            label0.TabIndex = 4;
            label0.Text = "Произвольная ссылка:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(219, 71);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 5;
            label1.Text = "label2";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(376, 71);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 6;
            label2.Text = "label3";
            // 
            // btnCheck
            // 
            btnCheck.Location = new Point(710, 67);
            btnCheck.Name = "btnCheck";
            btnCheck.Size = new Size(75, 23);
            btnCheck.TabIndex = 7;
            btnCheck.Text = "button1";
            btnCheck.UseVisualStyleBackColor = true;
            btnCheck.Visible = false;
            btnCheck.Click += btnCheck_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Navy;
            ClientSize = new Size(800, 532);
            Controls.Add(btnCheck);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(label0);
            Controls.Add(btnOpenLink);
            Controls.Add(txtURL);
            Controls.Add(btnStartParse);
            Controls.Add(webView);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Parser HH";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private Button btnStartParse;
        private TextBox txtURL;
        private Button btnOpenLink;
        private Label label0;
        private Label label1;
        private Label label2;
        private Button btnCheck;
    }
}
