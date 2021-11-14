namespace yajal_data_post_processing {
    internal static class Program {
        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();

            string path = "";
            string out_path = "";

            using (var dialog = new SelectDir("�İ����� ������ �����մϴ�.")) {
                if (dialog.ShowDialog() != DialogResult.OK || dialog.Path == null) return;
                path = dialog.Path;
            }

            using (var dialog = new SelectDir("��� ������ �����մϴ�.")) {
                if (dialog.ShowDialog() != DialogResult.OK || dialog.Path == null) return;
                out_path = dialog.Path;
            }

            Application.Run(new Processing(path, out_path));
        }
    }
}