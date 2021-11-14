namespace yajal_data_post_processing {
    partial class SelectDir {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this._dir_box = new System.Windows.Forms.TextBox();
            this._open_dir_btn = new System.Windows.Forms.Button();
            this._ok_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AllowDrop = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 32);
            this.label1.TabIndex = 0;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.label1.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            // 
            // _dir_box
            // 
            this._dir_box.AllowDrop = true;
            this._dir_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dir_box.Location = new System.Drawing.Point(12, 56);
            this._dir_box.Name = "_dir_box";
            this._dir_box.Size = new System.Drawing.Size(294, 23);
            this._dir_box.TabIndex = 1;
            this._dir_box.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this._dir_box.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            // 
            // _open_dir_btn
            // 
            this._open_dir_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._open_dir_btn.Location = new System.Drawing.Point(312, 56);
            this._open_dir_btn.Name = "_open_dir_btn";
            this._open_dir_btn.Size = new System.Drawing.Size(54, 23);
            this._open_dir_btn.TabIndex = 2;
            this._open_dir_btn.Text = "찾기";
            this._open_dir_btn.UseVisualStyleBackColor = true;
            this._open_dir_btn.Click += new System.EventHandler(this.OnOpenDir);
            this._open_dir_btn.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this._open_dir_btn.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            // 
            // _ok_btn
            // 
            this._ok_btn.AllowDrop = true;
            this._ok_btn.Location = new System.Drawing.Point(140, 94);
            this._ok_btn.Name = "_ok_btn";
            this._ok_btn.Size = new System.Drawing.Size(98, 34);
            this._ok_btn.TabIndex = 3;
            this._ok_btn.Text = "확인";
            this._ok_btn.UseVisualStyleBackColor = true;
            this._ok_btn.Click += new System.EventHandler(this.OnOkClick);
            this._ok_btn.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this._ok_btn.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            // 
            // SelectDir
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 140);
            this.Controls.Add(this._ok_btn);
            this.Controls.Add(this._open_dir_btn);
            this.Controls.Add(this._dir_box);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectDir";
            this.Text = "SelectDir";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox _dir_box;
        private Button _open_dir_btn;
        private Button _ok_btn;
    }
}