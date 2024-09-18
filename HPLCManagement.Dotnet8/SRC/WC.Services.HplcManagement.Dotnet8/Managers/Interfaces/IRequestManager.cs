// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   IRequestManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Managers.Interfaces
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Collections.Generic;
    using WC.Services.HplcManagement.Dotnet8.Models;

    /// <summary>
    /// 
    /// </summary>
    public interface IRequestManager
    {
        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Data GetRequest(long id);

        /// <summary>
        /// Updates the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        long UpdateRequest(HplcServiceRequest request);
        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        long CreateRequest(HplcServiceRequest request);

        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="modelState">State of the model.</param>
        /// <returns></returns>
        bool ValidateRequest(
            HplcServiceRequest request,
            ModelStateDictionary modelState);

        /// <summary>
        /// Searches the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEnumerable<Data> Search(SearchRequest request);

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <param name="applicationNumber">The application number.</param>
        /// <param name="providerNpi">The provider tin.</param>
        /// <returns></returns>
        StatusResponse SearchByNpi(
            string applicationNumber,
            string providerNpi);
    }
}