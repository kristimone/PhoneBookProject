using PhoneBook.Library;
using PhoneBook.Library.Models;
using PhoneBook.Library.Source.BinaryFile;
using System;

namespace PhoneBookProject
{
    class Program
    {
        static void Main(string[] args)
        {

            var binaryFileManager = new BinaryFileManager();

            if(!binaryFileManager.CreateFile())
            {
                Console.WriteLine("Gabim gjate krijimit te skedarit");
                return;
            }

            binaryFileManager.Add(new PhoneEntryModel
            {
                Id = 1,
                FirstName = "Kristi",
                LastName = "Mone",
                PhoneNumber = "+355682024896",
                EntryType = PhoneEntryType.WORK
            });


            binaryFileManager.Add(new PhoneEntryModel
            {
                Id = 2,
                FirstName = "Ermal",
                LastName = "Arapi",
                PhoneNumber = "+355695231205",
                EntryType = PhoneEntryType.CELLPHONE
            });

            binaryFileManager.Add(new PhoneEntryModel
            {
                Id = 3,
                FirstName = "Mario",
                LastName = "Coku",
                PhoneNumber = "+355692465823",
                EntryType = PhoneEntryType.CELLPHONE
            });

            binaryFileManager.Add(new PhoneEntryModel
            {
                Id = 4,
                FirstName = "Gerta",
                LastName = "Mone",
                PhoneNumber = "+35568602345698",
                EntryType = PhoneEntryType.WORK
            });

            binaryFileManager.Add(new PhoneEntryModel
            {
                Id = 5,
                FirstName = "Elektra",
                LastName = "Myrto",
                PhoneNumber = "+35542236894",
                EntryType = PhoneEntryType.HOME
            });


            binaryFileManager.Edit(new PhoneEntryModel
            {
                Id = 4,
                FirstName = "Endi",
                LastName = "Koci",
                PhoneNumber = "+35542354693",
                EntryType = PhoneEntryType.HOME
            });

            binaryFileManager.Delete(new PhoneEntryModel
            {
                Id = 2,
                FirstName = "Ermal",
                LastName = "Arapi",
                PhoneNumber = "+355695231205",
                EntryType = PhoneEntryType.CELLPHONE
            });


            foreach (var item in binaryFileManager.Iterate(true))
            {
                Console.WriteLine(item);
            }

            foreach (var item in binaryFileManager.Iterate(false))
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}
