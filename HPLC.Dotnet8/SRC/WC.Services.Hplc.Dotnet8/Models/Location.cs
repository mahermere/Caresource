// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Location.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Dotnet8.Models
{
    using System.Collections.Generic;

    /// <summary>
    ///    Data describing a CareSource.WC.Services.Hplc.Models.v1.Location object.
    /// </summary>
    public class Location : BaseWorkViewEntity
    {
        public Location()
            => ClassName = Constants.Location.ClassName;

        /// <summary>
        ///    Gets or sets the action type for this instance.
        /// </summary>
        [WorkViewName(Constants.Location.ActionType)]
        public string ActionType { get; set; }

        /// <summary>
        ///    Gets or sets the Location Capacity
        /// </summary>
        [WorkViewName(Constants.Location.Capacity)]
        public int Capacity { get; set; }

        /// <summary>
        ///    Gets or sets the Location City
        /// </summary>
        [WorkViewName(Constants.Location.City)]
        public string City { get; set; }

        /// <summary>
        ///    Gets or sets the Location County
        /// </summary>
        [WorkViewName(Constants.Location.County)]
        public string County { get; set; }

        /// <summary>
        ///    Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        ///    <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
        [WorkViewName(Constants.Location.PrimaryLocation)]
        public bool IsPrimary { get; set; }

        /// <summary>
        ///    Gets or sets the Location Notes
        /// </summary>
        [WorkViewName(Constants.Location.Notes)]
        public string Notes { get; set; }

        /// <summary>
        ///    Gets or sets the Location Postal Code
        /// </summary>
        [WorkViewName(Constants.Location.PostalCode)]
        public string PostalCode { get; set; }

        /// <summary>
        ///    Gets or sets the Location State
        /// </summary>
        [WorkViewName(Constants.Location.State)]
        public string State { get; set; }

        /// <summary>
        ///    Gets or sets the Location Street 1
        /// </summary>
        [WorkViewName(Constants.Location.Street1)]
        public string Street1 { get; set; }

        /// <summary>
        ///    Gets or sets the Location Street 2
        /// </summary>
        [WorkViewName(Constants.Location.Street2)]
        public string Street2 { get; set; }

        /// <summary>
        ///    Gets or sets the Location Type
        /// </summary>
        public IEnumerable<string> Types { get; set; } = new List<string>();

        /// <summary>
        ///    Gets or sets the Location Phones
        /// </summary>
        public IEnumerable<Phone> Phones { get; set; } = new List<Phone>();

        /// <summary>
        ///    Gets or sets the Location Minimum Age
        /// </summary>
        [WorkViewName(Constants.Location.MinAge)]
        public int MinAge { get; set; }

        /// <summary>
        ///    Gets or sets the Location Maximum Age
        /// </summary>
        [WorkViewName(Constants.Location.MaxAge)]
        public int MaxAge { get; set; }

        /// <summary>
        ///    Gets or sets the Location Gender Restrictions
        /// </summary>
        [WorkViewName(Constants.Location.GenderRestrictions)]
        public string GenderRestrictions { get; set; }
    }
}