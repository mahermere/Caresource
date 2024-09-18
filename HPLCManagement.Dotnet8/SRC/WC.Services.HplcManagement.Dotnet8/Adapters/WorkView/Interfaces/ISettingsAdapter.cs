namespace WC.Services.HplcManagement.Dotnet8.Adapters.WorkView.Interfaces
{
    public interface ISettingsAdapter
    {
        string GetSetting(string key, string defaultValue = null);

        TSettingType GetSetting<TSettingType>(string key, TSettingType defaultValue = default);

        TSectionType GetSection<TSectionType>(string section, TSectionType defaultValue = default) where TSectionType : new();
    }
}