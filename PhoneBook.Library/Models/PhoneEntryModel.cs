using System;

namespace PhoneBook.Library.Models
{
    /// <summary>
    /// Class PhoneEntryModel which I stored: id, firstname, lastname, entrytype, phonenumber of the entry. 
    /// </summary>
    [Serializable]
    public class PhoneEntryModel : IEquatable<PhoneEntryModel>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public PhoneEntryType EntryType { get; set; }

        public string PhoneNumber { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as PhoneEntryModel);
        }

        public bool Equals(PhoneEntryModel other)
        {
            if (other == null) return false;

            return other.Id.Equals(Id) && other.FirstName.Equals(FirstName) && other.LastName.Equals(LastName);
        }

        public override string ToString() => $"Id={Id} - FirstName={FirstName} - LastName={LastName} - EntryType={EntryType.ToString()} - PhoneNumber={PhoneNumber}";
    }
}
