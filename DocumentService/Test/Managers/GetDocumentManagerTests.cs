// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document.Tests
//   GetDocumentManagerTests.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Tests.Managers
{
	using CareSource.WC.Services.Document.Adapters;
	using CareSource.WC.Services.Document.Managers;
	using CareSource.WC.Services.Document.Models;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Shouldly;

	[TestClass]
	public class GetDocumentManagerTests
	{
		private readonly Mock<IGetDocumentAdapter<OnBaseDocument>> _mockIGetDocumentAdapter;

		private readonly GetDocumentManager _getDocumentManager;

		public GetDocumentManagerTests()
		{
			_mockIGetDocumentAdapter = new Mock<IGetDocumentAdapter<OnBaseDocument>>();

			_getDocumentManager = new GetDocumentManager(_mockIGetDocumentAdapter.Object);
		}

		[TestMethod]
		public void GetDocumentTest()
		{
			//OnBaseDocument onbaseDocument = new OnBaseDocument
			//{
			//	Id = 123456
			//};
			//_mockIGetDocumentAdapter
			//	.Setup(
			//		gda => gda.GetDocument(
			//			It.IsAny<long>(),
			//			It.IsAny<GetDocumentRequest>()))
			//	.Returns(onbaseDocument);

			//var document = _getDocumentManager.GetDocument(
			//	123456,
			//	new GetDocumentRequest
			//	{
			//		SourceApplication="Test Suite",
			//		UserId= "Green Goblin"
			//	});

			//document.ShouldNotBeNull();
			//document.Id.ShouldBe(123456);
		}
	}
}