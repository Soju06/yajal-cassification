using System.Drawing.Imaging;

namespace yajal_data_post_processing {
    public partial class Processing : Form {
        string _path, _out_path, org_path;
        List<(Bitmap, RectangleF)> _selected_images = new();

        public Processing(string path, string out_path) {
            InitializeComponent();
            var files = Directory.GetFiles(_path = path);
            for (int i = 0; i < files.Length; i++)
                files[i] = Path.GetFileName(files[i]);
            _files.Items.AddRange(files);
            _out_path = out_path;
        }

        private void OnDragSelected(object sender, DragSelectedEventArgs e) {
            var rect = e.SourceRectangle;

            if (rect.Right < 10 || rect.Bottom < 10)
                _undo_btn.PerformClick();

            var img = _processing_view.GetClippingImage(rect);

            if (img != null)
                AddSelectedImage(img, rect);
        }

        void AddSelectedImage(Bitmap image, RectangleF rect) {
            _selected_images.Insert(0, new(image, rect));
            _preview_box.Image = image;
            _selected_area.Items.Insert(0, rect);
            UpdateCount();
        }

        void RemoveSelectedImage(int index) {
            if (index < 0 || _selected_images.Count <= index) return;
            var item = _selected_images[index];
            _selected_images.RemoveAt(index);
            _selected_area.Items.RemoveAt(index);
            item.Item1?.Dispose();
            UpdateCount();
        }

        void FocusItem(int index) {
            if (index < 0 || _selected_images.Count <= index) return;
            var item = _selected_images[index];
            _preview_box.Image = item.Item1;
            _selected_area.SelectedIndex = index;
        }

        void UpdateCount() {
            _selected_area_count.Value = _selected_area.Items.Count;
        }

        private void OnUndo(object sender, EventArgs e) {
            var rects = _processing_view.ClientSelected;
            if (rects.Count > 0) {
                rects.RemoveAt(rects.Count - 1);
                RemoveSelectedImage(0);
                FocusItem(0);
            }
            _processing_view.Refresh();
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.Z)
                _undo_btn.PerformClick();
        }

        private void OnAreaKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete && _selected_area.SelectedIndex != -1)
                RemoveSelectedImage(_selected_area.SelectedIndex);
        }

        private void OnSelectedChanged(object sender, EventArgs e) {
            if (_selected_area.SelectedIndex != -1)
                FocusItem(_selected_area.SelectedIndex);
        }

        private void OnNext(object sender, EventArgs e) {
            if (_files.Items.Count - 1 > _files.SelectedIndex)
                _files.SelectedIndex++;
        }

        private void OnPrev(object sender, EventArgs e) {
            if(_files.SelectedIndex > 0)
                _files.SelectedIndex--;
        }

        private void OnSelectedFileChanged(object sender, EventArgs e) {
            if (_files.SelectedIndex != -1 && _files.SelectedItem is string file) {
                SaveSelectedArea();
                _file_selected_area.Items.Clear();
                var file_key = file.Split('_');
                file = Path.Combine(_path, file);

                if (!File.Exists(file)) {
                    _files.Items.RemoveAt(_files.SelectedIndex);
                    return;
                }


                org_path = file;
                _preview_box.Image?.Dispose();
                _processing_view.Image?.Dispose();

                using (var fs = File.OpenRead(file))
                    _processing_view.Image = Bitmap.FromStream(fs);

                _file_selected_area.Items.AddRange(GetFileSelectedItems(Directory.GetFiles(_out_path), file_key[0]).ToArray());
            }
        }

        private void Processing_Load(object sender, EventArgs e) {

        }

        void SaveSelectedArea() {
            while (_selected_images.Count > 0) {
                var img = _selected_images[0];
                var rect = img.Item2;
                var file_path = Path.Combine(_out_path, Path.GetFileNameWithoutExtension(org_path) + $"_{rect.X}-{rect.Y}_{rect.Width}x{rect.Height}.jpg");

                img.Item1.Save(file_path, ImageFormat.Jpeg);
                RemoveSelectedImage(0);
            }
            _processing_view.ClientSelected.Clear();
        }

        IEnumerable<string> GetFileSelectedItems(string[] files, string key) {
            for (int i = 0; i < files.Length; i++) {
                var file = Path.GetFileName(files[i]);
                var sp = file.Split("_");
                if (sp.Length < 2 || sp[0] != key) continue;
                yield return file;
            }
        }
    }
}