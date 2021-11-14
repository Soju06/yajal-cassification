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
            this._processing_view = new yajal_data_post_processing.DragSelect();
            this._undo_btn = new System.Windows.Forms.Button();
            this._selected_area = new System.Windows.Forms.ListBox();
            this._preview_box = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this._selected_area_count = new yajal_data_post_processing.InfoLabel();
            this._file_selected_area = new System.Windows.Forms.ListBox();
            this._files = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this._preview_box)).BeginInit();
            this.SuspendLayout();
            // 
            // _processing_view
            // 
            this._processing_view.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._processing_view.DisabledText = "Disabled";
            this._processing_view.Image = null;
            this._processing_view.Location = new System.Drawing.Point(12, 12);
            this._processing_view.Name = "_processing_view";
            this._processing_view.Size = new System.Drawing.Size(611, 482);
            this._processing_view.TabIndex = 1;
            this._processing_view.Text = "dragSelect1";
            this._processing_view.DragSelected += new yajal_data_post_processing.DragSelectedEventHandler(this.OnDragSelected);
            // 
            // _undo_btn
            // 
            this._undo_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._undo_btn.Location = new System.Drawing.Point(629, 292);
            this._undo_btn.Name = "_undo_btn";
            this._undo_btn.Size = new System.Drawing.Size(73, 33);
            this._undo_btn.TabIndex = 3;
            this._undo_btn.Text = "실행 취소";
            this._undo_btn.UseVisualStyleBackColor = true;
            this._undo_btn.Click += new System.EventHandler(this.OnUndo);
            // 
            // _selected_area
            // 
            this._selected_area.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._selected_area.FormattingEnabled = true;
            this._selected_area.ItemHeight = 15;
            this._selected_area.Location = new System.Drawing.Point(629, 192);
            this._selected_area.Name = "_selected_area";
            this._selected_area.Size = new System.Drawing.Size(192, 94);
            this._selected_area.TabIndex = 4;
            this._selected_area.SelectedValueChanged += new System.EventHandler(this.OnSelectedChanged);
            this._selected_area.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnAreaKeyDown);
            // 
            // _preview_box
            // 
            this._preview_box.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._preview_box.Location = new System.Drawing.Point(629, 12);
            this._preview_box.Name = "_preview_box";
            this._preview_box.Size = new System.Drawing.Size(192, 174);
            this._preview_box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._preview_box.TabIndex = 5;
            this._preview_box.TabStop = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(775, 292);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(46, 33);
            this.button2.TabIndex = 6;
            this.button2.Text = "다음";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnNext);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(716, 292);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(53, 33);
            this.button3.TabIndex = 7;
            this.button3.Text = "이전";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnPrev);
            // 
            // _selected_area_count
            // 
            this._selected_area_count.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._selected_area_count.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._selected_area_count.Location = new System.Drawing.Point(12, 497);
            this._selected_area_count.Name = "_selected_area_count";
            this._selected_area_count.Size = new System.Drawing.Size(125, 18);
            this._selected_area_count.TabIndex = 8;
            this._selected_area_count.Template = "선택된 영역 : {0}";
            this._selected_area_count.Text = "0";
            this._selected_area_count.Value = "0";
            // 
            // _file_selected_area
            // 
            this._file_selected_area.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._file_selected_area.FormattingEnabled = true;
            this._file_selected_area.ItemHeight = 15;
            this._file_selected_area.Location = new System.Drawing.Point(629, 331);
            this._file_selected_area.Name = "_file_selected_area";
            this._file_selected_area.Size = new System.Drawing.Size(192, 64);
            this._file_selected_area.TabIndex = 9;
            // 
            // _files
            // 
            this._files.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._files.FormattingEnabled = true;
            this._files.ItemHeight = 15;
            this._files.Location = new System.Drawing.Point(629, 401);
            this._files.Name = "_files";
            this._files.Size = new System.Drawing.Size(192, 109);
            this._files.TabIndex = 10;
            this._files.SelectedIndexChanged += new System.EventHandler(this.OnSelectedFileChanged);
            // 
            // Processing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 524);
            this.Controls.Add(this._files);
            this.Controls.Add(this._file_selected_area);
            this.Controls.Add(this._selected_area_count);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this._preview_box);
            this.Controls.Add(this._selected_area);
            this.Controls.Add(this._undo_btn);
            this.Controls.Add(this._processing_view);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Processing";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Processing_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this._preview_box)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DragSelect _processing_view;
        private Button _undo_btn;
        private ListBox _selected_area;
        private PictureBox _preview_box;
        private Button button2;
        private Button button3;
        private InfoLabel _selected_area_count;
        private ListBox _file_selected_area;
        private ListBox _files;
    }
}