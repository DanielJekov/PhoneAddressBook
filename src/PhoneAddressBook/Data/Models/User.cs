namespace PhoneAddressBook.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Records = new HashSet<Record>();
        }

        [Required]
        [MaxLength(36)]
        public override string Id { get; set; }

        public ICollection<Record> Records { get; set; }
    }
}
