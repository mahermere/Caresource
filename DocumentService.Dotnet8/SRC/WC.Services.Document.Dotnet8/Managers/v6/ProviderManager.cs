// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ProviderManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.v6
{
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.Common;
	//using CareSource.WC.OnBase.Core.ExtensionMethods;
	using WC.Services.Document.Dotnet8.Adapters.v6;
	using WC.Services.Document.Dotnet8.Models.v6;
	using Microsoft.Extensions.Logging;
	using DocumentHeader = WC.Services.Document.Dotnet8.Models.v6.DocumentHeader;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using WC.Services.Document.Dotnet8.Adapters.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Managers.v6.Interfaces;


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
			log4net.ILog logger)
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
			if (String.IsNullOrWhiteSpace(ProviderId)
				&& String.IsNullOrWhiteSpace(ProviderTin))
			{
				modelState.AddModelError(
					ProviderIdKeywordName,
					ProviderIdIsRequired);
			}

			if (!String.IsNullOrWhiteSpace(ProviderId)
				&& ProviderId.Length != 12)
			{
				modelState.AddModelError(
					ProviderIdKeywordName,
					ProviderIdMustBe12CharactersInLength);
			}

			if (!String.IsNullOrWhiteSpace(ProviderTin)
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

			if (!String.IsNullOrWhiteSpace(ProviderId)
				&& request.DisplayColumns.All(dc => dc != ProviderIdKeywordName))
			{
				newDisplayColumns.Add(ProviderIdKeywordName);
			}

			if (!String.IsNullOrWhiteSpace(ProviderTin)
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

			if (String.IsNullOrWhiteSpace(ProviderId)
				&& request.Filters.All(f => f.Name != ProviderTinKeywordName))
			{
				newFilters.Add(
					new Filter(
						ProviderTinKeywordName,
						ProviderTin));
			}

			if (String.IsNullOrWhiteSpace(ProviderTin)
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