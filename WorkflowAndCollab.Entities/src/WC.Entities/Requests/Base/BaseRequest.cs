// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Entities
//   BaseRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Requests.Base
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using CareSource.WC.Entities.Requests.Base.Interfaces;

	public abstract class BaseRequest : IBaseRequest
	{
		public Guid CorrelationGuid { get; set; } = Guid.NewGuid();

		/// <summary>
		///    Gets or sets the request date time of the base request{ t request} class.
		/// </summary>
		public DateTime RequestDateTime { get; set; } = DateTime.UtcNow;

		/// <summary>
		///    Gets or sets the source application of the base request{ t request} class.
		/// </summary>
		[Required]
		public string SourceApplication { get; set; }

		/// <summary>
		///    Gets or Sets the User Identifier
		/// </summary>
		/// <remarks>
		/// This the current signed in user not a System or Service account. This UserId is stored in
		/// logs and History of items retrieved.  It allows us to record and audit everyone that has
		/// accessed this service.
		/// </remarks>
		[Required]
		public string UserId { get; set; }
	}

	public abstract class BaseRequest<TRequest> : BaseRequest, IBaseRequest<TRequest>
	{
		/// <summary>
		///    Gets or sets the request data of the base request{ t request} class.
		/// </summary>
		public TRequest RequestData { get; set; }
	}
}