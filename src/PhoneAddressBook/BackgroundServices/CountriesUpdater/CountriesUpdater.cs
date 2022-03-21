namespace PhoneAddressBook.BackgroundServices.CountriesUpdater
{
    using System.Threading.Tasks;

    using DNTScheduler.Core.Contracts;
    using PhoneAddressBook.Services.Countries;

    public class CountriesUpdater : IScheduledTask
    {
        private readonly ICountriesService countriesService;

        public CountriesUpdater(
            ICountriesService countriesService)
        {
            this.countriesService = countriesService;
        }

        public bool IsShuttingDown { get; set; }

        public async Task RunAsync()
        {
            await countriesService.UpdateAsync();
        }
    }
}
