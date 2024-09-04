// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Phone.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Dotnet8.Models
{
    /// <summary>
    ///    Data describing a CareSource.WC.Services.Hplc.Models.v1.Phone object.
    /// </summary>
    public class Phone : BaseWorkViewEntity
    {
        public Phone()
        {
            ClassName = Constants.Phone.ClassName;
        }

        /// <summary>
        ///    Gets or sets the Phone Number
        /// </summary>

        [WorkViewName(Constants.Phone.Number)]
        public string Number { get; set; }

        /// <summary>
        ///    Gets or sets the Phone Type
        /// </summary>
        [WorkViewName(Constants.Phone.Type)]
        public string Type { get; set; }
    }
}