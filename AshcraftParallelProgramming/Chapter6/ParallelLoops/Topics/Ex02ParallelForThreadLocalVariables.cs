using System.Collections.Concurrent;
using System.Text;
using ParallelLoops.Models;

namespace ParallelLoops.Topics;

public static class Ex02ParallelForThreadLocalVariables
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

            Parallel.For<long>(0, files.Length, () => 0,
                (index, loop, subtotal) =>
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

                    subtotal += size;
                    fileInfos.Add(fi);
                    return subtotal;
                },
                (runningTotal) => Interlocked.Add(ref totalFileSize, runningTotal)
            );

            results.FileInfoList = fileInfos.ToList();
            results.TotalSize = totalFileSize;
            results.LastFileWriteTime = lastWriteTime;
            results.LastWrittenFileName = lastFileWritten;

            return results;
        }
    }
}

/*
    The Parallel.For construct has an overload that will allow our code to keep a running subtotal
of the total file size for each thread participating in the loop. What that means is that we will only need
to use Interlocked.Add when aggregating the subtotal from each thread to totalFileSize.
This is accomplished by providing a thread-local variable to the loop. The subtotal in the following
code is stored discretely for each thread. So, if the loop has 200 iterations, but only 5 threads participate
in the loop, Interlocked.Add will only be called 5 times instead of 200 times without losing
any thread safety

    To summarize the preceding changes, you will notice we are using the Parallel.For<long>
generic method to indicate that the subtotal thread-local variable should be long instead of
int (the default type). The size is added to subtotal in the first lambda expression without any
locking expression. We now have to return subtotal, so the other iterations have access to the
data. Finally, we have added a final parameter to For with a lambda expression that adds each thread’s
runningTotal to totalFileSize using Interlocked.Add.

    The output will be the same, but the performance will be improved, perhaps not noticeably
*/