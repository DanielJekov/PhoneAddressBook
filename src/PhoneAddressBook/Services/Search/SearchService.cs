namespace PhoneAddressBook.Services.Search
{
    using System.Collections.Generic;
    using System.Linq;

    using PhoneAddressBook.Data;
    using PhoneAddressBook.Services.Records.Dtos;

    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext db;
        public SearchService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<RecordServiceDto> GetByNames(string input, string userId)
        {
            return this.db.Records
                       .Where(x => x.User.Id == userId)
                       .Where(x =>
                        x.FirstName.Contains(input) ||
                        x.LastName.Contains(input) ||
                        (x.FirstName + " " + x.LastName).Contains(input))
                       .Select(x => new RecordServiceDto
                       {
                           Id = x.Id,
                           FirstName = x.FirstName,
                           LastName = x.LastName,
                           FullNumber = x.Country.PhoneCode + x.PhoneNumber
                       })
                       .ToList();
        }

        public IEnumerable<RecordServiceDto> GetByPhoneNumber(string phoneNumber, string userId)
        {
            return this.db.Records
                       .Where(x => x.User.Id == userId)
                       .Where(x => (x.Country.PhoneCode + x.PhoneNumber).Contains(phoneNumber))
                       .Select(x => new RecordServiceDto
                       {
                           Id = x.Id,
                           FirstName = x.FirstName,
                           LastName = x.LastName,
                           FullNumber = x.Country.PhoneCode + x.PhoneNumber
                       })
                       .ToList();
        }
    }
}
