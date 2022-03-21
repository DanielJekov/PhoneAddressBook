namespace PhoneAddressBook.Controllers
{
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PhoneAddressBook.Models.Records;
    using PhoneAddressBook.Models.Search;
    using PhoneAddressBook.Services.Search;

    [Authorize]
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public IActionResult Result(string input)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var byNames = this.searchService
                              .GetByNames(input, userId)
                              .Select(x => new RecordViewModel
                              {
                                  Id = x.Id,
                                  FirstName = x.FirstName,
                                  LastName = x.LastName,
                                  FullNumber = x.FullNumber,
                              })
                              .ToList();

            var byPhoneNumber = this.searchService
                                    .GetByPhoneNumber(input, userId)
                                    .Select(x => new RecordViewModel
                                    {
                                        Id = x.Id,
                                        FirstName = x.FirstName,
                                        LastName = x.LastName,
                                        FullNumber = x.FullNumber,
                                    })
                                    .ToList();

            var viewModel = new SearchViewModel()
            {
                ResultsByNames = byNames,
                ResultsByPhoneNumber = byPhoneNumber,
            };

            return this.View(viewModel);
        }
    }
}
