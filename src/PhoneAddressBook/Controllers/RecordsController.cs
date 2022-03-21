namespace PhoneAddressBook.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PhoneAddressBook.Models.Records;
    using PhoneAddressBook.Services.Records;
    using PhoneAddressBook.Services.Records.Dtos;

    [Authorize]
    public class RecordsController : Controller
    {
        private readonly IRecordsService recordsService;

        public RecordsController(IRecordsService recordsService)
        {
            this.recordsService = recordsService;
        }
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecordCreateInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }
            var recordServiceDto = new RecordCreateinputDto()
            {
                FirstName = input.FirstName.Trim(),
                LastName = input.LastName.Trim(),
                PhoneNumber = input.PhoneNumber.Trim(),
                CountryCallCode = input.CountryCallCode.Trim(),
            };

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.recordsService.CreateAsync(recordServiceDto, userId);

            return this.Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.recordsService.DeleteAsync(id);

            return this.Redirect("/");
        }
    }
}
