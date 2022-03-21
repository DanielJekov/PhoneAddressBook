namespace PhoneAddressBook.Models.Records
{
    using System.Collections.Generic;

    public class RecordListViewModel : PagingViewModel
    {
        public IEnumerable<RecordViewModel> Records { get; set; }
    }
}
