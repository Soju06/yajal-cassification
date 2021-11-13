namespace yajal_data_post_processing {
    internal static class Program {
        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();

            string path = "";

            //using (var dialog = new SelectDir()) {
            //    if (dialog.ShowDialog() != DialogResult.OK || dialog.Path == null) return;
            //    path = dialog.Path;
            //}

            Application.Run(new Processing(path));
        }
    }
}