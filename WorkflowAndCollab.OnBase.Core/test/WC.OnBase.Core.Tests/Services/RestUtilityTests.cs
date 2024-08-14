namespace WorkFlowAndCollab.Tools.Services.Tests
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using WorkFlowAndCollab.Entities.Responses;
	using WorkFlowAndCollab.Entities.Responses.Base;

	[TestClass]
	public class RestUtilityTests
	{
		//[TestMethod]
		public void PostTest()
		{
			RestClient<BaseResponse, BaseResponse> client = new RestClient<BaseResponse, BaseResponse>(
				MediaTypes.ApplicationJson,
				"http://localhost:7061/api/v1/appealgrievances/",
				"PostTesting");
			BaseResponse s = client.Post(
				new BaseResponse(ResponsStatuses.Failure,
				"here",
				"0"));
		}

		//[TestMethod]
		public void GetTest()
		{
			RestClient<string, string> client = new RestClient<string, string>(
				MediaTypes.ApplicationJson,
				"http://localhost:7061/api/v1/appealgrievances/",
				"");
			string s = client.Get(null);
		}
	}
}