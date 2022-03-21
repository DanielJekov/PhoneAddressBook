namespace PhoneAddressBook.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;

    using PhoneAddressBook.Models;
    using PhoneAddressBook.Models.Records;
    using PhoneAddressBook.Services.Records;

    public class HomeController : Controller
    {
        private readonly IRecordsService recordService;

        public HomeController(IRecordsService recordService)
        {
            this.recordService = recordService;
        }

        public IActionResult Index(int id = 1)
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }
            if (this.User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var recordsDtos = this.recordService.GetAll(userId, id);
                var recordsModelCollection = recordsDtos
                   .Select(x => new RecordViewModel
                   {
                       Id = x.Id,
                       FirstName = x.FirstName,
                       LastName = x.LastName,
                       FullNumber = x.FullNumber
                   })
                   .ToList();

                const int ItemsPerPage = 12;

                var viewModel = new RecordListViewModel()
                {
                    ItemsPerPage = ItemsPerPage,
                    PageNumber = id,
                    RecordsCount = this.recordService.GetCount(userId),
                    Records = recordsModelCollection,
                };
                return this.View(viewModel);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
