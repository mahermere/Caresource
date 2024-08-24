namespace WC.Services.CreateGrievanceAppeals.Dotnet8.Models.Data
{
    public class GrievanceAppealsResult
    {
        /// <summary>
        ///    ESB Transaction GUID
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        ///    OnBase Document ID (handle)
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        ///    ESB GUID (Key Transaction GUID)
        /// </summary>
        public string ESBGuid { get; set; }

        /// <summary>
        ///    Valid values: "SUCCESS", "FAILURE"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///    Verbage containing ESBGUID, CallType, and Appeal ID/Dispute ID
        /// </summary>
        public string Message { get; set; }
    }
}
