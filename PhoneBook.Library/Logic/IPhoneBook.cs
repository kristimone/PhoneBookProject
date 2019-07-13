using PhoneBook.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Library
{
    /// <summary>
    /// Interface with the functions which I will use for my project.
    /// </summary>
    public interface IPhoneBook
    {   
        /// <summary>
        /// GetAll function where I would getall entries from a file and store it in a list of object.
        /// </summary>
        /// <returns>List of object readed from the binary file.</returns>
        List<PhoneEntryModel> GetAll();
        /// <summary>
        /// Add function where I would store an instance of object to the binary file.
        /// </summary>
        /// <param name="entry">An instance of object PhoneEntryModel</param>
        /// <returns>boolean value true or false</returns>
        bool Add(PhoneEntryModel entry);
        /// <summary>
        /// Edit function where I would edit an entry from the binary file.
        /// </summary>
        /// <param name="entry">An instance of object PhoneEntryModel</param>
        /// <returns>boolean value true or false</returns>
        bool Edit(PhoneEntryModel entry);
        /// <summary>
        /// Delete function where I would delete an entry from the binary file.
        /// </summary>
        /// <param name="entry">An instance of object PhoneEntryModel</param>
        /// <returns>boolean value true or false</returns>
        bool Delete(PhoneEntryModel entry);
        /// <summary>
        /// Iterate function where I would itereate the list of objects, order by firstname or lastname and store it in the binary file.
        /// </summary>
        /// <param name="orderByFirstName">boolean value true orderbyfirstname, false orderbylastname</param>
        /// <returns>List of objects from PhoneEntryModel</returns>
        List<PhoneEntryModel> Iterate(bool orderByFirstName);
    }
}
