// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Request.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Dotnet8.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///    Data and functions describing a CareSource.WC.Services.Hplc.Models.v2.Request object.
    /// </summary>
    public class Request : BaseWorkViewEntity
    {
        private string _source;
        private string _type;

        public Request()
        {
            ClassName = Constants.Request.ClassName;
        }

        /// <summary>
        ///    Gets or sets the Request CareSource Received Date
        /// </summary>
        [WorkViewName(Constants.Request.CareSourceReceivedDate)]
        public DateTime? CareSourceReceivedDate { get; set; } = DateTime.Now;

        /// <summary>
        ///    Gets or sets the Request Change Effective Date
        /// </summary>
        [WorkViewName(Constants.Request.ChangeEffectiveDate)]
        public DateTime? ChangeEffectiveDate { get; set; }

        /// <summary>
        ///    Gets or sets the Request Entity Contact Email
        /// </summary>
        [EmailAddress]
        [WorkViewName(Constants.Request.ContactEmail)]
        public string ContactEmail { get; set; }

        /// <summary>
        ///    Gets or sets the Request Contact Name
        /// </summary>
        [WorkViewName(Constants.Request.ContactName)]
        public string ContactName { get; set; }

        /// <summary>
        ///    Gets or sets the Request Contact Phone
        /// </summary>
        [WorkViewName(Constants.Request.ContactPhone)]
        public string ContactPhone { get; set; }

        /// <summary>
        ///    Gets or sets the Request Date
        /// </summary>
        [WorkViewName(Constants.Request.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>
        ///    Gets or sets the Request Entity Name
        /// </summary>
        [WorkViewName(Constants.Request.EntityName)]
        public string EntityName { get; set; }

        /// <summary>
        ///    Gets or sets the Request Entity Tin
        /// </summary>

        public string EntityTin { get; set; }

        /// <summary>
        ///    Gets or sets the Request Identifier
        /// </summary>
        public new long Id { get; set; }

        /// <summary>
        ///    Gets or sets the Request Notes
        /// </summary>
        [WorkViewName(Constants.Request.Notes)]
        public string Notes { get; set; }

        /// <summary>
        ///    Gets or sets the Request Primary State
        /// </summary>
        [WorkViewName(Constants.Request.PrimaryState)]
        public string PrimaryState { get; set; }

        /// <summary>
        ///    Gets or sets the Request Lines Of Business
        /// </summary>
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        ///    Gets or sets the Request Providers
        /// </summary>
        public IEnumerable<Provider> Providers { get; set; } = new List<Provider>();

        /// <summary>
        /// Gets or sets the Request Status
        /// </summary>
        [WorkViewName(Constants.Request.Status)]
        public string Status { get; internal set; }

        /// <summary>
        ///    Gets or sets the Request Signatory Email
        /// </summary>
        [Required]
        [EmailAddress]
        [WorkViewName(Constants.Request.SignatoryEmail)]
        public string SignatoryEmail { get; set; }

        /// <summary>
        ///    Gets or sets the Request Signatory Name
        /// </summary>
        [Required]
        [WorkViewName(Constants.Request.SignatoryName)]
        public string SignatoryName { get; set; }

        /// <summary>
        ///    Gets or sets the Request Signatory Title
        /// </summary>
        [Required]
        [WorkViewName(Constants.Request.SignatoryTitle)]
        public string SignatoryTitle { get; set; }

        /// <summary>
        /// The type of the health partner agreement.
        /// </summary>
        [WorkViewName(Constants.Request.HealthPartnerAgreementType)]
        public string HealthPartnerAgreementType { get; set; }

        /// <summary>
        ///    Gets or sets the Request Source
        /// </summary>
        [WorkViewName(Constants.Request.Source)]
        public string Source
        {
            get => _source?.ToUpper();
            set => _source = value?.ToUpper();
        }

        [WorkViewName(Constants.Request.JsonData)]
        public string JsonData { get; set; }

        /// <summary>
        ///    Gets or sets the Request Type
        /// </summary>
        [Required]
        [WorkViewName(Constants.Request.Type)]
        public string Type
        {
            get => _type?.ToUpper();
            set => _type = value?.ToUpper();
        }

        /// <summary>
        /// The type of the health partner agreement.
        /// </summary>
        [WorkViewName(Constants.Request.InternalViewLink)]
        public string InternalViewLink { get; set; }

        /// <summary>
        /// The type of the health partner agreement.
        /// </summary>
        [WorkViewName(Constants.Request.FinalDeleteLink)]
        public string FinalDeleteLink { get; set; }
    }
}