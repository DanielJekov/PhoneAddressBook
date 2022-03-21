namespace PhoneAddressBook.Models.Search
{
    using System.Collections.Generic;

    using PhoneAddressBook.Models.Records;

    public class SearchViewModel
    {
        public ICollection<RecordViewModel> ResultsByNames { get; set; }

        public ICollection<RecordViewModel> ResultsByPhoneNumber { get; set; }
    }
}
