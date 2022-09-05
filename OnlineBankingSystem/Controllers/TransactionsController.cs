using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBankingSystem.Data;
using OnlineBankingSystem.Models;

namespace OnlineBankingSystem.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Admin")]
        // GET: Transactions
        public async Task<IActionResult> Index(string FromAccountNumber)
        {
            return View(await _context.Transaction.Where( x=> x.FromAccountNumber == FromAccountNumber).ToListAsync());
        }

        
        [Authorize]
        public async Task<IActionResult> Search(string searchstring)
        {
            ViewData["actiontype"] = "post";
            var transactions = await _context.Transaction.ToListAsync();
            if (!String.IsNullOrEmpty(searchstring))
            {
                transactions = await _context.Transaction.Where(m => m.TransactionTime.ToString()!.Contains(searchstring)).AsNoTracking().ToListAsync();

            }
            var transactionSearch = new SearchTransaction
            {
                Transactions = transactions
            };
            return View(transactionSearch);
        }

        [Authorize]
        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [Authorize]
        // GET: Transactions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ToAccountNumber,TransactionStatus, TransactionAmount")] Transaction transaction)
        {
            ViewData["error"] = "No error";
            //transaction.TransactionStatus = "success";
            if (transaction.ToAccountNumber != null && transaction.TransactionAmount > 0 )
            {
                var username = User.Claims.FirstOrDefault(y => y.Type == "Username").Value;
                Account Fromaccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == username);
                Account ToAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == transaction.ToAccountNumber);
                if(ToAccount == null || Fromaccount.AccountNumber==ToAccount.AccountNumber || Fromaccount.AccountNumber == transaction.ToAccountNumber)
                {
                    //add if for same to address and from address
                    ViewData["error"] = "Sender Account number doesnt exist!\n Please check account number again";
                    return View(transaction);
                }
                if (!Fromaccount.Freezed && !ToAccount.Freezed)
                {
                    
                    if (Fromaccount.Balance - transaction.TransactionAmount >= 0)
                    {
                        transaction.FromAccountNumber = Fromaccount.AccountNumber;
                        transaction.TransactionTime = DateTime.Now;
                        transaction.TransactionStatus = "success";
                        
                        Fromaccount.Balance -= transaction.TransactionAmount;
                        Fromaccount.NumberOfTransactions += 1;
                        ToAccount.Balance += transaction.TransactionAmount;
                        ToAccount.NumberOfTransactions += 1;

                        _context.Update(Fromaccount);
                        _context.Add(transaction);

                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ViewData["error"] = "Insufficient Balance! ";
                        return View(transaction);
                    }
                }

                
                else
                {
                    ViewData["error"] = Fromaccount.Freezed ? "Your Account is Freezed!" : "Sender Account is Freezed";
                    return View(transaction);
                }

                //return RedirectToAction(nameof(Index), Fromaccount.AccountNumber);
                return RedirectToAction(actionName:"Dashboard", controllerName: "Users");

            }
            ViewData["error"] = "Enter Correct Account number and Amount";
            return View(transaction);
        }



        //[Authorize(Roles = "Admin")]
        //// GET: Transactions/Delete/5
        //public async Task<IActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var transaction = await _context.Transaction
        //        .FirstOrDefaultAsync(m => m.TransactionId == id);
        //    if (transaction == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(transaction);
        //}

        // POST: Transactions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long id)
        //{
        //    var transaction = await _context.Transaction.FindAsync(id);
        //    _context.Transaction.Remove(transaction);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [Authorize]
        private bool TransactionExists(long id)
        {
            return _context.Transaction.Any(e => e.TransactionId == id);
        }
    }
}
