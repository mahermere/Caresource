// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ProviderManager.cs
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
	public class ProviderManager : BaseDocumentManager, IProviderManager
	{
		private const string ProviderFacingKeywordName = "Provider Facing";
		private const string ProviderIdIsRequired = "Provier Id or Provider TIN is required.";
		private const string ProviderIdKeywordName = "Provider ID";
		private const string ProviderTinKeywordName = "Provider TIN";
		private const string ProviderIdMustBe12CharactersInLength
			= "Provider Id must be 12 characters in length";

		private const string ProviderTinMustBe9CharactersInLength
			= "Provider TIN must be 9 characters in length";

		public ProviderManager(
			IOnBaseSqlAdapter<DocumentHeader> adapter,
			IOnBaseAdapter onBaseAdapter,
			ILogger logger)
			: base(
				adapter,
				onBaseAdapter,
				logger)
		{ }

		public string ProviderId { get; set; }
		public string ProviderTin { get; set; }

		public override bool IsValid(
			ISearchRequest request,
			ModelStateDictionary modelState)
		{
			SetDisplayColumns(request);

			return IsValid(
				(IFilteredRequest)request,
				modelState);
		}

		public override bool IsValid(
			IFilteredRequest request,
			ModelStateDictionary modelState)
		{
			if (ProviderId.IsNullOrWhiteSpace()
				&& ProviderTin.IsNullOrWhiteSpace())
			{
				modelState.AddModelError(
					ProviderIdKeywordName,
					ProviderIdIsRequired);
			}

			if (!ProviderId.IsNullOrWhiteSpace()
				&& ProviderId.Length != 12)
			{
				modelState.AddModelError(
					ProviderIdKeywordName,
					ProviderIdMustBe12CharactersInLength);
			}

			if (!ProviderTin.IsNullOrWhiteSpace()
				&& ProviderTin.Length != 9)
			{
				modelState.AddModelError(
					ProviderTinKeywordName,
					ProviderTinMustBe9CharactersInLength);
			}

			SetFilters(request);

			base.IsValid(
				request,
				modelState);

			return modelState.IsValid;
		}

		protected override void SetDisplayColumns(ISearchRequest request)
		{
			if (request.DisplayColumns == null)
			{
				request.DisplayColumns = new List<string>();
			}

			List<string> newDisplayColumns = new List<string>();

			if (!ProviderId.IsNullOrWhiteSpace()
				&& request.DisplayColumns.All(dc => dc != ProviderIdKeywordName))
			{
				newDisplayColumns.Add(ProviderIdKeywordName);
			}

			if (!ProviderTin.IsNullOrWhiteSpace()
				&& request.DisplayColumns.All(dc => dc != ProviderTinKeywordName))
			{
				newDisplayColumns.Add(ProviderTinKeywordName);
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

			List<Filter> newFilters = new List<Filter>();

			if (ProviderId.IsNullOrWhiteSpace()
				&& request.Filters.All(f => f.Name != ProviderTinKeywordName))
			{
				newFilters.Add(
					new Filter(
						ProviderTinKeywordName,
						ProviderTin));
			}

			if (ProviderTin.IsNullOrWhiteSpace()
				&& request.Filters.All(f => f.Name != ProviderIdKeywordName))
			{
				newFilters.Add(
					new Filter(
						ProviderIdKeywordName,
						ProviderId));
			}

			if (request.Filters.All(f => f.Name != ProviderFacingKeywordName))
			{
				newFilters.Add(
					new Filter(
						ProviderFacingKeywordName,
						null));
			}

			newFilters.AddRange(request.Filters);

			request.Filters = newFilters;
		}
	}
}