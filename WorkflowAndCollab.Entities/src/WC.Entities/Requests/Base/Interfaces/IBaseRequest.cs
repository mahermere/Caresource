// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Entities
//   IBaseRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Requests.Base.Interfaces
{
	using System;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public interface IBaseRequest
	{
		/// <summary>
		///    Gets or sets the request date time of the base request class.
		/// </summary>
		DateTime RequestDateTime { get; set; }

		/// <summary>
		///    Gets or sets the source application of the base request class.
		/// </summary>
		[Required]
		[Description("Identifier string to help identify the systesm calling the service")]
		string SourceApplication { get; set; }

		/// <summary>
		///    Gets or Sets the User Identifier
		/// </summary>
		[Required]
		[Description("The current signed in user, used for logging.")]
		string UserId { get; set; }
	}

	public interface IBaseRequest<TRequest> : IBaseRequest
	{
		/// <summary>
		///    Gets or sets the request data of the base request{ t request} class.
		/// </summary>
		TRequest RequestData { get; set; }
	}
}