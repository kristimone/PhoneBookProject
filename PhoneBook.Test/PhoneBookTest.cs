using Moq;
using NUnit.Framework;
using PhoneBook.Library.Models;
using PhoneBook.Library.Source.BinaryFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            BinaryFileManager binaryfile = new BinaryFileManager(null);
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
            BinaryFileManager binaryfile = new BinaryFileManager(null);
            if (File.Exists(Constants.FilePath))
            {
                var file = File.Open(Constants.FilePath, FileMode.Append);
                file.Close();
            }
            Assert.IsTrue(File.Exists(Constants.FilePath));
        }

        [Test(Description = "Test if in one list of objects are read all the entries from the file")]
        public void GetAllFile()
        {
            var tmp = new List<PhoneEntryModel>();
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
            mockfile.Setup(m => m.ReadFromBinaryFile<List<PhoneEntryModel>>(Constants.FilePath)).Returns(tmp);
        }

        [Test(Description = "Test if an entry from a list of object is written on the file")]
        public void AddEntry()
        {
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
            PhoneEntryModel model = new PhoneEntryModel
            {
                Id = 1,
                FirstName = "Kristi",
                LastName = "Mone",
                PhoneNumber = "+355682024896",
                EntryType = PhoneEntryType.WORK
            };
            var phoneEntries = binaryFile.GetAll();
            phoneEntries.Add(model);
            mockfile.Setup(m => m.WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false));
        }

        [Test(Description = "Test if entry is null, throws exception")]
        public void AddEntryNullException()
        {
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
            var phoneEntries = binaryFile.GetAll();
            PhoneEntryModel model = new PhoneEntryModel();
            model = null;
            mockfile.Verify(m => m.WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false), Times.Never);
        }

        [Test(Description = "Test if an entry with the same id with an entry from a list of object is edited on the file")]
        public void EditEntry()
        {
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
            var phoneEntries = binaryFile.GetAll();
            PhoneEntryModel model = new PhoneEntryModel
            {
                Id = 1,
                FirstName = "Kristi",
                LastName = "Mone",
                PhoneNumber = "+355682024896",
                EntryType = PhoneEntryType.WORK
            };
            for (int i = 0; i < phoneEntries.Count; i++)
            {
                if (phoneEntries[i].Id == model.Id)
                {
                    phoneEntries[i] = model;
                    break;
                }
            }
            mockfile.Setup(m => m.WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false));
        }

        [Test(Description = "Test if entry is null, throws exception")]
        public void EditEntryNullException()
        {
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
            var phoneEntries = binaryFile.GetAll();
            PhoneEntryModel model = new PhoneEntryModel();
            model = null;
            mockfile.Verify(m => m.WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false), Times.Never);
        }

        [Test(Description = "Test if id of the entry is different of the id of any of entries on the list, edit entry return false")]
        public void EditEntryDifferentIdFromFile()
        {
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
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
            Assert.IsFalse(binaryFile.Edit(model));
        }

        [Test(Description = "Test if an entry from a list of object will be deleted from the file")]
        public void DeleteEntry()
        {
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
            PhoneEntryModel model = new PhoneEntryModel
            {
                Id = 1,
                FirstName = "Kristi",
                LastName = "Mone",
                PhoneNumber = "+355682024896",
                EntryType = PhoneEntryType.WORK
            };
            var phoneEntries = binaryFile.GetAll();
            phoneEntries.RemoveAll(x => x.Id == model.Id);
            mockfile.Setup(m => m.WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false));
        }
        [Test(Description = "Test if an entry is null, throws exception")]
        public void DeleteEntryNullException()
        {
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
            var phoneEntries = binaryFile.GetAll();
            PhoneEntryModel model = new PhoneEntryModel();
            model = null;
            mockfile.Verify(m => m.WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false), Times.Never);
        }
        [Test(Description = "Test if id of the entry is different of the id of any of entries on the list, delete entry returns false")]
        public void DeleteEntryDifferentId_FromFile()
        {
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
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
                Assert.IsFalse(binaryFile.Delete(model));
        }
        [Test(Description = "Test if the list is null, throws Exception")]
        public void IterateListNullException()
        {
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
            var phoneEntries = binaryFile.GetAll();
            if (phoneEntries == null)
            mockfile.Verify(m => m.WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false), Times.Never);
        }
        [Test(Description = "Test if the list is iterated and order by firstname and written to file")]
        public void IterateListByFirstName()
        {
            bool orderByFirstName = true;
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
            var phoneEntries = binaryFile.GetAll();
            if(orderByFirstName)
            phoneEntries = phoneEntries.OrderBy(p => p.FirstName).ToList();
            mockfile.Setup(m => m.WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false));
        }
        [Test(Description = "Test if the list is iterated and order by lastname and written to file")]
        public void IterateListByLastName()
        {
            bool orderByFirstName = false;
            Mock<IPhoneBook> mockfile = new Mock<IPhoneBook>();
            BinaryFileManager binaryFile = new BinaryFileManager(mockfile.Object);
            var phoneEntries = binaryFile.GetAll();
            if (!orderByFirstName)
                phoneEntries = phoneEntries.OrderBy(p => p.LastName).ToList();
            mockfile.Setup(m => m.WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false));
        }
    }
}
