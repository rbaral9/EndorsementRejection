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

        // GET: EndoRejectionsToBeApproved
        public async Task<IActionResult> ToBeApproved()
        {
            return View(await _context.EndoRejections.Where(Rejection => Rejection.ApprovalStatus==null).ToListAsync());
        }

        // GET: EndoRejectionsToGenerateLetter
        public async Task<IActionResult> ToGenerateLetter()
        {
            IUserRepository EndoUserRepo = new UserRepository(_context);
            List<EndoUser> EndoUserList = EndoUserRepo.EndoUserList();
            //List<ApprovalUser> ApprovalUserList = EndoUserRepo.ApprovalUserList();
            ViewBag.EndoUserList = EndoUserList;
            //ViewBag.ApprovalUserList = ApprovalUserList;
            return View(await _context.EndoRejections.Where(Rejection => Rejection.completedBy == null && Rejection.ApprovedBy !=null).ToListAsync());
        }


        // POST: EndoRejectionsToGenerateLetter
        [HttpPost]
        public async Task<IActionResult> ToGenerateLetter(string Search, string Clear, string User)
        {
            IUserRepository EndoUserRepo = new UserRepository(_context);
            List<EndoUser> EndoUserList = EndoUserRepo.EndoUserList();
            //List<ApprovalUser> ApprovalUserList = EndoUserRepo.ApprovalUserList();
            ViewBag.EndoUserList = EndoUserList;
            //ViewBag.ApprovalUserList = ApprovalUserList;

            if (!string.IsNullOrEmpty(Clear))
            {
                return RedirectToAction("ToGenerateLetter");
            }


            if (!string.IsNullOrEmpty(Search))
            {
                try
                {



                    if (User != "")
                    {

                        //ViewBag.TableName = formcollection["PolicyNoSearch"];
                        ViewBag.User = User;
                        //HttpCookie RT = new HttpCookie("ResultTableName", formcollection["ResultTableName"]);
                        //HttpContext.Response.AppendCookie(new HttpCookie("PolicyNo", PolicyNo));


                        //List<EndoRejection> ListPolicy = await _context.EndoRejections.Where(EndoRejection => EndoRejection.RequestedBy == User).OrderByDescending(c => c.ApprovedDate).ThenBy(n => n.ApprovalStatus).ToListAsync();
                        List<EndoRejection> ListPolicy = await _context.EndoRejections.Where(Rejection => Rejection.completedBy == null && Rejection.ApprovedBy != null && Rejection.RequestedBy == User).OrderByDescending(c => c.ApprovedDate).ThenBy(n => n.ApprovalStatus).ToListAsync();

                        if (ListPolicy.Count() == 0)
                        {

                            TempData["Message"] = "No Records Found";
                            return View(ListPolicy);
                            //return RedirectToAction("Index");
                            //return RedirectToAction("GetPolicy", new { PolicyNoSearch = PolicyNoSearch });

                        }
                        else
                        {
                            //ViewBag.EnableAddRow = true;

                            return View(ListPolicy);
                        }

                    }

                    //Export(ListVendorName);










                }

                catch (Exception ex)
                {

                    return RedirectToAction("Index");
                }
            }
            
            return View(await _context.EndoRejections.Where(Rejection => Rejection.completedBy == null && Rejection.ApprovedBy != null).ToListAsync());
        }

        // GET: EndoRejections
        public async Task<IActionResult> Index()
        {
            return View(await _context.EndoRejections.ToListAsync());
        }


        // POST: EndoRejections
        [HttpPost]
        public async Task<IActionResult> Index(string Search, string PolicyNo, string Clear)
        {
            if (!string.IsNullOrEmpty(Clear))
            {
                return RedirectToAction("Index");
            }


            if (!string.IsNullOrEmpty(Search))
            {
                try
                {



                    if (PolicyNo != "")
                    {

                        //ViewBag.TableName = formcollection["PolicyNoSearch"];
                        ViewBag.PolicyNo = PolicyNo;
                        //HttpCookie RT = new HttpCookie("ResultTableName", formcollection["ResultTableName"]);
                        //HttpContext.Response.AppendCookie(new HttpCookie("PolicyNo", PolicyNo));


                        List<EndoRejection> ListPolicy = await _context.EndoRejections.Where(EndoRejection => EndoRejection.PolicyNumber == PolicyNo).ToListAsync();


                        if (ListPolicy.Count() == 0)
                        {

                            TempData["Message"] = "No Records Found";
                            return View(ListPolicy);
                            //return RedirectToAction("Index");
                            //return RedirectToAction("GetPolicy", new { PolicyNoSearch = PolicyNoSearch });

                        }
                        else
                        { 
                            //ViewBag.EnableAddRow = true;

                            return View(ListPolicy);
                        }

                    }

                    //Export(ListVendorName);










                }

                catch (Exception ex)
                {

                    return RedirectToAction("Index");
                }
            }

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
        public async Task<IActionResult> Create([Bind("Id,RequestedBy,PolicyNumber,PolicyHolder,ProcessedType,RejectionReason,ApprovalStatus,ApprovedBy,RequestedDate,ApprovedDate,ApprovalComments,completedBy,completedDate,RejectionLetterComments")] EndoRejection endoRejection)
        {
            if(endoRejection.RequestedBy == "Select"  )
            {
                endoRejection.RequestedBy = null;
               // ViewBag.RequestedBy = "Requested By field is required";
                ModelState.AddModelError(nameof(EndoRejection.RequestedBy), "Requested By is required");
            }
            if (endoRejection.ApprovedBy == "Select")
            {
                endoRejection.ApprovedBy = null;
                
            }
            if (endoRejection.completedBy == "Select")
            {
                endoRejection.completedBy = null;
                
            }

            if (ModelState.IsValid)
            {
                _context.Add(endoRejection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            EndoRejection endorejectionViewModel = new EndoRejection();
            IUserRepository EndoUserRepo = new UserRepository(_context);
            List<EndoUser> EndoUserList = EndoUserRepo.EndoUserList();
            List<ApprovalUser> ApprovalUserList = EndoUserRepo.ApprovalUserList();
            ViewBag.EndoUserList = EndoUserList;
            ViewBag.ApprovalUserList = ApprovalUserList;
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,RequestedBy,PolicyNumber,PolicyHolder,ProcessedType,RejectionReason,ApprovalStatus,ApprovedBy,RequestedDate,ApprovedDate,ApprovalComments,completedBy,completedDate,RejectionLetterComments")] EndoRejection endoRejection)
        {
            if (endoRejection.ApprovedBy == "Select")
            {
                endoRejection.ApprovedBy = null;

            }

            if (endoRejection.completedBy == "Select")
            {
                endoRejection.completedBy = null;

            }

            if (endoRejection.ApprovalStatus != null && endoRejection.ApprovedBy==null)
            {
                //endoRejection.RequestedBy = null;
               // ViewBag.RequestedBy = "Requested By field is required";
                ModelState.AddModelError(nameof(EndoRejection.ApprovedBy), "Approved By is required");
            }

            if (endoRejection.ApprovedDate != null && ( endoRejection.ApprovedBy == null || endoRejection.ApprovalStatus ==null))
            {
                //endoRejection.RequestedBy = null;
                // ViewBag.RequestedBy = "Requested By field is required";
                if(endoRejection.ApprovalStatus == null)
                {
                    ModelState.AddModelError(nameof(EndoRejection.ApprovalStatus), "Approval Status is required");
                }
                if (endoRejection.ApprovedBy == null)
                {
                    ModelState.AddModelError(nameof(EndoRejection.ApprovedBy), "Approved By is required");
                }   
                
            }

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
            IUserRepository EndoUserRepo = new UserRepository(_context);
            List<EndoUser> EndoUserList = EndoUserRepo.EndoUserList();
            List<ApprovalUser> ApprovalUserList = EndoUserRepo.ApprovalUserList();
            ViewBag.EndoUserList = EndoUserList;
            ViewBag.ApprovalUserList = ApprovalUserList;

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
