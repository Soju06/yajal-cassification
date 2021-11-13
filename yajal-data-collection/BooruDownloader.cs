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
            int safeCount, int questionableCount, int explicitCount, int multiDownloadCount = 10, params string[] tags) {

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
                a_posts,
                downloader, 
                path,
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
                var _tag = posts.Key.ToString().ToLower();
                var p_path = Path.Combine(path, _tag);

                Console.WriteLine($"Download {posts.Key} ({Directory.CreateDirectory(p_path).FullName})");
                Console.Write($"Download Images... {_posts.Length} ");

                using (var progress = new ProgressBar()) {
                    results.Add(downloader.Download(p_path, _posts, multiDownloadCount,
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
        public async Task FillPostsAsync(IDictionary<Rating, SearchResult?[]> c_posts,
            ImageDownloader downloader, string path, params string[] tags) {
            if (c_posts == null) throw new ArgumentNullException(nameof(c_posts));
            if (downloader == null) throw new ArgumentNullException(nameof(downloader));

            foreach (var r in c_posts) {
                var _posts = r.Value;
                var posts = await _booru.GetRandomPostsAsync(_posts.Length, tagsArg: tags.Append($"rating:{r.Key.ToString().ToLower()}").ToArray());
                for (int i = 0; i < posts.Length; i++) {
                    var post = posts[i];

                    if (downloader.IsExists(post, path))
                        return;

                    _posts[i] = post;
                }
            }
        }
    }
}
