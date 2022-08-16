using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBankingSystem.Models
{
    public class Account
    {
        [Key]
        public string AccountNumber { get; set; }

        [Display(Name = "User")]
        public virtual string Username { get; set; }
        [ForeignKey("Username")]
        public virtual User AccUsername { get; set; }

        public bool Freezed { get; set; }

        public Int64 Balance { get; set; }

        public int NumberOfTransactions { get; set; }

        public bool Checkbook { get; set; }
    }
}
