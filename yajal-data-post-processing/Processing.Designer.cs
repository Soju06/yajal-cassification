namespace yajal_data_post_processing {
    partial class Processing {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Processing));
            this.infoLabel1 = new yajal_data_post_processing.InfoLabel();
            this.dragSelect1 = new yajal_data_post_processing.DragSelect();
            this.SuspendLayout();
            // 
            // infoLabel1
            // 
            this.infoLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.infoLabel1.Location = new System.Drawing.Point(12, 423);
            this.infoLabel1.Name = "infoLabel1";
            this.infoLabel1.Size = new System.Drawing.Size(125, 18);
            this.infoLabel1.TabIndex = 0;
            this.infoLabel1.Template = "처리중인 작업: {0}";
            this.infoLabel1.Text = "0";
            this.infoLabel1.Value = "0";
            // 
            // dragSelect1
            // 
            this.dragSelect1.DisabledText = "Disabled";
            this.dragSelect1.Image = ((System.Drawing.Image)(resources.GetObject("dragSelect1.Image")));
            this.dragSelect1.Location = new System.Drawing.Point(161, 12);
            this.dragSelect1.Name = "dragSelect1";
            this.dragSelect1.Size = new System.Drawing.Size(611, 467);
            this.dragSelect1.TabIndex = 1;
            this.dragSelect1.Text = "dragSelect1";
            // 
            // Processing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 491);
            this.Controls.Add(this.dragSelect1);
            this.Controls.Add(this.infoLabel1);
            this.Name = "Processing";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private InfoLabel infoLabel1;
        private DragSelect dragSelect1;
    }
}