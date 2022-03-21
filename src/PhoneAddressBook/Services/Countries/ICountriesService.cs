namespace PhoneAddressBook.Services.Countries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PhoneAddressBook.Models.Countries;
    using PhoneAddressBook.Services.Countries.Dtos;

    public interface ICountriesService
    {
        IEnumerable<CountryViewModel> GetAll();

        Task<IEnumerable<Iso2AndPhoneCodeDto>> GetIso2AndPhoneCodes();

        Task<IEnumerable<Iso2AndIso3CodeDto>> GetIso2AndIso3Codes();

        Task UpdateAsync();
    }
}
