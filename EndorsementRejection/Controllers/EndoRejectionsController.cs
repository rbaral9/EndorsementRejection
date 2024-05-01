using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EndorsementRejection.Data;
using EndorsementRejection.Models.Entities;
using EndorsementRejection.Infrastructure.Abstract;
using EndorsementRejection.Infrastructure.Repository;

namespace EndorsementRejection.Controllers
{
    public class EndoRejectionsController : Controller
    {
        private readonly AppDbContext _context;

        public EndoRejectionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EndoRejections
        public async Task<IActionResult> Index()
        {
            return View(await _context.EndoRejections.ToListAsync());
        }

        // GET: EndoRejections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endoRejection = await _context.EndoRejections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endoRejection == null)
            {
                return NotFound();
            }

            return View(endoRejection);
        }

        // GET: EndoRejections/Create
        public IActionResult Create()
        {
            EndoRejection endorejectionViewModel = new EndoRejection();
            IUserRepository EndoUserRepo = new UserRepository(_context);
            List<EndoUser> EndoUserList = EndoUserRepo.EndoUserList();
            List<ApprovalUser> ApprovalUserList = EndoUserRepo.ApprovalUserList();
            ViewBag.EndoUserList = EndoUserList;
            ViewBag.ApprovalUserList = ApprovalUserList;
            return View(endorejectionViewModel);
        }

        // POST: EndoRejections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RequestedBy,PolicyNumber,PolicyHolder,ProcessedType,RejectionReason,ApprovalStatus,ApprovedBy,RequestedDate,ApprovedDate,ApprovalComments,completedBy,completedDate")] EndoRejection endoRejection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(endoRejection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(endoRejection);
        }


        // GET: EndoRejections/Approval/5
        public async Task<IActionResult> Approval(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endoRejection = await _context.EndoRejections.FindAsync(id);
            
            if (endoRejection == null)
            {
                return NotFound();
            }
            else
            {
                endoRejection.ApprovedDate = DateTime.Now;
            }
            return View(endoRejection);
        }


        // GET: EndoRejections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endoRejection = await _context.EndoRejections.FindAsync(id);
            if (endoRejection == null)
            {
                return NotFound();
            }

            IUserRepository EndoUserRepo = new UserRepository(_context);
            List<EndoUser> EndoUserList = EndoUserRepo.EndoUserList();
            List<ApprovalUser> ApprovalUserList = EndoUserRepo.ApprovalUserList();
            ViewBag.EndoUserList = EndoUserList;
            ViewBag.ApprovalUserList = ApprovalUserList;

            return View(endoRejection);
        }



        // POST: EndoRejections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RequestedBy,PolicyNumber,PolicyHolder,ProcessedType,RejectionReason,ApprovalStatus,ApprovedBy,RequestedDate,ApprovedDate,ApprovalComments,completedBy,completedDate")] EndoRejection endoRejection)
        {
            if (id != endoRejection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(endoRejection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EndoRejectionExists(endoRejection.Id))
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
            return View(endoRejection);
        }

        // GET: EndoRejections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endoRejection = await _context.EndoRejections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endoRejection == null)
            {
                return NotFound();
            }

            return View(endoRejection);
        }

        // POST: EndoRejections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var endoRejection = await _context.EndoRejections.FindAsync(id);
            if (endoRejection != null)
            {
                _context.EndoRejections.Remove(endoRejection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EndoRejectionExists(int id)
        {
            return _context.EndoRejections.Any(e => e.Id == id);
        }
    }
}
