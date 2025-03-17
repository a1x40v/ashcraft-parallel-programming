using System.Text;
using System.Threading.Tasks;

namespace ParallelLoops.Topics;

public static class Ex04ParallelForEachAsync
{
    private const string _folderToProcess = "/home/juracdev/img-files";

    public static async Task Run()
    {
        var cts = new CancellationTokenSource();

        List<string> filesToProcess = Directory.GetFiles(_folderToProcess).ToList();

        Task.Run(async () =>
        {
            await Task.Delay(500);
            cts.Cancel();
        });

        List<BitmapDummy> results = await FileProcessor.ConvertFilesToBitmapsAsync(filesToProcess, cts);

        StringBuilder resultText = new();

        foreach (var bmp in results)
        {
            resultText.AppendLine($"Bitmap dummy length: {bmp.Length}");
        }

        Console.WriteLine(resultText);
    }

    private class FileProcessor
    {
        public static async Task<List<BitmapDummy>> ConvertFilesToBitmapsAsync(
            List<string> files,
            CancellationTokenSource cts
        )
        {
            ParallelOptions po = new()
            {
                CancellationToken = cts.Token,
                MaxDegreeOfParallelism = Environment.ProcessorCount == 1 ? 1 : Environment.ProcessorCount - 1
            };

            var result = new List<BitmapDummy>();

            try
            {
                await Parallel.ForEachAsync(files, po,
                    async (file, _cts) =>
                    {
                        FileInfo fi = new(file);
                        string ext = fi.Extension.ToLower();

                        if (ext == ".jpg" || ext == ".jpeg")
                        {
                            result.Add(ConvertJpgToBitmap(file));
                            await Task.Delay(2000, _cts); // some delay for cancelling
                        }
                    }
                );
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                cts.Dispose();
            }
            return result;
        }

        private static BitmapDummy ConvertJpgToBitmap(string fileName)
        {
            BitmapDummy bmp = new() { Length = fileName.Length };

            return bmp;
        }
    }

    private class BitmapDummy
    {
        public int Length { get; set; } = 0;
    }
}

/*
    The new method is async, returns Task<List<Bitmap>>, accepts
CancellationTokenSource, and uses that when creating ParallelOptions
to pass to the Parallel.ForEachAsync method. Parallel.ForEachAsync
is awaited and its lambda expression is declared as async so we can await the new Task.
Delay that has been added to give us enough time to click the Cancel button before the
loop completes.

    Enclosing Parallel.ForEachAsync in a try/catch block that handles
OperationCanceledException enables the method to catch the cancellation. We’ll
show a message to the user after the cancellation is handled.

    The code is also setting the ProcessorCount option. If there is only one CPU core
available, we will set the value to 1; otherwise, we want to use no more than the number of
available cores minus one. The .NET runtime typically manages this value very well, so you
should only change this option if you find it improves your application’s performance.
*/