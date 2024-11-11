// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    AppSettingsAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Configuration;

public class AppSettingsAdapter : ISettingsAdapter
{
	private readonly IConfiguration _configuration;

	public AppSettingsAdapter(
		IConfiguration configuration) => _configuration = configuration;

	public T GetSection<T>(
		string section)
		where T : new()
	{
		T sectionObj = new();

		IConfigurationSection configSection = _configuration.GetSection(section);

		if (!configSection.Exists())
		{
			throw new ArgumentNullException(
				$"Could not find section '{section}' in appsettings.json file.");
		}

		configSection.Bind(sectionObj);

		return sectionObj;
	}

	public string GetSetting(
		string key)
	{
		string value = _configuration.GetValue<string>(
			key,
			null);

		if (value == null)
		{
			throw new ArgumentNullException(
				$"Could not find setting value for key '{key}' in appsettings.json file.");
		}

		return value;
	}

	public T GetSetting<T>(
		string key) => _configuration.GetValue<T>(key);
}