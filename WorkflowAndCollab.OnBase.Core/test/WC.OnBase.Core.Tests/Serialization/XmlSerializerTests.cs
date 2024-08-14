// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.OnBase.Core.Tests
//   XmlSerializerTests.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Tests.Serialization
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Shouldly;
    using CareSource.WC.OnBase.Core.Helpers;

    [TestClass]
	public class XmlSerializerTests
	{
		[TestMethod]
		public void FromXmlTest()
		{
			TestXml test = new TestXml
			{
				Id = 1,
				Name = "test name"
			};

            var _xmlSerializerHelper = new XmlSerializerHelper();

            TestXml xml = _xmlSerializerHelper.FromXml<TestXml>(
				"<TestXml xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Id>1</Id>  <Name>test name</Name></TestXml>");

			xml.ShouldNotBeNull();
			xml.ShouldBeOfType<TestXml>();
			xml.Id.ShouldBe(test.Id);
			xml.Name.ShouldBe(test.Name);
		}

		[TestMethod]
		public void ToXmlTest()
		{
			TestXml test = new TestXml
			{
				Id = 1,
				Name = "test name"
			};

            var _xmlSerializerHelper = new XmlSerializerHelper();

            string xml = _xmlSerializerHelper.ToXml(test);

			xml.ShouldNotBeNull();
			xml.ShouldNotBeEmpty();
			xml.ShouldBe("<TestXml><Id>1</Id><Name>test name</Name></TestXml>");
		}

		public class TestXml
		{
			public int Id { get; set; }

			public string Name { get; set; }
		}
	}
}