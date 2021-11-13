using BooruSharp.Search.Post;

namespace yajal_data_collection {
    internal class LargeImageDownloader : ImageDownloader {
        public override async Task DownloadAsync(SearchResult result, string dir) {
            var path = GetFullName(dir, result);
            using var stream = await HttpClient.GetStreamAsync(result.FileUrl);
            using var fs = File.OpenWrite(path);
            await stream.CopyToAsync(fs);
            await fs.FlushAsync();
        }

        public override bool IsExists(SearchResult result, string dir) =>
            File.Exists(GetFullName(dir, result));

        static string GetFullName(string dir, SearchResult result) => Path.Combine(dir, GetName(result));

        static string GetName(SearchResult r) => $"{r.ID}_{r.PreviewWidth}x{r.PreviewHeight}.jpg";
    }
}
