namespace PhoneAddressBook.Models.Records
{
    using System.ComponentModel.DataAnnotations;

    public class RecordCreateInputModel
    {
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(25)]
        public string CountryCallCode { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        [MaxLength(26)]
        public string PhoneNumber { get; set; }
    }
}
