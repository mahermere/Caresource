// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2023.  All rights reserved.
// 
//    WC.Services.Claims
//    Member.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Models
{
	public class Member
	{
		public long? ContrivedKey {
			get;
			set;
		}

		public DateTime? DateOfBirth {
			get;
			set;
		}

		public string Email {
			get;
			set;
		} = string.Empty;

		public string FirstName {
			get;
			set;
		} = string.Empty;

		public string Hicn {
			get;
			set;
		} = string.Empty;

		public Address HomeAddress {
			get;
			set;
		} = new Address();

		public string LastName {
			get;
			set;
		} = string.Empty;

		public string MedicaidId {
			get;
			set;
		} = string.Empty;

		public string MiddleInitial {
			get;
			set;
		} = string.Empty;

		public string Phone {
			get;
			set;
		} = string.Empty;

		public string SubscriberId {
			get;
			set;
		} = string.Empty;

		public string Suffix {
			get;
			set;
		} = string.Empty;
	}
}