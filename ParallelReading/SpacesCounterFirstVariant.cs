using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelReading
{
    internal class SpacesCounterFirstVariant
    {
        public void IntroduceYourself() => Console.WriteLine($"Выбранный метод расчета: {this}");

        // 1: Прочитать файлы параллельно и вычислить количество пробелов
        public virtual async Task<int[]> CountSpacesInFilesAsync(IEnumerable<string> files)
        {
            var stopwatch = Stopwatch.StartNew();
            var spaces = await Task.WhenAll(files.Select(file => CountSpacesInFileAsync(file)));
            stopwatch.Stop();
            Console.WriteLine($"Время обработки файлов: {stopwatch.ElapsedMilliseconds} мс.");
            return spaces;
        }

        // 2: Прочитать все файлы в папке и вычислить количество пробелов
        public async Task<int> CountSpacesInFolderFilesAsync(string folderName)
        {
            Console.WriteLine("");
            Console.WriteLine($"Считаем общее количество пробелов в файлах в папке");
            var folderFiles = Directory.GetFiles(folderName);
            var stopwatch = Stopwatch.StartNew();
            int[] spaceCounts = await CountSpacesInFilesAsync(folderFiles);
            var totalSpaceCount = spaceCounts.Sum();
            stopwatch.Stop();
            Console.WriteLine($"Общее количество пробелов в папке: {totalSpaceCount}");
            Console.WriteLine($"Время с суммированием результатов: {stopwatch.ElapsedMilliseconds} мс.");
            return totalSpaceCount;
        }

        // Количество пробелов в одном файле (filePath)
        static public async Task<int> CountSpacesInFileAsync(string filePath)
        {
            Console.WriteLine($"++Начинаем читать файл {filePath}, Поток: {Thread.CurrentThread.ManagedThreadId}");
            var watch = Stopwatch.StartNew();
            using (var reader = new StreamReader(filePath))
            {
                await Task.Delay(500); // Задержка в 500 мс для минимизации влияния накладных расходов на доказательство параллельности вычислений
                string content = await reader.ReadToEndAsync();
                int res = content.Count(c => c == ' ');
                watch.Stop();
                Console.WriteLine($"--Закончили читать файл {filePath}, Результат: {res} Поток: {Thread.CurrentThread.ManagedThreadId}, время чтения: {watch.ElapsedMilliseconds} мс.");
                return res;
            }
        }

    }
}
