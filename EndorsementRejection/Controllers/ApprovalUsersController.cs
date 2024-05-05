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
    public class ApprovalUsersController : Controller
    {
        private readonly AppDbContext _context;

        public ApprovalUsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ApprovalUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApprovalUsers.ToListAsync());
        }

        // GET: ApprovalUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalUser = await _context.ApprovalUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approvalUser == null)
            {
                return NotFound();
            }

            return View(approvalUser);
        }

        // GET: ApprovalUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApprovalUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,Role")] ApprovalUser approvalUser)
        {

            if (ModelState.IsValid)
            {
                _context.Add(approvalUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(approvalUser);
        }

        // GET: ApprovalUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalUser = await _context.ApprovalUsers.FindAsync(id);
            if (approvalUser == null)
            {
                return NotFound();
            }
            return View(approvalUser);
        }

        // POST: ApprovalUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,Role")] ApprovalUser approvalUser)
        {
            if (id != approvalUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approvalUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovalUserExists(approvalUser.Id))
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
            return View(approvalUser);
        }

        // GET: ApprovalUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalUser = await _context.ApprovalUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approvalUser == null)
            {
                return NotFound();
            }

            return View(approvalUser);
        }

        // POST: ApprovalUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var approvalUser = await _context.ApprovalUsers.FindAsync(id);
            if (approvalUser != null)
            {
                _context.ApprovalUsers.Remove(approvalUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApprovalUserExists(int id)
        {
            return _context.ApprovalUsers.Any(e => e.Id == id);
        }
    }
}
