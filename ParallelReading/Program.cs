using System.Diagnostics;
using System.Linq;
using ParallelReading;


// Что изменено:
// 1. Реализовано 3 варианта расчета(для измения типа расчета раскоментить строки 43 или 45 или 47):
//        а. SpacesCounterFirstVariant  - первоначальный вариант и я утверждаю, что файлы обрабатываются параллельно
//        б. SpacesCounterSecondVariant - модификация первого варианта для того что-бы каждый вызов CountSpacesInFileAsync производился из отдельного потока.
//        в. SpacesCounterThirdVariant  - Реализация через Parallel.ForEachAsync, первые два используют Task.WhenAll
// 2. Для проверки добавлен вывод в консоль времени выполнения каждой операции и номер потока.
// 3. Для того что-бы уменьшить влияние "накладных расходов" на результаты (у меня до 40мс. при времени обработки одного файла 1-4мс)  добавлен вызов await Task.Delay(500) в методе CountSpacesInFileAsync;

// Результаты:
//  ВСЕ три способа производят обработку параллельно. Это видно из вывода в консоли:
//        1. Последовательность вывода нарушена - сначала последовательно выводятся строки ""++Начинаем читать файл...", а затем, также последовательно "--Закончили читать....",
//        2. Время обработки одного файла составляет ~500-520мс(с учетом искусственной задержки), а вся операция не на много больше.
//  В первом варианте видно, что вызывающий поток метода CountSpacesInFileAsync всегда один(как правило основной поток или поток возвращаемый из предыдущего этапа расчета), но это происходит потому, что вывод в консоль производится до вызова await. Возвращаемый поток всегда разный.
// 
//  Мое мнение:
//     В задании, про создание явной многопоточности ни чего не сказано, есть фраза "Студент сделает запуск тасок в параллель.."
//     Это было реализовано вызовом Task<int> Task.WhenAll(IEnumerable<Task<int>> tasks)
//     В первом варианте, поскольку у Task не был вызван метод Run, новый поток не создается до вызова await. 
//     Во втором варианте я явно запускаю каждый таск.

internal class Program
{
    private static string _folderPath = @"..\..\..\Samples"; // Укажите путь к вашей папке
    private static string[] _files =
    {
          @"..\..\..\Samples\SampleFile1.txt"
        , @"..\..\..\Samples\SampleFile2.txt"
        , @"..\..\..\Samples\SampleFile3.txt"
        , @"..\..\..\Samples\SampleFile4.txt"
        , @"..\..\..\Samples\SampleFile5.txt"
    };

    private static async Task Main(string[] args)
    {
        try
        {
            // Первый вариант - параллельное вычисление, но вызов CountSpacesInFileAsync производится из одного потока
            var variant = new SpacesCounterFirstVariant();
            // Второй вариант - параллельное вычисление. Каждый вызов  CountSpacesInFileAsync из отдельного потока
            //var variant = new SpacesCounterSecondVariant();
            // Третий вариант - параллельное вычисление. Реализация через Parallel.ForEachAsync
            //var variant = new SpacesCounterThirdVariant();

            variant.IntroduceYourself();
            // 1: Прочитать 3 (для тестирования сделал 5) файла параллельно и вычислить количество пробелов
            await variant.CountSpacesInFilesAsync(_files);
            // 2: Прочитать все файлы в папке и вычислить количество пробелов
            await variant.CountSpacesInFolderFilesAsync(_folderPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в процессе выполения: {ex.Message}");
        }
        Console.Read();
    }

}
