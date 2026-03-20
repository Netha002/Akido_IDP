using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPortal.Core.DTOs
{
    public class SubscriberProfileDTO
    {
        public string Suid { get; set; }
        public DateTime Birthdate { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int IdDocumentType { get; set; }
        public string IdDocumentNumber { get; set; }
        public string Loa { get; set; }
        public string Country { get; set; }
        public string FullNameEn { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CurrentNationality { get; set; }
        public string GenderEn { get; set; }
        public string OccupationEn { get; set; }
        public string EmiratesIdNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string PassportType { get; set; }
        public string PassportNo { get; set; }
        public string DocumentNationalityAbbr { get; set; }
        public string DocumentNationality { get; set; }
        public DateTime PassportIssueDate { get; set; }
        public DateTime PassportExpiryDate { get; set; }
    }
}
