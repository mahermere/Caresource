// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ISettingsAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Configuration;

public interface ISettingsAdapter
{
	string GetSetting(
		string key);

	T GetSetting<T>(
		string key);

	T GetSection<T>(
		string section) where T : new();
}