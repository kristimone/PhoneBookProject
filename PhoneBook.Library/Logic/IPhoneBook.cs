using PhoneBook.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Library
{
    /// Interface with the functions which I will use for my project.
    public interface IPhoneBook
    {
        /// GetAll function where I would get all the entries of the file and stored it in a list of objects.
        List<PhoneEntryModel> GetAll();
        /// Add function where I would store an instance of object to the binary file.
        bool Add(PhoneEntryModel entry);
        /// Edit function where I would edit an entry from the binary file.
        bool Edit(PhoneEntryModel entry);
        /// Delete function where I would delete an entry from the binary file.
        bool Delete(PhoneEntryModel entry);
        /// Iterate function where I would itereate the list of objects, order by firstname or lastname and store it in the binary file.
        List<PhoneEntryModel> Iterate(bool orderByFirstName);
        /// ReadFromBinaryFile function for reading entries from the binary file and store it into a list of ojects by using the method of deserialize the file.
        T ReadFromBinaryFile<T>(string filePath);
        /// WriteToBinaryFile function for writing entries into the binary file by using the method of serialize objects.
        void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false);
    }
}
