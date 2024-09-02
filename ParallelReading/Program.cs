using System.Diagnostics;
internal class Program
{
    private static string _folderPath = @"..\..\..\Samples"; // Укажите путь к вашей папке
    private static string[] _files =
    {
          @"..\..\..\Samples\SampleFile1.txt"
        , @"..\..\..\Samples\SampleFile2.txt"
        , @"..\..\..\Samples\SampleFile3.txt"
    };

    private static async Task Main(string[] args)
    {
        // 1: Прочитать 3 файла параллельно и вычислить количество пробелов
        var spaceCounts = await Task.WhenAll(_files.Select(file => CountSpacesInFileAsync(file)));
        // var spaceCounts = Client.GetClients().AsParallel().Select(async file => await CountSpacesInFileAsync(file));
        for (int i = 0; i < _files.Length; i++)
        {
            Console.WriteLine($"Количество пробелов в {_files[i]}: {spaceCounts[i]}");
        }

        // 2: Прочитать все файлы в папке и вычислить количество пробелов
        var stopwatch = Stopwatch.StartNew();
        var totalSpaceCount = await CountSpacesInFolderAsync(_folderPath);
        stopwatch.Stop();

        Console.WriteLine($"Общее количество пробелов в папке: {totalSpaceCount}");
        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
        Console.Read();
    }
    static async Task<int> CountSpacesInFileAsync(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        {
            string content = await reader.ReadToEndAsync();
            return content.Count(c => c == ' ');
        }
    }

    static async Task<int> CountSpacesInFolderAsync(string folderPath)
    {
        var files = Directory.GetFiles(folderPath);
        var spaceCounts = await Task.WhenAll(files.Select(file => CountSpacesInFileAsync(file)));
         // var spaceCounts = Client.GetClients().AsParallel().Select(async file => await CountSpacesInFileAsync(file));
       return spaceCounts.Sum();
    }
}
