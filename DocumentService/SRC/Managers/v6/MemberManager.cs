// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   MemberManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v6
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Adapters.v6;
	using CareSource.WC.Services.Document.Models.v6;
	using Microsoft.Extensions.Logging;
	using DocumentHeader = CareSource.WC.Services.Document.Models.v6.DocumentHeader;

	/// <summary>
	///    Represents the data used to define a the document manager
	/// </summary>
	public class MemberManager : BaseDocumentManager, IMemberManager
	{
		private const string MemberFacingKeywordName = "Member Facing";
		private const string MemberIdIsRequired = "Member/Subscriber Id is required.";
		private const string MemberSuffixKeywordName = "Member Suffix";
		private const string SubscriberIdKeywordName = "Subscriber ID";
		private const string MemberIdKeywordName = "Member ID";
		private const string MemberSubscriberId = "Member/Subscriber Id";
		private const string MemberIdMustBeEither9Or11CharactersInLength
			= "Member Id must be either 9 or 11 characters in length.";

		public MemberManager(
			IOnBaseSqlAdapter<DocumentHeader> adapter,
			IOnBaseAdapter onBaseAdapter,
			ILogger logger)
			: base(
				adapter,
				onBaseAdapter,
				logger)
		{ }

		public string MemberId { get; set; }

		public override bool IsValid(
			ISearchRequest request,
			ModelStateDictionary modelState)
		{
			SetDisplayColumns(request);

			IsValid(
				(IFilteredRequest)request,
				modelState);

			return modelState.IsValid;
		}
		
		public override bool IsValid(
			IFilteredRequest request,
			ModelStateDictionary modelState)
		{
			if (MemberId.IsNullOrWhiteSpace())
			{
				modelState.AddModelError(
					MemberSubscriberId,
					MemberIdIsRequired);
			}

			if(MemberId.Length != 9 && MemberId.Length != 11)
			{
				modelState.AddModelError(
					MemberSubscriberId,
					MemberIdMustBeEither9Or11CharactersInLength);
			}

			SetFilters(request);

			base.IsValid(
				request,
				modelState);

			return modelState.IsValid;
		}

		public override (IDictionary<string, int>, long) DocumentTypeCounts(IFilteredRequest request)
		{
			SetFilters(request);
			return base.DocumentTypeCounts(request);
		}
		
		protected override void SetDisplayColumns(ISearchRequest request)
		{
			if (request.DisplayColumns == null)
			{
				request.DisplayColumns = new List<string>();
			}

			List<string> newDisplayColumns = new List<string>();

			if (request.DisplayColumns.All(dc => dc != SubscriberIdKeywordName))
			{
				newDisplayColumns.Add(SubscriberIdKeywordName);
			}

			if (request.DisplayColumns.All(dc => dc != MemberSuffixKeywordName))
			{
				newDisplayColumns.Add(MemberSuffixKeywordName);
			}

			if (request.DisplayColumns.All(dc => dc != MemberIdKeywordName))
			{
				newDisplayColumns.Add(MemberIdKeywordName);
			}

			newDisplayColumns.AddRange(request.DisplayColumns);

			request.DisplayColumns = newDisplayColumns;
		}

		protected override void SetFilters(IFilteredRequest request)
		{
			if (request.Filters == null)
			{
				request.Filters = new List<Filter>();
			}

			List<Filter> filters = new List<Filter>();

			if (MemberId.Length == 9 ||
				MemberId.Length == 11)
			{
				if (request.Filters.All(f => f.Name != SubscriberIdKeywordName))
				{
					filters.Add(
						new Filter(
							SubscriberIdKeywordName,
							MemberId.Substring(
								0,
								9)));
				}

				if (MemberId.Length == 11 &&
					request.Filters.All(f => f.Name != MemberSuffixKeywordName))
				{
					filters.Add(
						new Filter(
							MemberSuffixKeywordName,
							MemberId.Substring(
								9,
								2)));
				}
			}

			if (request.Filters.All(f => f.Name != MemberFacingKeywordName))
			{
				filters.Add(
					new Filter(
						MemberFacingKeywordName,
						null));
			}

			filters.AddRange(request.Filters);

			request.Filters = filters;
		}
	}
}