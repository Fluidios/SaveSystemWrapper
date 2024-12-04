namespace Game.SaveSystem.Core
{
    /// <summary>
    /// Provides static methods for saving and loading data using a specified save module.
    /// </summary>
    public static class SaveManager
    {
        private static SaveModuleBase _saveModule;
        private static bool _initialized = false;
        private static bool _useBackupFilesProtection = false;
        private static SMSettings _defaultSettings = new SMSettings();

        private static void CheckDefaults(ref SMSettings settings)
        {
            if (_saveModule == null)
            {
                _saveModule = SMConfig.GetNewSaveModuleInstance();
            }
            if (settings == null)
            {
                settings = _defaultSettings;
            }
            if(!_initialized)
            {
                _useBackupFilesProtection = SMConfig.UseBackupFilesProtection;
                _initialized = true;
            }
        }
        public static void Save<T>(string key, T value, SMSettings settings = null)
        {
            CheckDefaults(ref settings);
            try
            {
                _saveModule.Save(key, value, settings);
            }
            catch (System.Exception e)
            {
                if(_useBackupFilesProtection)
                {
                    _saveModule.RestoreBackup(settings);
                    _saveModule.Save(key, value, settings);
                }
                else
                {
                    SMLogger.LogError(e.Message);
                }
            }
        }
        public static T Load<T>(string key, T defaultValue, SMSettings settings = null)
        {
            CheckDefaults(ref settings);
            T value = defaultValue;
            try
            {
                value = _saveModule.Load(key, defaultValue, settings);
            }
            catch (System.Exception e)
            {
                if(_useBackupFilesProtection)
                {
                    _saveModule.RestoreBackup(settings);
                    value = _saveModule.Load(key, defaultValue, settings);
                }
                else
                {
                    SMLogger.LogError(e.Message);
                }
            }
            return value;
        }
        public static void DeleteKey(string key, SMSettings settings = null)
        {
            CheckDefaults(ref settings);
            try
            {
                _saveModule.Delete(key, settings);
            }
            catch (System.Exception e)
            {
                if (_useBackupFilesProtection)
                {
                    _saveModule.RestoreBackup(settings);
                    _saveModule.Delete(key, settings);
                }
                else
                {
                    SMLogger.LogError(e.Message);
                }
            }
        }
        public static void DeleteAll(SMSettings settings = null)
        {
            if(settings == null) settings = new SMSettings(string.Empty);
            CheckDefaults(ref settings);
            try
            {
                _saveModule.DeleteAll(settings);
            }
            catch (System.Exception e)
            {
                if (_useBackupFilesProtection)
                {
                    _saveModule.RestoreBackup(settings);
                    _saveModule.DeleteAll(settings);
                }
                else
                {
                    SMLogger.LogError(e.Message);
                }
            }
        }
        public static void CreateBackup(SMSettings settings)
        {
            CheckDefaults(ref settings);
            try
            {
                _saveModule.CreateBackup(settings);
            }
            catch (System.Exception e)
            {
                SMLogger.LogError(e.Message);
            }
        }
        public static void RestoreBackup(SMSettings settings)
        {
            CheckDefaults(ref settings);
            try
            {
                _saveModule.RestoreBackup(settings);
            }
            catch (System.Exception e)
            {
                SMLogger.LogError(e.Message);
            }
        }
    }
}
