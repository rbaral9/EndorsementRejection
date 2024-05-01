using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EndorsementRejection.Data;
using EndorsementRejection.Models.Entities;

namespace EndorsementRejection.Controllers
{
    public class EndoUsersController : Controller
    {
        private readonly AppDbContext _context;

        public EndoUsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EndoUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.EndoUsers.ToListAsync());
        }

        // GET: EndoUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endoUser = await _context.EndoUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endoUser == null)
            {
                return NotFound();
            }

            return View(endoUser);
        }

        // GET: EndoUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EndoUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,Role")] EndoUser endoUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(endoUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(endoUser);
        }

        // GET: EndoUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endoUser = await _context.EndoUsers.FindAsync(id);
            if (endoUser == null)
            {
                return NotFound();
            }
            return View(endoUser);
        }

        // POST: EndoUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,Role")] EndoUser endoUser)
        {
            if (id != endoUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(endoUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EndoUserExists(endoUser.Id))
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
            return View(endoUser);
        }

        // GET: EndoUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endoUser = await _context.EndoUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endoUser == null)
            {
                return NotFound();
            }

            return View(endoUser);
        }

        // POST: EndoUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var endoUser = await _context.EndoUsers.FindAsync(id);
            if (endoUser != null)
            {
                _context.EndoUsers.Remove(endoUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EndoUserExists(int id)
        {
            return _context.EndoUsers.Any(e => e.Id == id);
        }
    }
}
