using System.Collections.Concurrent;
using System.Text;
using ParallelLoops.Models;

namespace ParallelLoops.Topics;

public static class Ex01ParallelForBasic
{
    private const string _folderToProcess = "/home/juracdev/text-files";

    public static void Run()
    {
        if (Directory.Exists(_folderToProcess))
        {
            string[] filesToProcess = Directory.GetFiles(_folderToProcess);
            FileData results = FileProcessor.GetInfoForFiles(filesToProcess);

            StringBuilder resultText = new();
            resultText.Append($"Total file count: {results.FileInfoList.Count}; ");
            resultText.AppendLine($"Total file size: {results.TotalSize} bytes");
            resultText.Append($"Last written file: {results.LastWrittenFileName} ");
            resultText.Append($"at{results.LastFileWriteTime}");


            Console.WriteLine(resultText);
        }
    }

    private class FileProcessor
    {
        public static FileData GetInfoForFiles(string[] files)
        {
            var results = new FileData();

            /*  incorrent result without concurrent collection */
            // var fileInfos = new List<FileInfo>();

            var fileInfos = new ConcurrentBag<FileInfo>();

            long totalFileSize = 0;
            DateTime lastWriteTime = DateTime.MinValue;
            string lastFileWritten = "";
            object dateLock = new();

            Parallel.For(0, files.Length,
                index =>
                {
                    FileInfo fi = new(files[index]);
                    long size = fi.Length;
                    DateTime lastWrite = fi.LastWriteTimeUtc;

                    lock (dateLock)
                    {
                        if (lastWriteTime < lastWrite)
                        {
                            lastWriteTime = lastWrite;
                            lastFileWritten = fi.Name;
                        }
                    }

                    Interlocked.Add(ref totalFileSize, size);
                    fileInfos.Add(fi);
                }
            );

            results.FileInfoList = fileInfos.ToList();
            results.TotalSize = totalFileSize;
            results.LastFileWriteTime = lastWriteTime;
            results.LastWrittenFileName = lastFileWritten;

            return results;
        }
    }
}

