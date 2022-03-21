namespace PhoneAddressBook.Services.Search
{
    using System.Collections.Generic;

    using PhoneAddressBook.Services.Records.Dtos;

    public interface ISearchService
    {
        IEnumerable<RecordServiceDto> GetByNames(string input, string userId);

        IEnumerable<RecordServiceDto> GetByPhoneNumber(string phoneNumber, string userId);
    }
}
