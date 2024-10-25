// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Services.Document.WC.Services.Document
//   MemberDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Documents;
    using WC.Services.Document.Dotnet8.Adapters.Interfaces;
    using WC.Services.Document.Dotnet8.Managers.Interfaces;

    /// <summary>
    /// Represents the data used to define a the member document manager
    /// </summary>
    public class MemberDocumentManager :
		BaseDocumentManager<DocumentHeader>,
		IMemberDocumentManager<DocumentHeader>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MemberDocumentManager"/> class.
		/// </summary>
		/// <param name="documentAdapter">The provider broker.</param>
		public MemberDocumentManager(ISearchDocumentAdapter<DocumentHeader> documentAdapter)
			: base(documentAdapter)
		{ }

		/// <summary>
		///    Sets the display columns.
		/// </summary>
		protected override void SetDisplayColumns()
		{
			List<string> displayColumns = RequestData.DisplayColumns.ToList();

			if (!RequestData.DisplayColumns.Any(
				f => f.Equals(
					"Member ID",
					StringComparison.InvariantCulture)))
			{
				displayColumns.Add("Member ID");
			}

			if (!RequestData.DisplayColumns.Any(
				f => f.Equals(
					"Subscriber ID",
					StringComparison.InvariantCulture)))
			{
				displayColumns.Add("Subscriber ID");
			}

			if (!RequestData.DisplayColumns.Any(
				f => f.Equals(
					"Member Suffix",
					StringComparison.InvariantCulture)))
			{
				displayColumns.Add("Member Suffix");
			}

			RequestData.DisplayColumns = displayColumns;
		}

		/// <summary>
		///    Sets the filters.  Adds the Member facing filter, and the Provider ID filter
		/// </summary>
		protected override void SetFilters()
		{
			// create a new list for filters; it has two more than the count of passed in filters so
			// we can add [Subscriber ID], [member suffix], and [Member Facing]
			List<Filter> filters = new List<Filter>(RequestData.Filters.Count() + 3)
			{
				new Filter(
					"Subscriber ID",
					SearchId.Substring(
						0,
						9))
			};

			if (SearchId.Length == 11)
			{
				filters.Add(
					new Filter(
						"Member Suffix",
						SearchId.Substring(
							9,
							2)));
			}

			if (!filters.Any(f => f.Name.Equals(
				"Member Facing",
				StringComparison.InvariantCulture)))
			{
				filters.Add(
					new Filter(
						"Member Facing",
						null));
			}

			filters.AddRange(RequestData.Filters);

			RequestData.Filters = filters.AsEnumerable();
		}

		/// <summary>
		/// Validates the request.
		/// </summary>
		/// <exception cref="Exception">Member Id must be 9 or 11 characters</exception>
		protected override void ValidateRequest()
		{
			if (!SearchId.Length.Equals(9)
				&& !SearchId.Length.Equals(11))
			{
				throw new Exception("Member Id must be 9 or 11 characters");
			}

			base.ValidateRequest();
		}
	}
}