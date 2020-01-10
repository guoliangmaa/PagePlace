namespace Page_replacement_algorithm
{
    partial class resultForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(resultForm));
            this.buttonOPT = new System.Windows.Forms.Button();
            this.buttonFIFO = new System.Windows.Forms.Button();
            this.buttonLRU = new System.Windows.Forms.Button();
            this.listBoxOpt = new System.Windows.Forms.ListBox();
            this.listBoxFifo = new System.Windows.Forms.ListBox();
            this.listBoxLru = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonOPT
            // 
            this.buttonOPT.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonOPT.BackgroundImage")));
            this.buttonOPT.Font = new System.Drawing.Font("华文仿宋", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOPT.Location = new System.Drawing.Point(77, 125);
            this.buttonOPT.Name = "buttonOPT";
            this.buttonOPT.Size = new System.Drawing.Size(94, 170);
            this.buttonOPT.TabIndex = 1;
            this.buttonOPT.Text = "OPT\r\n模式\r\n游戏\r\n结果";
            this.buttonOPT.UseVisualStyleBackColor = true;
            // 
            // buttonFIFO
            // 
            this.buttonFIFO.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonFIFO.BackgroundImage")));
            this.buttonFIFO.Font = new System.Drawing.Font("华文仿宋", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonFIFO.Location = new System.Drawing.Point(77, 431);
            this.buttonFIFO.Name = "buttonFIFO";
            this.buttonFIFO.Size = new System.Drawing.Size(94, 169);
            this.buttonFIFO.TabIndex = 2;
            this.buttonFIFO.Text = "FIFO\r\n模式\r\n游戏\r\n结果";
            this.buttonFIFO.UseVisualStyleBackColor = true;
            // 
            // buttonLRU
            // 
            this.buttonLRU.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonLRU.BackgroundImage")));
            this.buttonLRU.Font = new System.Drawing.Font("华文仿宋", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonLRU.Location = new System.Drawing.Point(68, 746);
            this.buttonLRU.Name = "buttonLRU";
            this.buttonLRU.Size = new System.Drawing.Size(94, 157);
            this.buttonLRU.TabIndex = 3;
            this.buttonLRU.Text = "LRU\r\n模式\r\n游戏\r\n结果";
            this.buttonLRU.UseVisualStyleBackColor = true;
            // 
            // listBoxOpt
            // 
            this.listBoxOpt.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxOpt.FormattingEnabled = true;
            this.listBoxOpt.ItemHeight = 27;
            this.listBoxOpt.Location = new System.Drawing.Point(256, 23);
            this.listBoxOpt.Name = "listBoxOpt";
            this.listBoxOpt.Size = new System.Drawing.Size(1201, 31);
            this.listBoxOpt.TabIndex = 8;
            // 
            // listBoxFifo
            // 
            this.listBoxFifo.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxFifo.FormattingEnabled = true;
            this.listBoxFifo.ItemHeight = 27;
            this.listBoxFifo.Location = new System.Drawing.Point(256, 351);
            this.listBoxFifo.Name = "listBoxFifo";
            this.listBoxFifo.Size = new System.Drawing.Size(1201, 31);
            this.listBoxFifo.TabIndex = 9;
            // 
            // listBoxLru
            // 
            this.listBoxLru.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxLru.FormattingEnabled = true;
            this.listBoxLru.ItemHeight = 27;
            this.listBoxLru.Location = new System.Drawing.Point(256, 679);
            this.listBoxLru.Name = "listBoxLru";
            this.listBoxLru.Size = new System.Drawing.Size(1201, 31);
            this.listBoxLru.TabIndex = 10;
            // 
            // resultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1924, 1021);
            this.Controls.Add(this.listBoxLru);
            this.Controls.Add(this.listBoxFifo);
            this.Controls.Add(this.listBoxOpt);
            this.Controls.Add(this.buttonLRU);
            this.Controls.Add(this.buttonFIFO);
            this.Controls.Add(this.buttonOPT);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "resultForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "游戏结果";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.resultForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonOPT;
        private System.Windows.Forms.Button buttonFIFO;
        private System.Windows.Forms.Button buttonLRU;
        private System.Windows.Forms.ListBox listBoxOpt;
        private System.Windows.Forms.ListBox listBoxFifo;
        private System.Windows.Forms.ListBox listBoxLru;
    }
}