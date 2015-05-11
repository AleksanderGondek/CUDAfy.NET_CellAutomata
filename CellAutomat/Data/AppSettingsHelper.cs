using System;
using System.Configuration;

namespace CellAutomat.Data
{
    public static class AppConfigHelper
    {
        public static String GetValueFromAppSettings(String key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            if (!appSettings.HasKeys())
            {
                throw new ConfigurationErrorsException("App settings file contains no entries!");
            }

            var customSetting = appSettings[key];
            if (null == customSetting)
            {
                throw new SettingsPropertyNotFoundException("There isn't setting with key: " + key);
            }

            return customSetting;
        }
    }
}
