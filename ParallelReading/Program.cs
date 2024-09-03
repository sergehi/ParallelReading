using System.Diagnostics;
using System.Linq;

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
        try
        {
            // 1: Прочитать 3 файла параллельно и вычислить количество пробелов
            int[] spaceCounts = await CountSpacesInFilesAsync(_files);
            for (int i = 0; i < _files.Length; i++)
                Console.WriteLine($"Количество пробелов в {_files[i]}: {spaceCounts[i]}");

            // 2: Прочитать все файлы в папке и вычислить количество пробелов
            Stopwatch stopwatch = Stopwatch.StartNew();
            string[] folder_files = Directory.GetFiles(_folderPath);
            spaceCounts = await CountSpacesInFilesAsync(folder_files);
            int totalSpaceCount = spaceCounts.Sum();
            stopwatch.Stop();
            Console.WriteLine($"Общее количество пробелов в папке: {totalSpaceCount}");
            Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в просессе выполения: {ex.Message}");
        }
        Console.Read();
    }

    // Количество пробелов в нескольких файлах (files)
    // Выполняется параллельно
    static async Task<int[]> CountSpacesInFilesAsync(IEnumerable<string> files)
    {
        return await Task.WhenAll(files.Select(file => CountSpacesInFileAsync(file)));
    }

    // Количество пробелов в одном файле (filePath)
    static async Task<int> CountSpacesInFileAsync(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        {
            string content = await reader.ReadToEndAsync();
            return content.Count(c => c == ' ');
        }
    }
}
