using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBankingSystem.Models
{
    public class Transaction
    {
        [Key]
        public Int64 TransactionId { get; set; }


        public string FromAccountNumber { get; set; }
        [Required]
     
        [Display(Name = "ToAccount")]
        public virtual string ToAccountNumber { get; set; }
        [ForeignKey("AccountNumber")]
        public virtual Account ToAccount { get; set; }

        [Required]
        public DateTime TransactionTime { get; set; }
        [Required]
        public Int64 TransactionAmount { get; set; }
        [Required]
        public string TransactionStatus { get; set; }

        public string TransactionMessage { get; set; }

    }
}
