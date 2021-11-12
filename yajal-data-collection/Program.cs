using BooruSharp.Booru;
using BooruSharp.Booru.Template;
using yajal_data_collection;

Console.WriteLine("\n야짤 데이터 수집기 v.01");

var path = Path.Combine(Environment.CurrentDirectory, "images");

SET_IMAGES_PATH:

Console.WriteLine();
Console.Write($"데이터 폴더 (default: {path}): ");
var u_path = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(u_path)) path = u_path;

try {
    path = Directory.CreateDirectory(path).FullName;
} catch (Exception ex) {
    Console.WriteLine($"폴더를 생성하지 못했습니다.\n{ex}");
    goto SET_IMAGES_PATH;
}

var booru = (Moebooru)SelectCreateObject("Booru를 선택합니다.", "Booru", 0, new (Type, string)[] {
    new (typeof(Konachan), "Konachan"),
    new (typeof(Lolibooru), "Lolibooru"),
    new (typeof(BooruSharp.Booru.Gelbooru), "Gelbooru")
});

var booruDownloader = new BooruDownloader(booru);

var safeCount =         ReadInt("SAFE 타입 이미지 개수를 지정합니다.",           "개수 입력", 200,               e => e > 0);
var questionableCount = ReadInt("QUESTIONABLE 타입 이미지 개수를 지정합니다.",   "개수 입력", safeCount,         e => e > 0);
var explicitCount =     ReadInt("EXPLICIT 타입 이미지 개수를 지정합니다.",       "개수 입력", questionableCount, e => e > 0);

var downloader = (ImageDownloader)SelectCreateObject("Downloader를 선택합니다.", "Booru", 0, new (Type, string)[] {
    new (typeof(ThumbnailImageDownloader), "소형 이미지 다운로더"),
});

DOWNLOAD:
Console.WriteLine();
Console.WriteLine("다운로드를 시작합니다.");
Console.WriteLine($"총 개수: {safeCount + questionableCount + explicitCount}");
Console.WriteLine();

foreach (var result in await booruDownloader.DownloadPostsAsync(path, downloader, safeCount, questionableCount, explicitCount, 100)) {
    Console.WriteLine($"다운로드 완료됨 {result.Rating}");
    Console.WriteLine($"포스트 개수: {result.Count - result.FailedPosts.Length}/{result.Count}");
}

Console.Write("다운로드를 다시 하시겠습니까? (y/N)");
var _k = Console.ReadKey();
if (_k.Key == ConsoleKey.Y) goto DOWNLOAD;

goto EXIT;


EXIT:
Console.WriteLine();
Thread.Sleep(1000);
Environment.Exit(0);

object SelectCreateObject(string title, string message, int? defaultValue = null, params (Type, string)[] items) {
    var index = SelectItem(title, message, defaultValue, items: (from item in items select item.Item2).ToArray());

    Console.WriteLine("개체 생성중...");
    try {
        var obj = Activator.CreateInstance(items[index].Item1);
        if (obj == null) throw new NullReferenceException("개체가 null입니다.");
        return obj;
    } catch (Exception ex) {
        Console.WriteLine($"개체를 생성하지 못했습니다.\n{ex}\n");
        Thread.Sleep(1000);
        Environment.Exit(0);
        return null;
    }
}

int SelectItem(string title, string message, int? defaultValue = null, params string[] items) {
    string _title = "";
    int i = 1;
    foreach (var item in items)
        _title += $"{i++}. {item}\n";

    title = $"{title}\n{_title}";

    SHOW:
    var index = ReadInt(title, message, defaultValue) - 1;

    if (index > items.Length - 1) goto SHOW;
    else return index;
}

int ReadInt(string title, string message, int? defaultValue = null, Func<int, bool>? filter = null) {
    SHOW:
    Console.WriteLine();
    if (title != null)
        Console.WriteLine(title);
    Console.Write($"{message} {(defaultValue.HasValue ? $"(default: {defaultValue})" : "")}: ");
    var m = Console.ReadLine();
    if (int.TryParse(m, out var s) && filter?.Invoke(s) != false)
        return s;
    else if (defaultValue.HasValue)
        return defaultValue.Value;
    else goto SHOW;
}