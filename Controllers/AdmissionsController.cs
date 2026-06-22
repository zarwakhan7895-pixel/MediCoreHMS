using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;

namespace MediCoreHMS.Controllers
{
    public class AdmissionsController : Controller
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();
        
        // GET: Admissions
        public ActionResult Index()
        {
            var admissions = db.Admissions
                .OrderByDescending(a => a.AdmissionDate)
                .ToList();

            return View(admissions);
        }

        // GET: Admissions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var admission = db.Admissions.SingleOrDefault(a => a.AdmissionId == id);
            if (admission == null) return HttpNotFound();
            return View(admission);
        }

        // GET: Admissions/Create
        public ActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Admissions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admission admission)
        {
            // VALIDATION 1: bed must exist and not already be occupied
            var bed = db.Beds.SingleOrDefault(b => b.BedId == admission.BedId);
            if (bed == null)
            {
                ModelState.AddModelError("", "Selected bed does not exist.");
            }
            else if (bed.IsOccupied)
            {
                ModelState.AddModelError("", "This bed is already occupied. Please choose another bed.");
            }

            // VALIDATION 2: prevent duplicate active admission for same patient
            bool alreadyAdmitted = db.Admissions.Any(a => a.PatientId == admission.PatientId && !a.IsDischarged);
            if (alreadyAdmitted)
            {
                ModelState.AddModelError("", "This patient already has an active admission.");
            }

            if (ModelState.IsValid)
            {
                admission.AdmissionDate = DateTime.Now;
                admission.IsDischarged = false;
                bed.IsOccupied = true;

                db.Admissions.InsertOnSubmit(admission);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            PopulateDropdowns();
            return View(admission);
        }

        // GET: Admissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var admission = db.Admissions.SingleOrDefault(a => a.AdmissionId == id);
            if (admission == null) return HttpNotFound();
            PopulateDropdowns();
            return View(admission);
        }

        // POST: Admissions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admission admission)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Admissions.SingleOrDefault(a => a.AdmissionId == admission.AdmissionId);
                if (existing == null) return HttpNotFound();

                existing.PatientId = admission.PatientId;
                existing.DoctorId = admission.DoctorId;
                existing.BedId = admission.BedId;
                existing.Ward = admission.Ward;
                existing.EntryType = admission.EntryType;
                existing.TreatmentSummary = admission.TreatmentSummary;

                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            PopulateDropdowns();
            return View(admission);
        }

        // GET: Admissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var admission = db.Admissions.SingleOrDefault(a => a.AdmissionId == id);
            if (admission == null) return HttpNotFound();
            return View(admission);
        }

        // POST: Admissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var admission = db.Admissions.SingleOrDefault(a => a.AdmissionId == id);
            if (admission != null)
            {
                if (!admission.IsDischarged)
                {
                    var bed = db.Beds.SingleOrDefault(b => b.BedId == admission.BedId);
                    if (bed != null) bed.IsOccupied = false;
                }
                db.Admissions.DeleteOnSubmit(admission);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Admissions/Discharge/5
        public ActionResult Discharge(int id)
        {
            var admission = db.Admissions.SingleOrDefault(a => a.AdmissionId == id);
            if (admission == null) return HttpNotFound();
            return View(admission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DischargeConfirmed(int id, string treatmentSummary)
        {
            var admission = db.Admissions.SingleOrDefault(a => a.AdmissionId == id);
            if (admission == null) return HttpNotFound();

            admission.IsDischarged = true;
            admission.DischargeDate = DateTime.Now;
            admission.TreatmentSummary = treatmentSummary;

            var bed = db.Beds.SingleOrDefault(b => b.BedId == admission.BedId);
            if (bed != null) bed.IsOccupied = false;

            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        private void PopulateDropdowns()
        {
            var availablePatients = db.Patients.Where(p =>
                !db.Admissions.Any(a => a.PatientId == p.PatientId && !a.IsDischarged)).ToList();
            ViewBag.PatientId = new SelectList(availablePatients, "PatientId", "FullName");

            var availableDoctors = db.Doctors.Where(d => d.IsAvailable).ToList();
            ViewBag.DoctorId = new SelectList(availableDoctors, "DoctorId", "FullName");

            var freeBeds = db.Beds.Where(b => !b.IsOccupied).ToList();
            ViewBag.BedId = new SelectList(freeBeds, "BedId", "Ward");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
