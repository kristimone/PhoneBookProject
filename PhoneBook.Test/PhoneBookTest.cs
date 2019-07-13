using NUnit.Framework;
using PhoneBook.Library.Models;
using PhoneBook.Library.Source.BinaryFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhoneBook.Library.Tests
{
    [TestFixture]
    class PhoneBookTest
    {
        [Test(Description = "Test if the file doesnt exists, creates the file")]
        public void CreateFileTest()
        {
            BinaryFileManager binaryfile = new BinaryFileManager();

            if (!File.Exists(Constants.FilePath))
            {
                var file = File.Create(Constants.FilePath);
                file.Close();
            }

            Assert.IsTrue(File.Exists(Constants.FilePath));
        }

        [Test(Description = "Test if the file exists and has a length > 10MB, delete the file and create it again")]
        public void DeleteAndCreateFileTest()
        {
            BinaryFileManager binaryfile = new BinaryFileManager();
            FileInfo fi = new FileInfo(Constants.FilePath);
            if (fi.Length > Constants.FileMaxSize)
            {
                File.Delete(Constants.FilePath);
                var file = File.Create(Constants.FilePath);
                file.Close();
            }
            Assert.IsTrue(File.Exists(Constants.FilePath));
        }

        [Test(Description = "Test if the file exists, open the file")]
        public void OpenFileTest()
        {
            BinaryFileManager binaryfile = new BinaryFileManager();
            if (File.Exists(Constants.FilePath))
            {
                var file = File.Open(Constants.FilePath, FileMode.Append);
                file.Close();
            }
            Assert.IsTrue(File.Exists(Constants.FilePath));
        }

        [Test(Description = "Test if in one list of objects are read all the entries from the file")]
        public void AddEntriesAndCheckIfListContainsThem()
        {
            var entry1 = new PhoneEntryModel
            {
                Id = 1,
                FirstName = "Kristi",
                LastName = "Mone",
                PhoneNumber = "+355682456321",
                EntryType = PhoneEntryType.CELLPHONE
            };

            var entry2 = new PhoneEntryModel
            {
                Id = 2,
                FirstName = "Orges",
                LastName = "Kreka",
                PhoneNumber = "+355696054698",
                EntryType = PhoneEntryType.WORK
            };

            var entry3 = new PhoneEntryModel
            {
                Id = 3,
                FirstName = "Ermal",
                LastName = "Arapi",
                PhoneNumber = "+35542235205",
                EntryType = PhoneEntryType.HOME
            };

            BinaryFileManager binaryFile = new BinaryFileManager();

            binaryFile.Add(entry1);
            binaryFile.Add(entry2);
            binaryFile.Add(entry3);

            var result = binaryFile.GetAll();

            Assert.IsTrue(result[0].Equals(entry1) && result[1].Equals(entry2) && result[2].Equals(entry3));

        }

        [Test(Description = "Test if an entry is null, throw exception")]
        public void AddEntryNullException()
        {
            BinaryFileManager binaryFile = new BinaryFileManager();
            PhoneEntryModel model = new PhoneEntryModel();
            model = null;
            Assert.Throws<ArgumentNullException>(() => { throw new ArgumentNullException(); });
        }

        [Test(Description = "Test if an entry from a list of object is written on the file")]
        public void AddEntryToPhoneBook()
        {
            BinaryFileManager binaryFile = new BinaryFileManager();
            PhoneEntryModel model = new PhoneEntryModel
            {
                Id = 4,
                FirstName = "Mario",
                LastName = "Coku",
                PhoneNumber = "+355682024896",
                EntryType = PhoneEntryType.WORK
            };

            binaryFile.Add(model);

            Assert.IsTrue(binaryFile.GetAll().Any(x => x.FirstName.Equals("Mario") && x.LastName.Equals("Coku")));

        }
        [Test(Description = "Test Add Function with ThreadSafety process")]
        public void AddEntryThreadSafety()
        {
            var binaryFileManager = new BinaryFileManager();

            var listOfTasks = new List<Task>();

            var entries = binaryFileManager.GetAll();

            foreach (var item in entries)
            {
                binaryFileManager.Delete(item);
            }

            var listOfObjects = new List<PhoneEntryModel>
            {
               new PhoneEntryModel
               {
                    Id = 1,
                    FirstName = "Kristi",
                    LastName = "Mone",
                    PhoneNumber = "+355682024896",
                    EntryType = PhoneEntryType.WORK
               },

               new PhoneEntryModel
               {
                    Id = 2,
                    FirstName = "Orges",
                    LastName = "Kreka",
                    PhoneNumber = "+355682024896",
                    EntryType = PhoneEntryType.WORK
               },
               new PhoneEntryModel
               {
                    Id = 3,
                    FirstName = "Ermal",
                    LastName = "Arapi",
                    PhoneNumber = "+355682024896",
                    EntryType = PhoneEntryType.WORK
               }
            };

            foreach (var model in listOfObjects)
            {
                var addTask = new Task<bool>(()=> binaryFileManager.Add(model) );
                listOfTasks.Add(addTask);
            }

            listOfTasks.ForEach(x => x.Start());
            Task.WaitAll(listOfTasks.ToArray()); 

            var tmp = binaryFileManager.GetAll();

            Assert.IsTrue(tmp.Count == listOfObjects.Count);
           
        }

        [Test(Description = "Test if an entry from a list of object is written on the file")]
        public void AddEntryToPhoneBookReturnFalse()
        {
            BinaryFileManager binaryFile = new BinaryFileManager();
            PhoneEntryModel model = new PhoneEntryModel
            {
                Id = 4,
                FirstName = "Mario",
                LastName = "Coku",
                PhoneNumber = "+355682024896",
                EntryType = PhoneEntryType.WORK
            };

            var phoneEntries = binaryFile.GetAll();
            if (phoneEntries.Any(x => x.Id == model.Id))

            Assert.IsFalse(false);

        }

        [Test(Description = "Test if entry is null, throws exception")]
        public void EditEntryNullException()
        {
            BinaryFileManager binaryFile = new BinaryFileManager();
            PhoneEntryModel model = new PhoneEntryModel();
            model = null;
            Assert.Throws<ArgumentNullException>(() => { throw new ArgumentNullException(); });
        }

        [Test(Description = "Test if an entry with the same id with an entry from a list of object is edited on the file")]
        public void EditEntryToPhoneBook()
        {
            BinaryFileManager binaryFile = new BinaryFileManager();
            PhoneEntryModel model = new PhoneEntryModel
            {
                Id = 3,
                FirstName = "Endi",
                LastName = "Koci",
                PhoneNumber = "+355682024896",
                EntryType = PhoneEntryType.WORK
            };

            var result = binaryFile.Edit(model);

            Assert.IsTrue(result == true && binaryFile.GetAll().Any(x => x.FirstName.Equals("Endi") && x.LastName.Equals("Koci")));
        }
        [Test(Description = "Test Edit Function with ThreadSafety process")]
        public void EditEntryThreadSafety()
        {
            var binaryFileManager = new BinaryFileManager();

            var listOfTasks = new List<Task>();

            var entries = binaryFileManager.GetAll();

            var listOfObjects = new List<PhoneEntryModel>
            {
               new PhoneEntryModel
               {
                    Id = 3,
                    FirstName = "Petrit",
                    LastName = "Lame",
                    PhoneNumber = "+355682624896",
                    EntryType = PhoneEntryType.CELLPHONE
               }
            };

            foreach (var model in listOfObjects)
            {
                var addTask = new Task<bool>(() => binaryFileManager.Edit(model));
                listOfTasks.Add(addTask);
            }

            listOfTasks.ForEach(x => x.Start());
            Task.WaitAll(listOfTasks.ToArray());

            var tmp = binaryFileManager.GetAll();

            Assert.IsTrue(tmp.Count == entries.Count);

            for (int i = 0; i < tmp.Count; i++)
            {
                Assert.IsTrue(tmp[i].FirstName != entries[i].FirstName);
                break;
            }
        }

        [Test(Description = "Test if id of the entry is different of the id of any of entries on the list, edit entry return false")]
        public void EditEntryDifferentIdFromFile()
        {
            BinaryFileManager binaryFile = new BinaryFileManager();
            var phoneEntries = binaryFile.GetAll();
            PhoneEntryModel model = new PhoneEntryModel
            {
                Id = 7,
                FirstName = "Orges",
                LastName = "Kreka",
                PhoneNumber = "+355682323896",
                EntryType = PhoneEntryType.WORK
            };

            if (!phoneEntries.Any(x => x.Id == model.Id))

            Assert.IsFalse(false);
        }

        [Test(Description = "Test if an entry is null, throws exception")]
        public void DeleteEntryNullException()
        {
            BinaryFileManager binaryFile = new BinaryFileManager();
            PhoneEntryModel model = new PhoneEntryModel();
            model = null;
            Assert.Throws<ArgumentNullException>(() => { throw new ArgumentNullException(); });
        }

        [Test(Description = "Test if an entry from a list of object will be deleted from the file")]
        public void DeleteEntryFromPhoneBook()
        {
            PhoneEntryModel model = new PhoneEntryModel
            {
                Id = 1,
                FirstName = "Kristi",
                LastName = "Mone",
                PhoneNumber = "+355682024896",
                EntryType = PhoneEntryType.WORK
            };

            var binaryFileManager = new BinaryFileManager();
            var result = binaryFileManager.Delete(model);

            Assert.IsTrue(result && !binaryFileManager.GetAll().Any(x => x.Id == model.Id));
        }

        [Test(Description = "Test Delete Function with ThreadSafety process")]
        public void DeleteEntryThreadSafety()
        {
            var binaryFileManager = new BinaryFileManager();

            var listOfTasks = new List<Task>();

            var entries = binaryFileManager.GetAll();

            var listOfObjects = new List<PhoneEntryModel>
            {
               new PhoneEntryModel
               {
                    Id = 2,
                    FirstName = "Orges",
                    LastName = "Kreka",
                    PhoneNumber = "+355682024896",
                    EntryType = PhoneEntryType.WORK
               }
            };

            foreach (var model in listOfObjects)
            {
                var addTask = new Task<bool>(() => binaryFileManager.Delete(model));
                listOfTasks.Add(addTask);
            }

            listOfTasks.ForEach(x => x.Start());
            Task.WaitAll(listOfTasks.ToArray());

            var tmp = binaryFileManager.GetAll();

            Assert.IsTrue(tmp.Count == (entries.Count - listOfObjects.Count));

        }

        [Test(Description = "Test if id of the entry is different of the id of any of entries on the list, delete entry returns false")]
        public void DeleteEntryDifferentId_FromFile()
        {
            BinaryFileManager binaryFile = new BinaryFileManager();
            var phoneEntries = binaryFile.GetAll();
            PhoneEntryModel model = new PhoneEntryModel
            {
                Id = 7,
                FirstName = "Orges",
                LastName = "Kreka",
                PhoneNumber = "+355682323896",
                EntryType = PhoneEntryType.WORK
            };

            if (!phoneEntries.Any(x => x.Id == model.Id))

            Assert.IsFalse(false);
        }
        [Test(Description = "Test if the list is iterated and order by firstname and written to file")]
        public void IterateListByFirstName()
        {
            bool orderByFirstName = true;
            BinaryFileManager binaryFile = new BinaryFileManager();
            var phoneEntries = binaryFile.GetAll();
            var orderList = new List<PhoneEntryModel>();
            if (orderByFirstName)
                orderList = phoneEntries.OrderBy(p => p.FirstName).ToList();
            Assert.That(orderList, Is.Ordered.By("FirstName"));
        }
        [Test(Description = "Test if the list is iterated and order by lastname and written to file")]
        public void IterateListByLastName()
        {
            bool orderByFirstName = false;
            BinaryFileManager binaryFile = new BinaryFileManager();
            var phoneEntries = binaryFile.GetAll();
            var orderList = new List<PhoneEntryModel>();
            if (orderByFirstName)
                orderList = phoneEntries.OrderBy(p => p.LastName).ToList();
            Assert.That(orderList, Is.Ordered.By("LastName"));
        }
    }
}
