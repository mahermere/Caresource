// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.OnBase.Core.Tests
//   AppSettingsTests.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Tests.Configuration
{
    using CareSource.WC.OnBase.Core.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Shouldly;

    [TestClass]
	public class AppSettingsTests
	{
		//[TestMethod]
		public void GetSetting_Int_Test()
		{
            var _settingsAdapter = new AppSettingsAdapter();

			int value = _settingsAdapter.GetSetting(
				"IntTest1",
				7);
			value.ShouldBe(1);

			value = _settingsAdapter.GetSetting(
				"IntTest2",
				7);
			value.ShouldBe(7);

			value = _settingsAdapter.GetSetting(
				"IntTest3",
				7);
			value.ShouldBe(7);

			value = _settingsAdapter.GetSetting(
				"IntTest4",
				9);
			value.ShouldBe(9);

			value = _settingsAdapter.GetSetting(
				"IntTest5",
				7);
			value.ShouldBe(7);
		}

		//[TestMethod]
		public void GetSetting_String_Test()
		{
            var _settingsAdapter = new AppSettingsAdapter();

            string value = _settingsAdapter.GetSetting(
				"IntTest1",
				"7");
			value.ShouldBe("1");

			value = _settingsAdapter.GetSetting(
				"StringTest1",
				"7");
			value.ShouldBe("OBServer_Int");

			value = _settingsAdapter.GetSetting(
				"StringTest2",
				"7");
			value.ShouldBe("");

			value = _settingsAdapter.GetSetting(
				"StringTest3",
				"7");
			value.ShouldBe("7");
		}

		//[TestMethod]
		public void GetSetting_NoDefault_Test()
		{
            var _settingsAdapter = new AppSettingsAdapter();

            string value = _settingsAdapter.GetSetting("IntTest1");
			value.ShouldBe("1");

			value = _settingsAdapter.GetSetting("StringTest1");
			value.ShouldBe("OBServer_Int");

			value = _settingsAdapter.GetSetting("StringTest2");
			value.ShouldBe("");

			value = _settingsAdapter.GetSetting("StringTest3");
			value.ShouldBeNull();
		}
	}
}