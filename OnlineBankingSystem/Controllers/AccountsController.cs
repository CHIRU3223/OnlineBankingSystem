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
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        [Authorize(Roles ="Admin")]
        [Route("Accounts")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Accounts.Include(a => a.AccUsername);
            return View(applicationDbContext);
        }

        [Authorize]
        public async Task<IActionResult> MyAccountBalance()
        {
            var username = User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == username);
            //var balance = await _context.Accounts.FirstOrDefaultAsync(x => x.AccUsername == username);
            Console.WriteLine(user);
            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> Details()
        {
            var username = User.Claims.FirstOrDefault(y => y.Type == "Username").Value;
            var account = await _context.Accounts.Include(a => a.AccUsername).FirstOrDefaultAsync(x => x.Username == username);
            return View(account);
        }
        // GET: Accounts/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var account = await _context.Accounts
        //        .Include(a => a.AccUsername)
        //        .FirstOrDefaultAsync(m => m.AccountNumber == id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(account);
        //}

        // GET: Accounts/Create

        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Freezed,Balance,Checkbook")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.NumberOfTransactions = 0;
                User updateUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == account.Username);
                updateUser.NoOfAccounts += 1;
                _context.Add(account);
                _context.Update(updateUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", account.Username);
            return View(account);
        }

        // GET: Accounts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", account.Username);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AccountNumber,Username,Freezed,Balance,NumberOfTransactions,Checkbook")] Account account)
        {
            if (id != account.AccountNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", account.Username);
            return View(account);
        }

        // GET: Accounts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.AccUsername)
                .FirstOrDefaultAsync(m => m.AccountNumber == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool AccountExists(string id)
        {
            return _context.Accounts.Any(e => e.AccountNumber == id);
        }
    }
}
