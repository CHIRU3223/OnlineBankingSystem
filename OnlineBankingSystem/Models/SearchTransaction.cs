using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBankingSystem.Models
{
    public class SearchTransaction
    {
        public List<Transaction>? Transactions { get; set; }
        public string? SearchString { get; set; }
    }
}
