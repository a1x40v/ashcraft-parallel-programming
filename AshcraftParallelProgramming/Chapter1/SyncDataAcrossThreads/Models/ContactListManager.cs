namespace SyncDataAcrossThreads.Models;

public class ContactListManager
{
    private readonly List<Contact> contacts;
    private readonly ReaderWriterLockSlim contactLock = new ReaderWriterLockSlim();

    public ContactListManager(List<Contact> initialContacts)
    {
        contacts = initialContacts;
    }

    public void AddContact(Contact newContact)
    {
        try
        {
            contactLock.EnterWriteLock();
            contacts.Add(newContact);
        }
        finally
        {
            contactLock.ExitWriteLock();
        }

        /* unsafe approach */
        // contacts.Add(newContact);
    }

    public Contact GetContactByPhoneNumber(string phoneNumber)
    {
        try
        {
            contactLock.EnterReadLock();
            return contacts.FirstOrDefault(x => x.PhoneNumber == phoneNumber)!;
        }
        finally
        {
            contactLock.ExitReadLock();
        }

        /* unsafe approach */
        // return contacts.FirstOrDefault(x => x.PhoneNumber == phoneNumber)!;
    }
}
