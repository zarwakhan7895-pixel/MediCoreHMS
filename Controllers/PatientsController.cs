using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models; // namespace jahan HospitalDataContext.dbml generate hua hai

namespace MediCoreHMS.Controllers
{
    public class PatientsController : Controller
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();

        // GET: Patients
        public ActionResult Index(string search)
        {
            var patients = db.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                patients = patients.Where(p => p.FullName.Contains(search)
                                             || p.ContactNumber.Contains(search));
            }

            return View(patients.OrderByDescending(p => p.RegisteredOn).ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var patient = db.Patients.SingleOrDefault(p => p.PatientId == id);
            if (patient == null) return HttpNotFound();
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.RegisteredOn = DateTime.Now;
                db.Patients.InsertOnSubmit(patient);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var patient = db.Patients.SingleOrDefault(p => p.PatientId == id);
            if (patient == null) return HttpNotFound();
            return View(patient);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Patients.SingleOrDefault(p => p.PatientId == patient.PatientId);
                if (existing == null) return HttpNotFound();

                existing.FullName = patient.FullName;
                existing.Age = patient.Age;
                existing.Gender = patient.Gender;
                existing.ContactNumber = patient.ContactNumber;
                existing.Address = patient.Address;
                existing.MedicalHistory = patient.MedicalHistory;

                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var patient = db.Patients.SingleOrDefault(p => p.PatientId == id);
            if (patient == null) return HttpNotFound();
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var patient = db.Patients.SingleOrDefault(p => p.PatientId == id);
            if (patient != null)
            {
                db.Patients.DeleteOnSubmit(patient);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
