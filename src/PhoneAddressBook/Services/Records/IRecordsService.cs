namespace PhoneAddressBook.Services.Records
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PhoneAddressBook.Services.Records.Dtos;

    public interface IRecordsService
    {
        IEnumerable<RecordServiceDto> GetAll(string userId, int page, int itemsPerPage = 12);

        Task CreateAsync(RecordCreateinputDto input, string userId);

        Task DeleteAsync(int id);

        int GetCount(string userId);
    }
}
