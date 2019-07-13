using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PhoneBook.Library.Models;

namespace PhoneBook.Library.Source.BinaryFile
{
    /// <summary>
    /// Class BinaryFileManager where I managed the functions used for my project.
    /// </summary>
    public class BinaryFileManager : IPhoneBook
    {
        private readonly object _sysObject = new Object();

        /// <summary>
        /// Constructor with no parameters.
        /// </summary>
        public BinaryFileManager()
        {
        }

        /// <summary>
        /// Create a new binary file if it doesnt exist, if the file exist and has a length bigger than 10 MB it will be deleted and recreated again.
        /// If the file is created, the file will be open for append.
        /// </summary>
        /// <returns>boolean value true if the file is created or opened and false if the function catch an exception</returns>
        public bool CreateFile()
        {
            var flagToReturn = false;
            try
            {

                if (!File.Exists(Constants.FilePath))
                {
                    var file = File.Create(Constants.FilePath);
                    file.Close();
                }
                else
                {
                    FileInfo fi = new FileInfo(Constants.FilePath);
                    if (fi.Length > Constants.FileMaxSize)
                    {
                        File.Delete(Constants.FilePath);
                        var file = File.Create(Constants.FilePath);
                        file.Close();
                    }
                    else
                    {
                        var file = File.Open(Constants.FilePath, FileMode.Append);
                        file.Close();
                    }
                }

                flagToReturn = true;
            }
            catch (Exception ex)
            {
                flagToReturn = false;
            }

            return flagToReturn;
        }

        /// <summary>
        /// Get all entries from the file after been deserialized and write it to a list of objects.
        /// <typeparam name="List">The list of object that will be written from the file.</typeparam>
        /// </summary>
        /// <returns>List of objects with PhoneEntry read it from the binary file</returns>
        public List<PhoneEntryModel> GetAll()
        {
            lock (_sysObject)
            {
                var tmp = new List<PhoneEntryModel>();

                tmp = ReadFromBinaryFile<List<PhoneEntryModel>>(Constants.FilePath) ?? new List<PhoneEntryModel>();

                return tmp;
            }
        }

        /// <summary>
        /// /// Add an object to the list and write the list to a binary file.
        /// <typeparam name="bool">The boolean type if an objected is added to the list and the list is written to the binary file function will return true</typeparam>
        /// </summary>
        /// <param name="entry">The PhoneEntryModel instance object.</param>
        /// <returns>boolean value true if the entry passed as parameter is added to the file and false if the entry with that id exists in the file.</returns>
        public bool Add(PhoneEntryModel entry)
        {
            lock (_sysObject)
            {
                var phoneEntries = GetAll().ToList();
                if (entry == null)
                    throw new ArgumentNullException(nameof(entry));

                if (phoneEntries.Any(x => x.Id == entry.Id)) return false;

                phoneEntries.Add(entry);
                WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false);

                return true;
            }
        }

        /// <summary>
        ///  Edit an object from the list after been deserialized and write the new list to a binary file.
        /// <typeparam name="bool">The boolean type if an objected is edited from the list and the new list is written to a file with the new edited object function will return true</typeparam>
        /// </summary>
        /// <param name="entry">The PhoneEntryModel instance object.</param>
        /// <returns>boolean value true if the entry passed as parameter is edited from the file and false if the entry with that id doesnt exists in the file.</returns>
        public bool Edit(PhoneEntryModel entry)
        {
            lock (_sysObject)
            {
                var phoneEntries = GetAll().ToList();

                if (entry == null)
                    throw new ArgumentNullException(nameof(entry));

                if (!phoneEntries.Any(x => x.Id == entry.Id)) return false;

                for (int i = 0; i < phoneEntries.Count; i++)
                {
                    if (phoneEntries[i].Id == entry.Id)
                    {
                        phoneEntries[i] = entry;
                        break;
                    }
                }

                WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false);

                return true;
            }
        }

        /// <summary>
        /// /// Delete an object from the list after been deserialized and write the new list to a binary file.
        /// <typeparam name="bool">The boolean type if an objected is deleted from the list and the new list is written without that object to a file function will return true</typeparam>
        /// </summary>
        /// <param name="entry">The PhoneEntryModel instance object.</param>
        /// <returns>boolean value true if the entry passed as parameter is deleted from the file and false if the entry with that id doesnt exists in the file.</returns>
        public bool Delete(PhoneEntryModel entry)
        {
            lock (_sysObject)
            {
                var phoneEntries = GetAll().ToList();

                if (entry == null)
                    throw new ArgumentNullException(nameof(entry));

                if (!phoneEntries.Any(x => x.Id == entry.Id)) return false;

                phoneEntries.RemoveAll(x => x.Id == entry.Id);

                WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries);

                return true;
            }
        }

        /// <summary>
        /// Iterate the list with objects by order of the firstname or lastname and writes the given object instance to a binary file.
        /// <typeparam name="List">The list of object with phone entries being iterating and written to the binary file.</typeparam>
        /// </summary>
        /// <param name="orderByFirstName">The boolean value if it is true the list will order by firstname, if it is false the list will order by lastname.</param>
        /// <returns>List of objects with entries ordered by firstname if orderByFirstName is true and ordered by lastname if orderByFirstName is false</returns>
        public List<PhoneEntryModel> Iterate(bool orderByFirstName)
        {
            lock (_sysObject)
            {
                var phoneEntries = GetAll().ToList();

                if (phoneEntries == null)
                    throw new ArgumentNullException(nameof(phoneEntries));

                if (orderByFirstName == true)
                    phoneEntries = phoneEntries.OrderBy(p => p.FirstName).ToList();
                else
                    phoneEntries = phoneEntries.OrderBy(p => p.LastName).ToList();

                WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries);

                return phoneEntries;
            }
        }

        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// <typeparam name="T">The type of object being written to the binary file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the binary file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>

        private void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// Reads an object instance from a binary file.
        /// <typeparam name="T">The type of object to read from the binary file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        private T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                if (stream.Length == 0)
                    return default(T);
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
