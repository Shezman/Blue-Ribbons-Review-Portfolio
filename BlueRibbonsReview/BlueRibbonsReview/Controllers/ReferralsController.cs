using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlueRibbonsReview.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;

namespace BlueRibbonsReview.Controllers
{
    public class ReferralsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Referrals
        public ActionResult Index(string sortOrder)
        {
            ViewBag.ReferralNameSortParm = String.IsNullOrEmpty(sortOrder) ? "referralName_desc" : "";
            ViewBag.ReferralEmailSortParm = sortOrder == "referralEmail"? "referralEmail_desc" :"referralEmail";
            ViewBag.UserNameSortParm = sortOrder == "userName" ? "userName_desc" : "userName";
            ViewBag.UserEmailSortParm = sortOrder == "userEmail" ? "userEmail_desc" : "userEmail";

            var referrals = db.Referrals.Include(r => r.ApplicationUser);

            switch (sortOrder)
            {
                case "referralName_desc":
                    referrals = referrals.OrderByDescending(s => s.ReferralName);
                    break;
                case "referralEmail":
                    referrals = referrals.OrderBy(s => s.ReferralEmail);
                    break;
                case "referralEmail_desc":
                    referrals = referrals.OrderByDescending(s => s.ReferralEmail);
                    break;
                case "userName":
                    referrals = referrals.OrderBy(s => s.ApplicationUser.UserName);
                    break;
                case "userName_desc":
                    referrals = referrals.OrderByDescending(s => s.ApplicationUser.UserName);
                    break;
                case "userEmail":
                    referrals = referrals.OrderBy(s => s.ApplicationUser.Email);
                    break;
                case "userEmail_desc":
                    referrals = referrals.OrderByDescending(s => s.ApplicationUser.Email);
                    break;
                default:
                    referrals = referrals.OrderBy(s => s.ReferralName);
                    break;
            }
            return View(referrals.ToList());           
        }

        public ActionResult GetPartialReferrals()
        {

            var rUsers = from u in db.Users
                            from reff in db.Referrals
                            where u.Email == reff.ReferralEmail
                            select reff;

            return PartialView("_ReferralsIndex", rUsers.ToList());
        }

        // GET: Referrals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referral referral = db.Referrals.Find(id);
            if (referral == null)
            {
                return HttpNotFound();
            }
            return View(referral);
        }

        // GET: Referrals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Referrals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReferralID,ReferralName,ReferralEmail,Message")] Referral referral)
        {
            if (string.IsNullOrEmpty(referral.ReferralName)) // This if statement checks if the referral name has text in it
            {
                ModelState.AddModelError("ReferralName", "Please enter a name.");
            }
            try // TryCatch block to check if the user entered in valid email format (i.e. fakemail@mail.com)
            {
                var check = new System.Net.Mail.MailAddress(referral.ReferralEmail);
                if (check.Address != referral.ReferralEmail)
                {
                    ModelState.AddModelError("ReferralEmail", "Please enter a valid email.");
                }
            }
            catch
            {
                ModelState.AddModelError("ReferralEmail", "Please enter a valid email.");
            }
            if (string.IsNullOrEmpty(referral.Message))
            {
                ModelState.AddModelError("Message", "Please enter a message.");
            }
            if (ModelState.IsValid)
            {
                // Application User ID Change
                string applicationUserId = User.Identity.GetUserId();
                referral.ApplicationUser = db.Users.Single(u => u.Id == applicationUserId);
                db.Referrals.Add(referral);
                db.SaveChanges();
 
            }
            return View(referral);
        }

        public ActionResult Sent()
        {
            return View();
        }

        // GET: Referrals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referral referral = db.Referrals.Find(id);
            if (referral == null)
            {
                return HttpNotFound();
            }
            return View(referral);
        }

        // POST: Referrals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReferralID,ReferralName,ReferralEmail,Message")] Referral referral)
        {
            if (ModelState.IsValid)
            {
                db.Entry(referral).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(referral);
        }

        // GET: Referrals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referral referral = db.Referrals.Find(id);
            if (referral == null)
            {
                return HttpNotFound();
            }
            return View(referral);
        }

        // POST: Referrals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Referral referral = db.Referrals.Find(id);
            db.Referrals.Remove(referral);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //Export Email List to .csv
        public void ExportEmailListToCSV()
        {
            StringWriter sW = new StringWriter();
            sW.WriteLine("\"ReferralEmail\"");
            //sW.WriteLine("\"First Name\",\"Last Name\",\"Email\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Exported_ReferralEmails.csv");
            Response.ContentType = "text/csv";

            var emails = db.Referrals.ToList();


            foreach (var item in emails)
            {
                sW.WriteLine(string.Format("\"{0}\"",
                                            item.ReferralEmail));
            }

            Response.Write(sW.ToString());

            Response.End();

        }       
        
        public string ConvertedReferrals()
        {
            int allReferrals = db.Referrals.Count();
            var rUsers = from u in db.Users
                            from reff in db.Referrals
                            where u.Email == reff.ReferralEmail
                            select reff;
            var referredUsers = rUsers.Count();
            decimal convertedReferrals = (Decimal)referredUsers / (Decimal)allReferrals;
            string convertedString = convertedReferrals.ToString("#0.##%");
            return convertedString;
        }
    }
}
