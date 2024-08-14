// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ISettingsAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Configuration.Interfaces
{
	public interface ISettingsAdapter
	{
		string GetSetting(
			string key);

		T GetSetting<T>(
			string key);

		T GetSection<T>(
			string section) where T : new();
	}
}