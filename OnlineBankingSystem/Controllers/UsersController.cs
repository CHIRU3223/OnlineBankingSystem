using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBankingSystem.Data;
using OnlineBankingSystem.Models;

namespace OnlineBankingSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        // GET: Users
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //var users = _context.Users.ToListAsync(
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> Dashboard()
        {
            var username = User.Claims.FirstOrDefault(y => y.Type == "Username").Value;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == username);
            ViewData["AccountBalance"] = account.Balance;
            //var transactions = await _context.Transaction.FromSqlInterpolated($"SELECT * FROM Transaction WHERE FromAccount={account.AccountNumber}").AsNoTracking().ToListAsync();
            //var transactions = await _context.Transaction.ToListAsync(x => x.FromAccountNumber == account.AccountNumber);
            var transactions = await _context.Transaction.Where(m => m.FromAccountNumber == account.AccountNumber).Take(10).AsNoTracking().ToListAsync();
            ViewData["Transactions"] = transactions;
            
            return View(transactions);

        }
        public async Task<IActionResult> MyDetails()
        {
            var username = User.Claims.FirstOrDefault(y => y.Type == "Username").Value;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == username);
            ViewData["AccountBalance"] = account.Balance;
            return View(user);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password, string returnUrl)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Username == username);
            ViewData["ReturnUrl"] = returnUrl;
            if(user.Username == username && user.Password == password)
            {
                var userRole = (!user.isAdmin) ? "User" : "Admin";
                var claims = new List<Claim>();
                claims.Add(new Claim("Username", username));
                //claims.Add(new Claim("UserType", userRole));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                claims.Add(new Claim("Roles", userRole));
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                await HttpContext.SignInAsync(claimPrincipal);
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction( actionName: "Index", controllerName: "Home");
        }
        // GET: Users/Details/5
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Username == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Password,Name,PhoneNo,SSN,DoB,UserCreated,isAdmin,email,NoOfAccounts")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [Bind("Username,Password,Name,PhoneNo,SSN,DoB,UserCreated,isAdmin,email,NoOfAccounts")] User user)
        {
            if (id != user.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Username))
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
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Username == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Username == id);
        }
    }
}
