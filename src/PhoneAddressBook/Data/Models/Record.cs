namespace PhoneAddressBook.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Record
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(26)]
        public string PhoneNumber { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        [Required]
        [MaxLength(36)]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
