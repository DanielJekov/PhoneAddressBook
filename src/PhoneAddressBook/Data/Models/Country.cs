namespace PhoneAddressBook.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        public Country()
        {
            this.Records = new HashSet<Record>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(2)]
        public string ISO2Code { get; set; }

        [Required]
        [MaxLength(3)]
        public string ISO3Code { get; set; }

        [Required]
        [MaxLength(25)]
        public string PhoneCode { get; set; }

        public ICollection<Record> Records { get; set; }
    }
}
