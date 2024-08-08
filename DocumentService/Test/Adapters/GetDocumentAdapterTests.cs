// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document.Tests
//   GetDocumentManagerTests.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Tests.Adapters
{
	using System;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.Services.Document.Adapters;
	using Hyland.Unity;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Shouldly;

	[TestClass]
	public class GetDocumentAdapterTests
	{
		private readonly Mock<IApplicationConnectionAdapter<Application>>
			_mockApplicationConnectionAdapterMock;

		private readonly OnBaseGetDocumentAdapter _getDocumentAdapter;

		public GetDocumentAdapterTests()
		{
			_mockApplicationConnectionAdapterMock = new Mock<IApplicationConnectionAdapter<Application>>();

			_getDocumentAdapter = new OnBaseGetDocumentAdapter(_mockApplicationConnectionAdapterMock.Object);
		}

		[TestMethod]
		public void GetDocumenTestNullUser()
		{
			Should.Throw<NullReferenceException>(() => _getDocumentAdapter.GetDocument(
					123459,
					new GetDocumentRequest
					{
						SourceApplication = "Test Suite"
					}))
				.Message.ShouldBe("The GetDocumentRequest.UserId is Required");
		}
	}
}