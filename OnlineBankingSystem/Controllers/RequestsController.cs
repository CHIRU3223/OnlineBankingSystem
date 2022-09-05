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
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requests

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Requests.Include(r => r.ReqUser);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> MyRequests()
        {
            var username = User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            var request = await _context.Requests.Where(x => x.Username == username && x.AccountNumber!= null).AsNoTracking().ToListAsync();

            return View(request); 
        }
        //public string hello()
        //{

        //}
        public async Task<IActionResult> RequestCheckbook(string? id) {
            var username = User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            Request request = new Request();
            request.Username = username;
            request.AccountNumber = id;
            request.Progress = false;
            request.RequestMsg = "Request Checkbook";
            _context.Add(request);
            await   _context.SaveChangesAsync();
            return RedirectToAction(actionName: "MyRequests", controllerName:"Requests") ;
        }

        public async Task<IActionResult> NewRequest(string? id, string? value)
        {
            var username = User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            Request request = new Request();
            request.Username = username;
            request.AccountNumber = id;
            request.Progress = false;
            if (value == "checkbook")
                request.RequestMsg = "Request Checkbook";
            else if (value == "freeze")
                request.RequestMsg = "Request Freeze";
            else if ( value == "unfreeze")
                request.RequestMsg = "Request UnFreeze";

            _context.Add(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: "MyRequests", controllerName: "Requests");
        }

        [Authorize]
        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.ReqUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }


        [Authorize]
        // GET: Requests/Create
        public IActionResult Create()
        {
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Username,AccountNumber,ReqAccountNumber,RequestMsg,RequestAction,Progress,RequestRemarks")] Request request)
        {
            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(actionName:"Dashboard", controllerName: "Users");
            }
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", request.Username);
            return View(request);
        }

        [Authorize]
        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", request.Username);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Username,AccountNumber,ReqAccountNumber,RequestMsg,RequestAction,Progress,RequestRemarks")] Request request)
        {
            if (id != request.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.ID))
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
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", request.Username);
            return View(request);
        }

        [Authorize]
        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.ReqUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        [Authorize]
        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.ID == id);
        }
    }
}
