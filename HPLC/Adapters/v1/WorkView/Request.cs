// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Request.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System;
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Adapters.v1.WorkView.WorkViewRequest
	///    object.
	/// </summary>
	/// <seealso cref="IRequest" />
	public abstract class Request : IRequest
	{
		/// <summary>
		///    Gets or sets the I Work View Request Correlation Unique identifier
		/// </summary>
		public Guid CorrelationGuid { get; set; } = Guid.NewGuid();

		/// <summary>
		///    Gets or sets the Work View Request Request Date Time
		/// </summary>
		public DateTime RequestDateTime { get; set; } = DateTime.Now;

		/// <summary>
		///    Gets or sets the Work View Request Source Application
		/// </summary>
		[Required]
		public string SourceApplication { get; set; } = "HPLC Service";

		/// <summary>
		///    Gets or sets the Work View Request User Identifier
		/// </summary>
		[Required]
		public string UserId { get; set; }

		/// <summary>
		///    Gets or sets the Work View Request Application Name
		/// </summary>
		[Required]
		public string ApplicationName { get; set; } = "HPLC Management";

		/// <summary>
		///    Gets or sets the Work View Request Class Name
		/// </summary>
		[Required]
		public string ClassName { get; set; }

		/// <summary>
		///    Gets or sets the Work View Request Filter Name
		/// </summary>
		public string FilterName { get; set; }
	}
}