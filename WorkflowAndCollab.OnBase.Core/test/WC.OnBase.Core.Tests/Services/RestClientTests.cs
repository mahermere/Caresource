// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.OnBase.Core.Tests
//   RestClientTests.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using CareSource.WC.OnBase.Core.Services;

namespace CareSource.WC.OnBase.Core.Tests.Services
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Providers;
	using CareSource.WC.Entities.Requests.Base;
    using CareSource.WC.OnBase.Core.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Shouldly;

	[TestClass]
	public class RestClientTests
	{
		//[TestMethod]
		public void AcceptOnBaseDocumentTest()
		{
			var client = new RestClient(new JsonSerializerHelper());
			string s = client.Post<string, string>(
                MediaTypes.ApplicationJson,
                "http://int-gateway-internal.caresource.corp:38080",
                "/Infrastructure/AcceptOnBaseDocument/1.0",
                "OnBaseEPSDTUser",
                "VUdP85r7",
                "{\"MemberId\":\"103103103-00\",\r\n \"DocumentId\":\"16409631\",\r\n \"DocumentURL\":\"https://onbaseint.caresource.corp/appnet/docpop/docpop.aspx?clienttype=html&docid=16409631\",\r\n \"DocumentType\":\"Transition of Care CCDA - AAMIR - IMAAN - 16409631-00 - 9/1/2017-NOTIFICATION\"\r\n}");

			s.ShouldNotBeNull();
		}

		//[TestMethod]
		public void PerformanceTestDocumentList()
		{
			var client =
				new RestClient(new JsonSerializerHelper());

            ListDocumentsRequest docRequest = new ListDocumentsRequest
			{
				Paging = new Paging
				{
					PageNumber = 1,
					PageSize = 100
				},
				DocumentTypes = new List<string>(),
				Filters = new List<Filter>
				{
					new Filter("Member Facing", null),
				},
				DisplayColumns = new List<string>
				{
					"Archive Only",
					"Correspondence ID",
					"Posted Date"
				}
			};

			Stopwatch listdocumentStopwatch = new Stopwatch();

			listdocumentStopwatch.Start();
			ListDocumentsResponse documentResponse = client.Get<ListDocumentsRequest, ListDocumentsResponse>(MediaTypes.ApplicationJson
                    , "https://dayintonapiwebsvc.caresource.corp/document/api/v1/610000011"
                    , ""
                    , null
                    , null
                    , docRequest);
			listdocumentStopwatch.Stop();


			writeline(
				$"ListDocuments: {listdocumentStopwatch.Elapsed}; Number of Documents Returned: {documentResponse.ResponseData.Count()}");
		}

		private void writeline(
			string line,
			[CallerMemberName] string memberName = "")
		{
			string filePath = "c:\\logs\\performance.log";

			FileInfo file = new FileInfo(filePath);
			file.Directory?.Create();

			using (StreamWriter streamWriter = new StreamWriter(
				filePath,
				true))
			{
				streamWriter.WriteLine($"[{DateTime.Now:u}] [{memberName}] [{line}]");
				streamWriter.Close();
			}
		}

		[TestMethod]
		public void GetQueryGetStringProviderTest()
		{
			RestClient client = new RestClient(new JsonSerializerHelper());

            GetProviderRequest request = new GetProviderRequest()
			{
				SourceApplication = "Testing Rest Client",
				UserId = "dummy"
			};
			string qs = client.GetQueryString(request);

			qs.ShouldNotBeNull();

			// Services is not running in INT, we need to revaluate this test
			//GetProviderResponse response = client.Get<GetProviderRequest, GetProviderResponse>(MediaTypes.ApplicationJson
   //                 , "https://dayintonapiwebsvc.caresource.corp/provider/api/v1/"
   //                 , "CS1613300242"
   //                 , null
   //                 , null
   //                 , request);

			//response.ShouldNotBeNull();
		}

        /*
		[TestMethod]
		public void GetQueryStringGetDocumentTest()
		{
			RestClient client = new RestClient(new JsonSerializerHelper());

            ListDocumentsRequest request = new ListDocumentsRequest
			{
				SourceApplication = "Test",
				Paging = new Paging
				{
					PageNumber = 1,
					PageSize = 100
				},
				DocumentTypes = new List<string>
				{
					"AR - Invoices",
					"AR - Dunning Letters "
				},
				Filters = new List<Filter>
				{
					new Filter(" Member Facing", null),
					new Filter("Archive Only", "NO", true)
				},
				StartDate = "2018.05.01",
				EndDate = "2019.03.05",
				DisplayColumns = new List<string>
				{
					"Archive Only "
				}
			};

			string qs = client.GetQueryString(request);

			qs.ShouldNotBeNull();

			ListDocumentsResponse response = client.Get<ListDocumentsRequest, ListDocumentsResponse>(MediaTypes.ApplicationJson
                    , "https://dayintonapiwebsvc.caresource.corp/document/api/v1/member/"
                    , "108334819"
                    , null
                    , null
                    , request);

			response.ShouldNotBeNull();
			response.CorrelationGuid.ShouldNotBe(Guid.Empty);
			response.ResponseData.Count().ShouldBeGreaterThan(0);
			response.TotalItems.ShouldBeGreaterThan(0);
		}*/
	}
}