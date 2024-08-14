//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    OnBase.Core
//    AppSettingsAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Configuration
{
	using System;
	using System.Configuration;
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;

	public class AppSettingsAdapter : ISettingsAdapter
	{
		public TSectionType GetSection<TSectionType>(string section,
			TSectionType defaultValue = default)
			where TSectionType : new()
		{
			object value = ConfigurationManager.GetSection(section);

			if (value == null)
			{
				return defaultValue;
			}

			return (TSectionType)value;
		}

		public string GetSetting(
			string key,
			string defaultValue = null)
		{
			string value = ConfigurationManager.AppSettings.Get(key);

			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}

			return value;
		}

		public TSettingType GetSetting<TSettingType>(string key,
			TSettingType defaultValue = default)
		{
			string value = ConfigurationManager.AppSettings.Get(key);

			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}

			return (TSettingType)Convert.ChangeType(value, typeof(TSettingType));
		}
	}
}