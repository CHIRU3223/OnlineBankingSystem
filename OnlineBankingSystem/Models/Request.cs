using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBankingSystem.Models
{
    public class Request
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "User")]
        public virtual string Username { get; set; }

        [ForeignKey("Username")]
        public virtual User ReqUser { get; set; }

        [Display(Name = "Account")]
        public virtual string AccountNumber { get; set; }

        [ForeignKey("AccountNumber")]
        public virtual string ReqAccountNumber { get; set; }
        
        [Required]
        public string RequestMsg { get; set; }

        public string RequestAction { get; set; }

        public bool Progress { get; set; }

        public string RequestRemarks { get; set; }


    }
}
