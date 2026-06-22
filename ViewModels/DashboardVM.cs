using System.Collections.Generic;

namespace MediCoreHMS.ViewModels
{
    public class DoctorOnDutyVM
    {
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string Ward { get; set; }
        public string ConsultationSchedule { get; set; }
    }

    public class RecentAdmissionVM
    {
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Ward { get; set; }
        public string EntryType { get; set; }
        public System.DateTime AdmissionDate { get; set; }
    }

    public class DashboardVM
    {
        public List<DoctorOnDutyVM> DoctorsOnDuty { get; set; }
        public List<RecentAdmissionVM> RecentAdmissions { get; set; }
        public int TotalPatients { get; set; }
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
    }
}
