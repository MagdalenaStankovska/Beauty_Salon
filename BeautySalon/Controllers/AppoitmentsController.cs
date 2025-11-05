using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BeautySalon.Models;

namespace BeautySalon.Controllers
{
    public class AppoitmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Appoitments
        public ActionResult Index(string sortOrder, string searchString, string statusFilter, string typeFilter)
        {
            var appoitments = db.Appoitments.Include(a => a.tipche).AsQueryable();

            // Check if logged-in user is admin
            string currentUserEmail = User.Identity.Name;
            ViewBag.IsAdmin = currentUserEmail.Equals("beauty.salon@yahoo.com", StringComparison.OrdinalIgnoreCase);

            // Only apply filters if admin
            if (ViewBag.IsAdmin)
            {
                if (!string.IsNullOrEmpty(searchString))
                    appoitments = appoitments.Where(a => a.ClientName.Contains(searchString));

                if (!string.IsNullOrEmpty(statusFilter))
                    appoitments = appoitments.Where(a => a.Status == statusFilter);

                if (!string.IsNullOrEmpty(typeFilter))
                    appoitments = appoitments.Where(a => a.tipche.tipche == typeFilter);
            }

            // Sorting parameters
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            // Sorting logic
            switch (sortOrder)
            {
                case "type_desc":
                    appoitments = appoitments.OrderByDescending(a => a.tipche.tipche);
                    break;
                case "Type":
                    appoitments = appoitments.OrderBy(a => a.tipche.tipche);
                    break;
                case "Date":
                    appoitments = appoitments.OrderBy(a => a.date);
                    break;
                case "date_desc":
                    appoitments = appoitments.OrderByDescending(a => a.date);
                    break;
                default:
                    appoitments = appoitments.OrderBy(a => a.tipche.tipche);
                    break;
            }

            // Pass filters to view
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentStatus = statusFilter;
            ViewBag.CurrentType = typeFilter;

            // Populate type list for dropdown
            ViewBag.TypeList = db.Types.Select(t => t.tipche).ToList();

            return View(appoitments.ToList());
        }

        // GET: Appoitments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var appoitment = db.Appoitments.Find(id);
            if (appoitment == null) return HttpNotFound();

            return View(appoitment);
        }

        // GET: Appoitments/Create
        public ActionResult Create()
        {
            string currentUserEmail = User.Identity.Name;
            bool isAdmin = currentUserEmail.Equals("beauty.salon@yahoo.com", StringComparison.OrdinalIgnoreCase);

            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "tipche");

            if (isAdmin)
            {
       
                ViewBag.ClientList = new SelectList(db.Clients, "ClientName", "ClientName");
            }
            else
            {
             
                var client = db.Clients.FirstOrDefault(c => c.Email == currentUserEmail);
                ViewBag.ClientList = new SelectList(new[] { client }, "ClientName", "ClientName");
            }

            return View();
        }

        // POST: Appoitments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppoitmentID,date,TypeID,ClientName")] Appoitments appoitment)
        {
            string currentUserEmail = User.Identity.Name;
            bool isAdmin = currentUserEmail.Equals("beauty.salon@yahoo.com", StringComparison.OrdinalIgnoreCase);

            if (!isAdmin)
            {
              
                var client = db.Clients.FirstOrDefault(c => c.Email == currentUserEmail);
                if (client != null)
                {
                    appoitment.ClientName = client.ClientName;
                }
            }

            if (ModelState.IsValid)
            {
                appoitment.Status = "Pending";
                db.Appoitments.Add(appoitment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "tipche", appoitment.TypeID);
            if (isAdmin)
                ViewBag.ClientList = new SelectList(db.Clients, "ClientName", "ClientName", appoitment.ClientName);
            else
                ViewBag.ClientList = new SelectList(new[] { db.Clients.FirstOrDefault(c => c.Email == currentUserEmail) }, "ClientName", "ClientName", appoitment.ClientName);

            return View(appoitment);
        }

        // GET: Appoitments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(User.Identity.Name.Equals("beauty.salon@yahoo.com", StringComparison.OrdinalIgnoreCase)))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var appoitment = db.Appoitments.Find(id);
            if (appoitment == null) return HttpNotFound();

            ViewBag.ClientList = new SelectList(db.Clients, "ClientName", "ClientName", appoitment.ClientName);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "tipche", appoitment.TypeID);
            return View(appoitment);
        }

        // POST: Appoitments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "date,TypeID,ClientName,Status")] Appoitments editedAppoitment)
        {
            if (!(User.Identity.Name.Equals("beauty.salon@yahoo.com", StringComparison.OrdinalIgnoreCase)))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (ModelState.IsValid)
            {
                var appoitment = db.Appoitments.Find(id);
                if (appoitment == null) return HttpNotFound();

                appoitment.date = editedAppoitment.date;
                appoitment.TypeID = editedAppoitment.TypeID;
                appoitment.ClientName = editedAppoitment.ClientName;
                appoitment.Status = editedAppoitment.Status;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientList = new SelectList(db.Clients, "ClientName", "ClientName", editedAppoitment.ClientName);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "tipche", editedAppoitment.TypeID);
            return View(editedAppoitment);
        }

        // GET: Appoitments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(User.Identity.Name.Equals("beauty.salon@yahoo.com", StringComparison.OrdinalIgnoreCase)))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var appoitment = db.Appoitments.Find(id);
            if (appoitment == null) return HttpNotFound();

            return View(appoitment);
        }

        // POST: Appoitments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!(User.Identity.Name.Equals("beauty.salon@yahoo.com", StringComparison.OrdinalIgnoreCase)))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var appoitment = db.Appoitments.Find(id);
            db.Appoitments.Remove(appoitment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
