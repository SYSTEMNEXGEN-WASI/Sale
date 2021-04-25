using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class ProspectDetailVM
    {
        public string ProspectID { get; set;}
        public string DealerCode { get; set; }
        public string ProspectTitle { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string ProspectType { get; set; }
        public string NIC { get; set; }
        public string NTN { get; set; }
        public string ResAddress { get; set; }
        public string ResCityCode { get; set; }
        public string ResPhone { get; set; }
        public string Mobile { get; set; }
        public string OfficeAddress { get; set; }
        public string OffCityCode { get; set; }
        public string OffPhone { get; set; }
        public string Fax { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string WeddingAnniversary { get; set; }
        public string Profession { get; set; }
        public string Designation { get; set; }
        public string Hobbies  { get; set; }
        public string Remarks  { get; set; }
        public string Education  { get; set; }
        public string Income { get; set; }
        public string Email { get; set; }
        public string Createdby { get; set; }
        public bool IsDeleted { get; set; }
        public string CusCode { get; set; }
        public string CompanyDetail { get; set; }
        public string ContactPerson { get; set; }
        public string UpdUser { get; set; }
        public DateTime UpdDate { get; set; }
        public DateTime UpdTime { get; set; }
        public string UpdTerm { get; set; }
    }
}
