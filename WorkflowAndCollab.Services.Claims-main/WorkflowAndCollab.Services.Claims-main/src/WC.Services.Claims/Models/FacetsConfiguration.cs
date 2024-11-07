// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    FacetsConfiguration.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Models
{
	public class FacetsConfiguration
	{
		public string ServiceUrl
		{
			get;
			set;
		}

		public string EndPoint
		{
			get;
			set;
		}

		public string Identity
		{
			get;
			set;
		}

		public string Region
		{
			get;
			set;
		}

		public MemberServiceSettings MemberServiceSettings
		{
			get;
			set;
		}

		public ClaimServiceSettings ClaimServiceSettings
		{
			get;
			set;
		}

		public ClaimLinesServiceSettings ClaimLinesServiceSettings
		{
			get;
			set;
		}

		public ClaimDcnServiceSettings ClaimDcnServiceSettings
		{
			get;
			set;
		}

		public DataBaseSettings DataBaseSettings
		{
			get;
			set;
		}

		public string AuthEndpoint
		{
			get;
			set;
		}

		public string AuthUser
		{
			get;
			set;
		}
	}

	public class MemberServiceSettings
	{
		public string ServiceAction
		{
			get;
			set;
		}
	}

	public class ClaimServiceSettings
	{
		public string ServiceAction
		{
			get;
			set;
		}

		public int? PageSize
		{
			get;
			set;
		}

		public int? MaxPages
		{
			get;
			set;
		}

		public string[] ClaimTypeCodes
		{
			get;
			set;
		}
	}

	public class ClaimLinesServiceSettings
	{
		public string ServiceAction
		{
			get;
			set;
		}
	}

	public class ClaimDcnServiceSettings
	{
		public string ServiceUrl
		{
			get;
			set;
		}
	}

	public class DataBaseSettings
	{
		public string DataSource
		{
			get;
			set;
		}

		public string InitialCatalog
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public int? QuerySize
		{
			get;
			set;
		}
	}
}