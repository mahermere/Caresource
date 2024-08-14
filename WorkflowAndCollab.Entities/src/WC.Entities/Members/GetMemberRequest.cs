// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   GetMemberRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Members
{
	using System.ComponentModel.DataAnnotations;
	using CareSource.WC.Entities.Requests.Base;

	public class GetMemberRequest : BaseRequest<GetMemberData>
	{ }

	public class GetMemberData
	{
		[StringLength(11)]
		public string MemberId { get; set; }

		[StringLength(9)]
		public string SubscriberId { get; set; }

		[StringLength(2)]
		public string Suffix { get; set; }
	}
}