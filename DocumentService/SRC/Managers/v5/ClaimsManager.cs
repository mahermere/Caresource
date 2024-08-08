// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ClaimsManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v5
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.WorkView.v2;
	using CareSource.WC.Services.Document.Adapters.v5;
	using CareSource.WC.Services.Document.Models.v5;

	public class ClaimsManager : IClaimsManager<Document>
	{
		//private readonly IGetDocumentAdapter<OnBaseDocument> _documentAdapter;
		//private readonly ISearchWorkViewAdapter<WorkViewObject> _WorkViewAdapter;

		//public ClaimsManager(
		//	ISearchWorkViewAdapter<WorkViewObject> WorkViewAdapter,
		//	IGetDocumentAdapter<OnBaseDocument> documentAdapter)
		//{
		//	_WorkViewAdapter = WorkViewAdapter;
		//	_documentAdapter = documentAdapter;
		//}

		//public IEnumerable<Document> ClaimSearch(
		//	string claimId,
		//	ClaimsRequest request)
		//{
		//	IEnumerable<WorkViewObject> documentIds = _WorkViewAdapter.ClaimSearch(claimId);

		//	return CreateResponse(
		//		request,
		//		documentIds);
		//}

		//public IEnumerable<Document> MemberSearch(
		//	string memberId,
		//	ClaimsRequest request)
		//{
		//	IEnumerable<WorkViewObject> documentIds = _WorkViewAdapter.MemberSearch(memberId);

		//	return CreateResponse(
		//		request,
		//		documentIds);
		//}

		//public IEnumerable<Document> ProviderSearch(
		//	string providerId,
		//	ClaimsRequest request)
		//{
		//	IEnumerable<WorkViewObject> documentIds = _WorkViewAdapter.ProviderSearch(providerId);

		//	return CreateResponse(
		//		request,
		//		documentIds);
		//}

		//private IEnumerable<Document> CreateResponse(
		//	ClaimsRequest request,
		//	IEnumerable<WorkViewObject> documentIds)
		//{
		//	int page = documentIds.Count() < request.Paging.PageSize
		//		? 1
		//		: request.Paging.PageNumber;

		//	return documentIds
		//		.Skip((page - 1) * request.Paging.PageSize)
		//		.Take(request.Paging.PageSize)
		//		.Select(
		//			wvo =>
		//			{
		//				OnBaseDocument obDoc = _documentAdapter.GetDocument(
		//					Convert.ToInt64(
		//						wvo.Attributes.FirstOrDefault(a => a.Name.Contains("DocumentId"))
		//							.Value),
		//					request.DisplayColumns);

		//				return new Document
		//				{
		//					Filename = obDoc.Filename,
		//					Id = obDoc.Id,
		//					Name = obDoc.Name,
		//					Type = obDoc.Type,
		//					DocumentDate = obDoc.DocumentDate,
		//					DisplayColumns = (Dictionary<string, string>)obDoc.Keywords
		//				};
		//			}
		//		);
		//}
		public IEnumerable<Document> ClaimSearch(
			string claimId,
			ClaimsRequest request)
			=> throw new NotImplementedException();

		public IEnumerable<Document> MemberSearch(
			string memberId,
			ClaimsRequest request)
			=> throw new NotImplementedException();

		public IEnumerable<Document> ProviderSearch(
			string providerId,
			ClaimsRequest request)
			=> throw new NotImplementedException();
	}
}