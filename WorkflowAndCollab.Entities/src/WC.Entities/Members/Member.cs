// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   Member.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Members
{
	using System;
	using CareSource.WC.Entities.Common;

	/// <summary>
	///    Represents the data used to define a the member
	/// </summary>
	public class Member
	{
		/// <summary>
		///    Gets or sets the contrived key of the member class.
		/// </summary>
		public long? ContrivedKey { get; set; }

		/// <summary>
		///    Gets or sets the date of birth of the member class.
		/// </summary>
		public DateTime? DateOfBirth { get; set; }

		/// <summary>
		///    Gets or sets the email of the member class.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		///    Gets or sets the first name of the member class.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		///    Gets or sets the hicn of the member class.
		/// </summary>
		public string Hicn { get; set; }

		/// <summary>
		///    Gets or sets the home address of the member class.
		/// </summary>
		public Address HomeAddress { get; set; }

		/// <summary>
		///    Gets or sets the last name of the member class.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		///    Gets or sets the medicaid identifier of the member class.
		/// </summary>
		public string MedicaidId { get; set; }

		/// <summary>
		///    Gets or sets the middle initial of the member class.
		/// </summary>
		public string MiddleInitial { get; set; }

		/// <summary>
		///    Gets or sets the phone of the member class.
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		///    Gets or sets the subscriber identifier of the member class.
		/// </summary>
		public string SubscriberId { get; set; }

		/// <summary>
		///    Gets or sets the suffix of the member class.
		/// </summary>
		public string Suffix { get; set; }
	}
}