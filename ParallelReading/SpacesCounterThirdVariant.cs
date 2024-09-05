using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelReading
{
    internal class SpacesCounterThirdVariant : SpacesCounterFirstVariant
    {

        // 1: Прочитать файлы параллельно и вычислить количество пробелов
        public override async Task<int[]> CountSpacesInFilesAsync(IEnumerable<string> files)
        {
            var spaceCounts = new List<int>();
            var stopwatch = Stopwatch.StartNew();

            await Parallel.ForEachAsync(files, async (file, cancellationToken) =>
            {
                int count = await CountSpacesInFileAsync(file);
                lock (spaceCounts) // lock для безопасного добавления в список
                {
                    spaceCounts.Add(count);
                }
            });
            stopwatch.Stop();
            Console.WriteLine($"Время обработки файлов: {stopwatch.ElapsedMilliseconds} мс.");
            return [.. spaceCounts];
        }
    }
}
