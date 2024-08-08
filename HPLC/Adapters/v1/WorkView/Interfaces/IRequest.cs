// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   IRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using CareSource.WC.Entities.Requests.Base.Interfaces;

	/// <summary>
	/// </summary>
	/// <seealso cref="CareSource.WC.Entities.Requests.Base.Interfaces.IBaseRequest" />
	public interface IRequest : IBaseRequest
	{
		/// <summary>
		///    Gets or sets the Work View Request Application Name
		/// </summary>
		[Required]
		string ApplicationName { get; set; }

		/// <summary>
		///    Gets or sets the Work View Request Class Name
		/// </summary>
		[Required]
		string ClassName { get; set; }

		/// <summary>
		///    Gets or sets the I Work View Request Correlation Unique identifier
		/// </summary>
		Guid CorrelationGuid { get; set; }

		/// <summary>
		///    Gets or sets the Work View Request Filter Name
		/// </summary>
		string FilterName { get; set; }
	}
}