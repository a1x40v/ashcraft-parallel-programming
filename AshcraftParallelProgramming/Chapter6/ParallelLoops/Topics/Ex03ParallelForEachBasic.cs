using System.Text;

namespace ParallelLoops.Topics;

public static class Ex03ParallelForEachBasic
{
    private const string _folderToProcess = "/home/juracdev/img-files";

    public static void Run()
    {
        List<string> filesToProcess = Directory.GetFiles(_folderToProcess).ToList();

        List<BitmapDummy> results = FileProcessor.ConvertFilesToBitmaps(filesToProcess);

        StringBuilder resultText = new();

        foreach (var bmp in results)
        {
            resultText.AppendLine($"Bitmap dummy length: {bmp.Length}");
        }

        Console.WriteLine(resultText);
    }

    private class FileProcessor
    {
        public static List<BitmapDummy> ConvertFilesToBitmaps(List<string> files)
        {
            var result = new List<BitmapDummy>();

            Parallel.ForEach(files, file =>
            {
                FileInfo fi = new(file);
                string ext = fi.Extension.ToLower();

                if (ext == ".jpg" || ext == ".jpeg")
                {
                    result.Add(ConvertJpgToBitmap(file));
                }
            });

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
