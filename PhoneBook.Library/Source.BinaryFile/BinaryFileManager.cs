using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneBook.Library.Models;

namespace PhoneBook.Library.Source.BinaryFile
{
    public class BinaryFileManager : IPhoneBook
    {
        private bool FileExists => File.Exists(Constants.FilePath);
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

        public List<PhoneEntryModel> GetAll()
        {
            var tmp = new List<PhoneEntryModel>();
            var foo = new PhoneEntryModel();

            //using (var file = File.Open(Constants.FilePath, FileMode.Open))
            //{
            //    while ((foo = Serializer.DeserializeWithLengthPrefix<PhoneEntryModel>(file, ProtoBuf.PrefixStyle.Base128, 1)) != null)
            //    {
            //        tmp.Add(foo);
            //    }
            //}

            //while( (foo = ReadFromBinaryFile<PhoneEntryModel>(Constants.FilePath)) != null)
            //{
            //    tmp.Add(foo);
            //}

            tmp = ReadFromBinaryFile<List<PhoneEntryModel>>(Constants.FilePath) ?? new List<PhoneEntryModel>();

            return tmp;
        }
        public bool Add(PhoneEntryModel entry)
        {
            var phoneEntries = GetAll().ToList();
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            if (phoneEntries.Any(x => x.Id == entry.Id)) return false;

            //using (var file = File.Open(Constants.FilePath, FileMode.Append))
            //{
            //    Serializer.SerializeWithLengthPrefix(file, entry, PrefixStyle.Base128, 1);
            //    file.SetLength(file.Position);
            //}
            phoneEntries.Add(entry);
            WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath, phoneEntries, false);

            return true;
        }

        public bool Edit(PhoneEntryModel entry)
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

            //using (var file = File.Open(Constants.FilePath, FileMode.Truncate))
            //{
            //    Serializer.SerializeWithLengthPrefix(file, phoneEntries, PrefixStyle.Base128, 1);
            //    //file.SetLength(file.Position);
            //}

            WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath,phoneEntries,false);

            return true;
        }

        public bool Delete(PhoneEntryModel entry)
        {
            var phoneEntries = GetAll().ToList();

            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            if (!phoneEntries.Any(x => x.Id == entry.Id)) return false;

            phoneEntries.RemoveAll(x => x.Id == entry.Id);

            //using (var file = File.Open(Constants.FilePath, FileMode.Open))
            //{
            //    Serializer.SerializeWithLengthPrefix(file, phoneEntries, PrefixStyle.Base128, 1);
            //    file.SetLength(file.Position);
            //}

            WriteToBinaryFile<List<PhoneEntryModel>>(Constants.FilePath,phoneEntries);

            return true;
        }

        /// <summary>
        ///     Returns the list of
        /// </summary>
        /// <param name="orderByFirstName"></param>
        /// <returns></returns>
        public List<PhoneEntryModel> Iterate(bool orderByFirstName)
        {
            var phoneEntries = GetAll().ToList();

            if (phoneEntries == null)
                throw new ArgumentNullException(nameof(phoneEntries));

            if (orderByFirstName == true)
                phoneEntries = phoneEntries.OrderBy(p => p.FirstName).ToList();
            else
                phoneEntries = phoneEntries.OrderBy(p => p.LastName).ToList();

            return phoneEntries;
        }

        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
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

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the binary file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        private  T ReadFromBinaryFile<T>(string filePath)
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
