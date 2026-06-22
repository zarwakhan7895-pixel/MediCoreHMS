using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;

namespace MediCoreHMS.Controllers
{
    public class DoctorsController : Controller
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();

        // GET: Doctors
        public ActionResult Index(string ward)
        {
            var doctors = db.Doctors.AsQueryable();

            if (!string.IsNullOrEmpty(ward))
            {
                doctors = doctors.Where(d => d.Ward == ward);
            }

            ViewBag.Wards = new[] { "General", "ICU", "Maternity", "Pediatric" };
            return View(doctors.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var doctor = db.Doctors.SingleOrDefault(d => d.DoctorId == id);
            if (doctor == null) return HttpNotFound();
            return View(doctor);
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Doctors.InsertOnSubmit(doctor);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var doctor = db.Doctors.SingleOrDefault(d => d.DoctorId == id);
            if (doctor == null) return HttpNotFound();
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Doctors.SingleOrDefault(d => d.DoctorId == doctor.DoctorId);
                if (existing == null) return HttpNotFound();

                existing.FullName = doctor.FullName;
                existing.Specialization = doctor.Specialization;
                existing.Ward = doctor.Ward;
                existing.IsAvailable = doctor.IsAvailable;
                existing.ConsultationSchedule = doctor.ConsultationSchedule;

                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var doctor = db.Doctors.SingleOrDefault(d => d.DoctorId == id);
            if (doctor == null) return HttpNotFound();
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var doctor = db.Doctors.SingleOrDefault(d => d.DoctorId == id);
            if (doctor != null)
            {
                db.Doctors.DeleteOnSubmit(doctor);
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
