// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v4
{
	using CareSource.WC.Services.Document.Models.v4;

	/// <summary>
	///    Minimum functions an properties for a Document Manager
	/// </summary>
	/// <typeparam name="TDataModel">The type of the data model.</typeparam>
	public interface IDocumentManager<TDataModel> : Managers.IDocumentManager<TDataModel>
	{
		long GetTotalRecords(TotalDocumentCountRequest request);
	}
}