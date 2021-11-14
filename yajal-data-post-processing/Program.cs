namespace yajal_data_post_processing {
    internal static class Program {
        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();

            string path = "";
            string out_path = "";

            using (var dialog = new SelectDir("후가공할 폴더를 선택합니다.")) {
                if (dialog.ShowDialog() != DialogResult.OK || dialog.Path == null) return;
                path = dialog.Path;
            }

            using (var dialog = new SelectDir("출력 폴더를 선택합니다.")) {
                if (dialog.ShowDialog() != DialogResult.OK || dialog.Path == null) return;
                out_path = dialog.Path;
            }

            Application.Run(new Processing(path, out_path));
        }
    }
}