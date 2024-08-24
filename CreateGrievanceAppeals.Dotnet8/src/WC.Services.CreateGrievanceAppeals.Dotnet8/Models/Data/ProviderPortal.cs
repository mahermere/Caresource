namespace WC.Services.CreateGrievanceAppeals.Dotnet8.Models.Data
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class PPAppealJson
    {
        [DataMember]
        public string DocumentTypeName { get; set; }

        [DataMember]
        public string DocumentFileName { get; set; }

        [DataMember(Name = "Appeal ID")]
        public string AppealID { get; set; }

        [DataMember(Name = "Begin DOS")]
        public string BeginDOS { get; set; }

        [DataMember(Name = "End DOS")]
        public string EndDOS { get; set; }

        [DataMember(Name = "Enrollee Name")]
        public string EnrolleeName { get; set; }

        [DataMember]
        public string HasPriorAuth { get; set; }

        [DataMember(Name = "Health Plan")]
        public string HealthPlan { get; set; }

        [DataMember]
        public string HICN { get; set; }

        [DataMember]
        public string IsRetroAuth { get; set; }

        [DataMember(Name = "Plan ID")]
        public string PlanID { get; set; }

        [DataMember(Name = "ESB GUID")]
        public string ESBGUID { get; set; }

        [DataMember(Name = "Signature Date")]
        public string SignatureDate { get; set; }

        [DataMember]
        public string Signature { get; set; }

        [DataMember]
        public string PageData { get; set; }
    }

    public class PPDisputeJson
    {
        public string DocumentTypeName { get; set; }

        public List<KeywordItems> Keywords { get; set; }

        //public string DisputeID { get; set; }
        //public string ClaimNumber { get; set; }
        //public string Description { get; set; }
        //public string IssueType { get; set; }
        //public string PlanID { get; set; }
        //public string ESBGUID { get; set; }
        public List<string> PageData { get; set; }
    }

    public class KeywordItems
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
