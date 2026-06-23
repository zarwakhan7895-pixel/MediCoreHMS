using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;
using MediCoreHMS.ViewModels;

namespace MediCoreHMS.Controllers
{
    // GROUP CONFIGURATION NUMBER (GCN) = 1
    // Required dashboard widgets: Doctors currently on duty +
    // Admissions made during the last 24 hours
    public class DashboardController : Controller
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();

        public ActionResult Index()
        {
            // ----- Doctors currently on duty -----
            var doctorsOnDuty = db.Doctors
                .Where(d => d.IsAvailable)
                .Select(d => new DoctorOnDutyVM
                {
                    DoctorName = d.FullName,
                    Specialization = d.Specialization,
                    Ward = d.Ward,
                    ConsultationSchedule = d.ConsultationSchedule
                })
                .ToList();

            // ----- Admissions made during the last 24 hours -----
            DateTime cutoff = DateTime.Now.AddHours(-24);

            var recentAdmissions = db.Admissions
                .Where(a => a.AdmissionDate >= cutoff)
                .OrderByDescending(a => a.AdmissionDate)
                .ToList()
                .Select(a => new RecentAdmissionVM
                {
                    PatientName = a.Patient.FullName,
                    DoctorName = a.Doctor.FullName,
                    Ward = a.Ward,
                    EntryType = a.EntryType,
                    AdmissionDate = a.AdmissionDate
                })
                .ToList();

            var model = new DashboardVM
            {
                DoctorsOnDuty = doctorsOnDuty,
                RecentAdmissions = recentAdmissions,
                TotalPatients = db.Patients.Count(),
                TotalBeds = db.Beds.Count(),
                OccupiedBeds = db.Beds.Count(b => b.IsOccupied)
            };

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
