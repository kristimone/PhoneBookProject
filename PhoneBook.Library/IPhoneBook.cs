using PhoneBook.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Library
{
    public interface IPhoneBook
    {
        List<PhoneEntryModel> GetAll();
        bool Add(PhoneEntryModel entry);
        bool Edit(PhoneEntryModel entry);
        bool Delete(PhoneEntryModel entry);
        List<PhoneEntryModel> Iterate(bool orderByFirstName);
    }
}
