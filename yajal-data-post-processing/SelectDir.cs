using Microsoft.WindowsAPICodePack.Dialogs;

namespace yajal_data_post_processing {
    public partial class SelectDir : Form {
        public SelectDir() {
            InitializeComponent();
        }

        private void OnOkClick(object sender, EventArgs e) {
            if (CheckExist(_dir_box.Text)) {
                Path = _dir_box.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        public string? Path { get; private set; }

        private void OnOpenDir(object sender, EventArgs e) {
            using (var dialog = new CommonOpenFileDialog() { 
                IsFolderPicker = true,
                InitialDirectory = Application.StartupPath
            }) {
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;

                var path = dialog.FileName;

                if (CheckExist(path))
                    _dir_box.Text = path;
            }
        }

        static bool CheckExist(string path) {
            if (!Directory.Exists(path)) {
                MessageBox.Show("폴더가 존재하지 않습니다.", "ㅇㅇ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
             return true;
        }
    }
}
