namespace PhoneAddressBook.Services.Records
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PhoneAddressBook.Data;
    using PhoneAddressBook.Data.Models;
    using PhoneAddressBook.Services.Records.Dtos;

    public class RecordsService : IRecordsService
    {
        private readonly ApplicationDbContext db;

        public RecordsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task CreateAsync(RecordCreateinputDto input, string userId)
        {
            var country = this.db.Countries
                                 .Where(c => c.PhoneCode == input.CountryCallCode)
                                 .FirstOrDefault();

            // Adding Default code if this from input doens't exist.
            if (country == null)
            {
                country = this.db.Countries
                                 .Where(c => c.PhoneCode == @"+359")
                                 .FirstOrDefault();
            }

            var record = new Record()
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                PhoneNumber = input.PhoneNumber,
                UserId = userId,
                CountryId = country.Id
            };

            this.db.Records.Add(record);
            await this.db.SaveChangesAsync();
        }

        public IEnumerable<RecordServiceDto> GetAll(string userId, int page, int itemsPerPage)
        {
            return this.db.Records
                          .Where(x => x.User.Id == userId)
                          .Select(x => new RecordServiceDto
                          {
                              Id = x.Id,
                              FirstName = x.FirstName,
                              LastName = x.LastName,
                              FullNumber = x.Country.PhoneCode + x.PhoneNumber
                          })
                          .ToList()
                          .OrderByDescending(x => x.Id)
                          .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                          .ToList();
        }

        public async Task DeleteAsync(int id)
        {
            var record = await this.db.Records.FindAsync(id);
            this.db.Records.Remove(record);
            await this.db.SaveChangesAsync();
        }

        public int GetCount(string userId)
        {
            return this.db.Records.Where(x => x.User.Id == userId).Count();
        }
    }
}
