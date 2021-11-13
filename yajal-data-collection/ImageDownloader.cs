using BooruSharp.Search.Post;

namespace yajal_data_collection {
    internal abstract class ImageDownloader {
        public HttpClient HttpClient { get; set; }

        public ImageDownloader() {
            HttpClient = new();
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.77 Safari/537.36");
        }

        public abstract bool IsExists(SearchResult result, string dir);

        public abstract Task DownloadAsync(SearchResult result, string dir);

        /// <summary>
        /// 포스트를 다운로드합니다.
        /// </summary>
        /// <param name="dir">경로</param>
        /// <param name="posts">포스트</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DownloadResult Download(string dir, IEnumerable<SearchResult> posts, int multiDownloadCount = 10,
            Action<SearchResult, int>? imageDownloadUpdate = null) {

            if (posts == null) throw new ArgumentNullException(nameof(posts));
            
            List<(int, string)> failedPosts = new();
            SearchResult e_post = default;
            int i = 0, f_i = 0;
            var tasks = new Task[multiDownloadCount];

            for (int l = 0; l < tasks.Length; l++)
                tasks[l] = Task.CompletedTask;

            foreach (var post in posts) {
                try {
                    if (i++ == 0) e_post = post;
                    var task = tasks[Task.WaitAny(tasks)] = DownloadAsync(post, dir);

                    task.ContinueWith(t => {
                        imageDownloadUpdate?.Invoke(post, ++f_i);
                    });
                } catch (Exception ex) {
                    failedPosts.Add(new(post.ID, ex.Message));
                }
            }

            Task.WaitAll(tasks);

            return new((from p in posts select p.ID).ToArray(), failedPosts.ToArray(), f_i, e_post.Rating);
        }
    }

    internal readonly struct DownloadResult {
        public int[] Posts { get; }
        public (int, string)[] FailedPosts { get; }
        public int Count { get; }
        public Rating Rating { get; }

        public DownloadResult(int[] posts, (int, string)[] failedPosts, int count, Rating rating) {
            Posts = posts;
            FailedPosts = failedPosts;
            Count = count;
            Rating = rating;
        }
    }
}
