using BooruSharp.Booru.Template;
using BooruSharp.Search.Post;

namespace yajal_data_collection {
    internal class BooruDownloader {
        protected Moebooru _booru;
        public BooruDownloader(Moebooru booru) {
            _booru = booru;
        }

        /// <summary>
        /// 포스트를 다운받습니디.
        /// </summary>
        /// <param name="path">폴더</param>
        /// <param name="safeCount">safe</param>
        /// <param name="safeCount">R-17</param>
        /// <param name="safeCount">R-19</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<DownloadResult[]> DownloadPostsAsync(string path, ImageDownloader downloader,
            int safeCount, int questionableCount, int explicitCount, int maxRefillCount, params string[] tags) {

            if (path == null) throw new ArgumentNullException(nameof(path));
            if (downloader == null) throw new ArgumentNullException(nameof(downloader));
            if (safeCount < 1) throw new ArgumentOutOfRangeException(nameof(safeCount), "포스트 개수는 0보다 커야합니다.");
            if (questionableCount < 1) throw new ArgumentOutOfRangeException(nameof(questionableCount), "포스트 개수는 0보다 커야합니다.");
            if (explicitCount < 1) throw new ArgumentOutOfRangeException(nameof(explicitCount), "포스트 개수는 0보다 커야합니다.");

            var count = safeCount + questionableCount + explicitCount;
            var a_posts = new Dictionary<Rating, SearchResult?[]> {
                { Rating.Safe,          new SearchResult?[safeCount] },
                { Rating.Questionable,  new SearchResult?[questionableCount] },
                { Rating.Explicit,      new SearchResult?[explicitCount] }
            };

            await FillPostsAsync(
                (int)(count * 5d), 
                a_posts,
                downloader, 
                path,
                maxRefillCount,
                tags
            );

            var results = new List<DownloadResult>();

            foreach (var posts in a_posts) {
                if (posts.Value == null) continue;
                var _posts = (from post
                             in posts.Value
                             where post.HasValue
                             select post.Value).ToArray();

                if (_posts.Length < 1) continue;

                var _count = _posts.Length;

                Console.Write($"Download {posts.Key} Images... ");

                using (var progress = new ProgressBar()) {
                    results.Add(await downloader.DownloadAsync(path, _posts,
                                                                     (_, i) => {
                                                                         progress.Report(i / (double)_count);
                                                                     }));
                }
                Console.WriteLine("Done.");
            }

            return results.ToArray();
        }

        /// <summary>
        /// 포스트 버퍼를 채웁니다.
        /// </summary>
        /// <param name="bufferCount">버퍼 크기</param>
        /// <param name="c_posts">포스트 버퍼</param>
        /// <param name="downloader">다운로더</param>
        /// <param name="tags">테그</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task FillPostsAsync(int bufferCount, IDictionary<Rating, SearchResult?[]> c_posts,
            ImageDownloader downloader, string path, int maxRefillCount, params string[] tags) {
            if (bufferCount < 1) throw new ArgumentOutOfRangeException(nameof(bufferCount), "버퍼 개수는 0보다 커야합니다.");
            if (c_posts == null) throw new ArgumentNullException(nameof(c_posts));
            if (c_posts.Count < 1) throw new ArgumentException(nameof(bufferCount), "포스트 버퍼는 1보다 커야합니다.");
            if (downloader == null) throw new ArgumentNullException(nameof(downloader));

            var r_posts = c_posts.ToDictionary(e => e.Key, e => e.Value);
            var fillCount = 0;

            while (r_posts.Count > 0 && fillCount++ <maxRefillCount) {
                var posts = await _booru.GetRandomPostsAsync(bufferCount, tags);

                for (int i = 0; i < posts.Length; i++) {
                    var post = posts[i];
                    if (!r_posts.TryGetValue(post.Rating, out var _posts))
                        return;

                    var index = Array.FindIndex(_posts, post => post == null);
                    if (index == -1) {
                        r_posts.Remove(post.Rating);
                        continue;
                    }

                    if (downloader.IsExists(post, path))
                        return;

                    _posts[index] = post;
                }
            }
        }
    }
}
