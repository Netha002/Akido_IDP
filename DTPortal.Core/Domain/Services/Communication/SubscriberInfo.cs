using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTPortal.Core.Domain.Services.Communication
{
    public class SubscriberInfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Suid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Birthdate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Gender { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? IdDocumentType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IdDocumentNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Loa { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FullNameEn { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentNationality { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GenderEn { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OccupationEn { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmiratesIdNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? IssueDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpiryDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PassportType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PassportNo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentNationalityAbbr { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentNationality { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PassportIssueDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PassportExpiryDate { get; set; }
    }
}
