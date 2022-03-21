namespace PhoneAddressBook.Services.Countries
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using PhoneAddressBook.BackgroundServices.CountriesUpdater.Dtos;
    using PhoneAddressBook.Data;
    using PhoneAddressBook.Data.Models;
    using PhoneAddressBook.Models.Countries;
    using PhoneAddressBook.Services.Countries.Dtos;

    public class CountriesService : ICountriesService
    {

        private readonly ApplicationDbContext db;

        public CountriesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CountryViewModel> GetAll()
        {
            return this.db.Countries
                          .Select(x => new CountryViewModel
                          {
                              Iso3Code = x.ISO3Code,
                              CallCode = x.PhoneCode,
                          })
                          .ToList();
        }

        public async Task<IEnumerable<Iso2AndPhoneCodeDto>> GetIso2AndPhoneCodes()
        {
            var jsonResult = await GetHtmlAsync(@"http://country.io/phone.json");
            jsonResult = Regex.Replace(jsonResult, @"""|{|}", string.Empty);
            var resultSplit = jsonResult.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var collection = new HashSet<Iso2AndPhoneCodeDto>();
            foreach (var countryRaw in resultSplit)
            {
                var countrySplitted = countryRaw.Split(":", StringSplitOptions.RemoveEmptyEntries);
                var iso2Code = countrySplitted[0].Trim();
                var callCode = countrySplitted[1].Trim();

                if (!callCode.StartsWith("+"))
                {
                    callCode = string.Concat("+" , callCode);
                }
               
                collection.Add(new Iso2AndPhoneCodeDto { Iso2 = iso2Code, PhoneCode = callCode });
            }

            return collection;
        }

        public async Task<IEnumerable<Iso2AndIso3CodeDto>> GetIso2AndIso3Codes()
        {
            var jsonResult = await GetHtmlAsync(@"http://country.io/iso3.json");
            jsonResult = Regex.Replace(jsonResult, @"""|{|}", string.Empty);
            var resultSplit = jsonResult.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var collection = new HashSet<Iso2AndIso3CodeDto>();
            foreach (var countryRaw in resultSplit)
            {
                var countrySplitted = countryRaw.Split(":", StringSplitOptions.RemoveEmptyEntries);
                var iso2Code = countrySplitted[0].Trim();
                var iso3Code = countrySplitted[1].Trim();

                collection.Add(new Iso2AndIso3CodeDto { Iso2 = iso2Code, Iso3 = iso3Code });
            }

            return collection;
        }

        private async Task<string> GetHtmlAsync(string url)
        {
            string content = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                content = await reader.ReadToEndAsync();
            }

            return content;
        }

        public async Task UpdateAsync()
        {
            var result1 = await this.GetIso2AndIso3Codes();
            var result2 = await this.GetIso2AndPhoneCodes();
            var countries = new HashSet<CountryUpdaterDto>();
            foreach (var countryRaw in result1)
            {
                var country = new CountryUpdaterDto()
                {
                    Iso2 = countryRaw.Iso2,
                    Iso3 = countryRaw.Iso3,
                    CodeNumber = result2.Where(x => x.Iso2 == countryRaw.Iso2).FirstOrDefault().PhoneCode
                };

                countries.Add(country);
            }

            var currentCountriesReadHash = countries.GetHashCode().ToString();
            var dbCountriesHash = this.db.CountriesDataHash.Select(x => x.Hash).FirstOrDefault();

            if (dbCountriesHash == null)
            {
                foreach (var country in countries)
                {
                    this.db.Countries.Add(
                        new Country
                        {
                            ISO2Code = country.Iso2,
                            ISO3Code = country.Iso3,
                            PhoneCode = country.CodeNumber
                        });
                }

                await this.db.SaveChangesAsync();
                this.db.CountriesDataHash.Add(new CountriesDataHash { Hash = currentCountriesReadHash });
                await this.db.SaveChangesAsync();

                return;
            }

            if (currentCountriesReadHash == dbCountriesHash)
            {
                return;
            }

            var dbCountries = db.Countries.ToList();
            foreach (var dbCountry in dbCountries)
            {
                dbCountry.PhoneCode = countries.Where(x => x.Iso3 == dbCountry.ISO3Code).FirstOrDefault().CodeNumber;
            }

            await this.db.SaveChangesAsync();
        }
    }
}
