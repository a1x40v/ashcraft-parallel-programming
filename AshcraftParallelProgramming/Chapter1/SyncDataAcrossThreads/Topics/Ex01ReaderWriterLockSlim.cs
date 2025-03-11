using SyncDataAcrossThreads.Models;

namespace SyncDataAcrossThreads.Topics;

public static class Ex01ReaderWriterLockSlim
{
    public static void Run()
    {
        var contactListManager = new ContactListManager(new List<Contact>
        {
            new Contact { Id = 0, PhoneNumber = "000-000-0" }
        });

        Thread[] writeThreads = new Thread[10];

        for (int i = 0; i < writeThreads.Length; i++)
        {
            int threadNumber = i;

            Thread writeThread = new Thread(() =>
            {
                while (true)
                {
                    contactListManager.AddContact(new Contact { Id = 1, PhoneNumber = $"000-000-0" });
                    Console.WriteLine($"Write Thread #{threadNumber}, Write contact");

                    Thread.Sleep(100);
                }
            });

            writeThreads[i] = writeThread;
        }

        Thread[] readThreads = new Thread[10];

        for (int i = 0; i < readThreads.Length; i++)
        {
            int threadNumber = i;

            Thread readThread = new Thread(() =>
            {
                while (true)
                {
                    var contact = contactListManager.GetContactByPhoneNumber("000-000-0");
                    Console.WriteLine($"Read Thread #{threadNumber}, Read contact: {contact?.PhoneNumber}");

                    Thread.Sleep(100);
                }
            });

            readThreads[i] = readThread;
        }


        foreach (var thread in writeThreads.Concat(readThreads))
        {
            thread.Start();
        }
    }
}

