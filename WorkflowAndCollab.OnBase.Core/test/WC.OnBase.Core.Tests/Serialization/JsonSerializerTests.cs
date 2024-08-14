// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.OnBase.Core.Tests
//   JsonSerializerTests.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Tests.Serialization
{
    using CareSource.WC.OnBase.Core.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Shouldly;

	[TestClass]
	public class JsonSerializerTests
	{
		[TestMethod]
		public void FromJsonTest()
		{
			Testjson test = new Testjson
			{
				Id = 1,
				Name = "test name"
			};

            var _jsonSerializerHelper = new JsonSerializerHelper();
            Testjson json = _jsonSerializerHelper.FromJson<Testjson>("{\"Id\":1,\"Name\":\"test name\"}");

			json.ShouldNotBeNull();
			json.ShouldBeOfType<Testjson>();
			json.Id.ShouldBe(test.Id);
			json.Name.ShouldBe(test.Name);
		}

		[TestMethod]
		public void NullTest()
		{
			string test = null;
            var _jsonSerializerHelper = new JsonSerializerHelper();
            string result = _jsonSerializerHelper.ToJson(test);

			result.ShouldNotBeNull();
		}

		[TestMethod]
		public void ToJsonTest()
		{
			Testjson test = new Testjson
			{
				Id = 1,
				Name = "test name"
			};
            var _jsonSerializerHelper = new JsonSerializerHelper();
            string json = _jsonSerializerHelper.ToJson(test);

			json.ShouldNotBeNull();
			json.ShouldNotBeEmpty();
			json.ShouldBe("{\"Id\":1,\"Name\":\"test name\"}");
		}
	}

	/// <summary>
	///    Simple class to test some basic Serialization
	/// </summary>
	internal class Testjson
	{
		/// <summary>
		///    Gets or sets the identifier.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///    Gets or sets the name.
		/// </summary>
		public string Name { get; set; }
	}
}