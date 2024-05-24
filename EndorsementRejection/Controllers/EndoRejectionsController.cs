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
            return View(await _context.EndoRejections.Where(Rejection => Rejection.ApprovalStatus == null).ToListAsync());
        }

        // GET: EndoRejectionsToGenerateLetter
        public async Task<IActionResult> ToGenerateLetter()
        {
            IUserRepository EndoUserRepo = new UserRepository(_context);
            List<EndoUser> EndoUserList = EndoUserRepo.EndoUserList();
            //List<ApprovalUser> ApprovalUserList = EndoUserRepo.ApprovalUserList();
            ViewBag.EndoUserList = EndoUserList;
            //ViewBag.ApprovalUserList = ApprovalUserList;
            return View(await _context.EndoRejections.Where(Rejection => Rejection.completedBy == null && Rejection.ApprovedBy != null).ToListAsync());
        }


        // POST: EndoRejectionsToGenerateLetter
        [HttpPost]
        public async Task<IActionResult> ToGenerateLetter(string Search, string Clear, string User, string AllRecords)
        {
            List<EndoRejection> ListPolicy = new List<EndoRejection>();

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

                        if (AllRecords == "AllRecords")
                        {
                            //return View(await _context.EndoRejections.Where(Rejection => Rejection.completedBy == null && Rejection.ApprovedBy != null).ToListAsync());
                            ListPolicy = await _context.EndoRejections.Where(Rejection => Rejection.RequestedBy == User).OrderByDescending(c => c.Id).ThenBy(n => n.ApprovalStatus).ToListAsync();

                        }
                        else
                        {
                            //List<EndoRejection> ListPolicy = await _context.EndoRejections.Where(EndoRejection => EndoRejection.RequestedBy == User).OrderByDescending(c => c.ApprovedDate).ThenBy(n => n.ApprovalStatus).ToListAsync();
                            ListPolicy = await _context.EndoRejections.Where(Rejection => Rejection.completedBy == null && Rejection.ApprovedBy != null && Rejection.RequestedBy == User).OrderByDescending(c => c.ApprovedDate).ThenBy(n => n.ApprovalStatus).ToListAsync();
                        }
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
            return View(await _context.EndoRejections.OrderByDescending(Req => Req.Id).Take(100).ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,RequestedBy,PolicyNumber,PolicyHolder,ProcessedType,EndoProcessed,RejectionReason,ApprovalStatus,ApprovedBy,RequestedDate,ApprovedDate,ApprovalComments,completedBy,completedDate,RejectionLetterComments,ReferralUrl")] EndoRejection endoRejection)
        {
            string ReturnUrl = Request.Headers["Referer"].ToString();

            ViewBag.ReturnUrl = "Index";
            endoRejection.ReferralUrl = "Index";


            EndoRejection endorejectionViewModel = new EndoRejection();
            IUserRepository EndoUserRepo = new UserRepository(_context);
            List<EndoUser> EndoUserList = EndoUserRepo.EndoUserList();
            List<ApprovalUser> ApprovalUserList = EndoUserRepo.ApprovalUserList();
            ViewBag.EndoUserList = EndoUserList;
            ViewBag.ApprovalUserList = ApprovalUserList;

            if (endoRejection.RequestedBy == "Select")
            {
                endoRejection.RequestedBy = null;
                // ViewBag.RequestedBy = "Requested By field is required";
                ModelState.AddModelError(nameof(EndoRejection.RequestedBy), "Requested By is required");
            }
            if (endoRejection.ProcessedType == "Partial")
            {
                if (endoRejection.EndoProcessed == null || endoRejection.EndoProcessed == "")
                {
                    ModelState.AddModelError(nameof(EndoRejection.EndoProcessed), "Endo processed for partial rejection is required");

                }
            }
            if (endoRejection.completedBy == "Select")
            {
                endoRejection.completedBy = null;

            }

            if (ModelState.IsValid)
            {
                _context.Add(endoRejection);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                TempData["Message"] = "Record saved successfully";
                ViewBag.EndoUserRecordSave = "Success";
                return View(endoRejection);

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
        public async Task<IActionResult> Edit(int? id, string ReferralUrl)
        {
            string ReturnUrl = Request.Headers["Referer"].ToString();

            if (id == null)
            {
                return NotFound();
            }

            var endoRejection = await _context.EndoRejections.FindAsync(id);
            if (endoRejection == null)
            {
                return NotFound();
            }
            if (endoRejection.ApprovedBy == null)
            {
                endoRejection.ApprovedDate = DateTime.Now;
            }
            if (endoRejection.ApprovedBy != null && endoRejection.completedBy == null)
            {
                endoRejection.completedDate = DateTime.Now;
            }
            endoRejection.ReferralUrl = ReferralUrl;
            IUserRepository EndoUserRepo = new UserRepository(_context);
            List<EndoUser> EndoUserList = EndoUserRepo.EndoUserList();
            List<ApprovalUser> ApprovalUserList = EndoUserRepo.ApprovalUserList();
            ViewBag.EndoUserList = EndoUserList;
            ViewBag.ApprovalUserList = ApprovalUserList;
            if (ReturnUrl.Contains("ToBeApproved"))
            {
                ViewBag.ReturnUrl = "ToBeApproved";
                endoRejection.ReferralUrl = "ToBeApproved";
            }
            else if (ReturnUrl.Contains("ToGenerateLetter"))
            {
                ViewBag.ReturnUrl = "ToGenerateLetter";
                endoRejection.ReferralUrl = "ToGenerateLetter";
            }
            else /*if(ReturnUrl.Contains("Index"))*/
            {
                ViewBag.ReturnUrl = "Index";
                endoRejection.ReferralUrl = "Index";
            }


            return View(endoRejection);
        }



        // POST: EndoRejections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RequestedBy,PolicyNumber,PolicyHolder,ProcessedType,EndoProcessed,RejectionReason,ApprovalStatus,ApprovedBy,RequestedDate,ApprovedDate,ApprovalComments,completedBy,completedDate,RejectionLetterComments,ReferralUrl")] EndoRejection endoRejection)
        {
            IUserRepository EndoUserRepo = new UserRepository(_context);
            List<EndoUser> EndoUserList = EndoUserRepo.EndoUserList();
            List<ApprovalUser> ApprovalUserList = EndoUserRepo.ApprovalUserList();
            ViewBag.EndoUserList = EndoUserList;
            ViewBag.ApprovalUserList = ApprovalUserList;




            if (endoRejection.ApprovalStatus == null)
            {
                endoRejection.ApprovedDate = null;
            }
            if (endoRejection.completedBy == null)
            {
                endoRejection.completedDate = null;
            }

            /* 
             Approval Validation
             */





            if (endoRejection.ApprovalStatus != null && endoRejection.ApprovedBy == null)
            {
                //endoRejection.RequestedBy = null;
                // ViewBag.RequestedBy = "Requested By field is required";
                ModelState.AddModelError(nameof(EndoRejection.ApprovedBy), "Approved By is required");
            }

            if (endoRejection.ApprovedBy != null && endoRejection.ApprovalStatus == null)
            {
                ModelState.AddModelError(nameof(EndoRejection.ApprovalStatus), "Approval Status is required");
            }

            if (endoRejection.ApprovedBy != null || endoRejection.ApprovalStatus != null)
            {
                if (endoRejection.ApprovedDate == null)
                {
                    ModelState.AddModelError(nameof(EndoRejection.ApprovedDate), "Approved Date is required");
                }
                if (endoRejection.ApprovalComments == null || endoRejection.ApprovalComments == "")
                {
                    ModelState.AddModelError(nameof(EndoRejection.ApprovalComments), "Approval comments is required");
                }

            }
            ///
            ///Rejection Letter Processed Validation    
            /// 

            if (endoRejection.completedBy != null && endoRejection.RejectionLetterComments == null)
            {
                ModelState.AddModelError(nameof(EndoRejection.RejectionLetterComments), "Rejection Letter Comments is required");
            }
            if (endoRejection.completedBy != null && endoRejection.completedDate == null)
            {
                ModelState.AddModelError(nameof(EndoRejection.completedDate), "Rejection Letter Processed Date is required");
            }

            if (endoRejection.completedBy == null && endoRejection.RejectionLetterComments != null)
            {

                ModelState.AddModelError(nameof(EndoRejection.completedBy), "Rejection Completed By is required");
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
                //return RedirectToAction(nameof(Index));
                TempData["Message"] = "Record updated successfully";
                return View(endoRejection);

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
