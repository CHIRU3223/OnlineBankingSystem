using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBankingSystem.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int PhoneNo { get; set; }
        [Required]
        public int SSN { get; set; }

        [Required]
        public DateTime DoB { get; set; }

        [Required]
        public DateTime UserCreated { get; set; }
        [Required]
        public Boolean isAdmin { get; set; }
        [Required]
        public string email { get; set; }

        public int NoOfAccounts { get; set; }
    }
}
