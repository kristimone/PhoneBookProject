using System;

namespace PhoneBook.Library.Models
{
    [Serializable]
    public class PhoneEntryModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public PhoneEntryType EntryType { get; set; }

        public string PhoneNumber { get; set; }

        public override string ToString() => $"Id={Id} - FirstName={FirstName} - LastName={LastName} - EntryType={EntryType.ToString()} - PhoneNumber={PhoneNumber}";
    }
}
