using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelReading
{
    internal class SpacesCounterSecondVariant : SpacesCounterFirstVariant
    {
        // 1: Прочитать файлы параллельно и вычислить количество пробелов
        public override async Task<int[]> CountSpacesInFilesAsync(IEnumerable<string> files)
        {
            var stopwatch = Stopwatch.StartNew();
            /// Отличие от SpacesCounterFirstVariant здесь  -------------
            ///                                                         |
            ///                                                         v
            var spaces = await Task.WhenAll(files.Select(file => Task.Run(() => CountSpacesInFileAsync(file))));
            stopwatch.Stop();
            Console.WriteLine($"Время обработки файлов: {stopwatch.ElapsedMilliseconds} мс.");
            return spaces;
        }
    }
}
