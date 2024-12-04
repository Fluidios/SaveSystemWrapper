using System;
using UnityEngine;

namespace Game.SaveSystem.Core
{
    public class PlayerPrefsSaveModule : SaveModuleBase
    {
        public PlayerPrefsSaveModule(){}
        public override void Delete(string key, SMSettings settings)
        {
            if(settings.SaveLocation == SMSettings.Location.Cache)
            {
                SMLogger.LogError("PlayerPrefsSaveModule does not support cache files.");
                return;
            }
            PlayerPrefs.DeleteKey(string.Format("SLOT{0}-{1}", settings.SaveSlot, key));
        }
        public override void DeleteAll(SMSettings settings)
        {
            if(settings.SaveLocation == SMSettings.Location.Cache)
            {
                SMLogger.LogError("PlayerPrefsSaveModule does not support cache files.");
                return;
            }
            PlayerPrefs.DeleteAll();
        }

        public override T Load<T>(string key, T defaultValue, SMSettings settings)
        {
            if(settings.SaveLocation == SMSettings.Location.Cache)
            {
                SMLogger.LogWarning("PlayerPrefsSaveModule does not support cache files.");
            }
            switch (IsTypeSupported(typeof(T)))
            {
                case SaveType.Int:
                    return (T)(object)PlayerPrefs.GetInt(string.Format("SLOT{0}-{1}", settings.SaveSlot, key), (int)(object)defaultValue);
                case SaveType.Float:
                    return (T)(object)PlayerPrefs.GetFloat(string.Format("SLOT{0}-{1}", settings.SaveSlot, key), (float)(object)defaultValue);
                case SaveType.String:
                    return (T)(object)PlayerPrefs.GetString(string.Format("SLOT{0}-{1}", settings.SaveSlot, key), (string)(object)defaultValue);
                case SaveType.Bool:
                    return (T)(object)(PlayerPrefs.GetInt(string.Format("SLOT{0}-{1}", settings.SaveSlot, key), (bool)(object)defaultValue ? 1 : 0) == 1);
                case SaveType.NotSupported:
                    SMLogger.LogError($"Type {typeof(T)} is not supported by PlayerPrefsSaveModule");
                    return defaultValue;
            }
            return defaultValue;
        }

        public override void Save<T>(string key, T value, SMSettings settings)
        {
            if(settings.SaveLocation == SMSettings.Location.Cache)
            {
                SMLogger.LogWarning("PlayerPrefsSaveModule does not support cache files.");
            }
            switch (IsTypeSupported(typeof(T)))
            {
                case SaveType.Int:
                    PlayerPrefs.SetInt(string.Format("SLOT{0}-{1}", settings.SaveSlot, key), (int)(object)value);
                    break;
                case SaveType.Float:
                    PlayerPrefs.SetFloat(string.Format("SLOT{0}-{1}", settings.SaveSlot, key), (float)(object)value);
                    break;
                case SaveType.String:
                    PlayerPrefs.SetString(string.Format("SLOT{0}-{1}", settings.SaveSlot, key), (string)(object)value);
                    break;
                case SaveType.Bool:
                    PlayerPrefs.SetInt(string.Format("SLOT{0}-{1}", settings.SaveSlot, key), (bool)(object)value ? 1 : 0);
                    break;
                case SaveType.NotSupported:
                    SMLogger.LogError($"Type {typeof(T)} is not supported by PlayerPrefsSaveModule");
                    break;
            }

        }
        protected override void CreateCacheFile(SMSettings settings)
        {
            SMLogger.LogWarning("PlayerPrefsSaveModule does not support cache files.");
        }
        public override void SaveCacheToDisk(SMSettings settings)
        {
            SMLogger.LogWarning("PlayerPrefsSaveModule does not support cache files.");
        }
        private SaveType IsTypeSupported(Type type)
        {
            return type switch
            {
                Type intType when intType == typeof(int) => SaveType.Int,
                Type floatType when floatType == typeof(float) => SaveType.Float,
                Type stringType when stringType == typeof(string) => SaveType.String,
                Type boolType when boolType == typeof(bool) => SaveType.Bool,
                _ => SaveType.NotSupported,
            };
        }
        public override void CreateBackup(SMSettings settings)
        {
            SMLogger.LogWarning("PlayerPrefsSaveModule does not support backup files.");
        }
        public override void RestoreBackup(SMSettings settings)
        {
            SMLogger.LogWarning("PlayerPrefsSaveModule does not support backup files.");
        }

        private enum SaveType
        {
            NotSupported,
            Int,
            Float,
            String,
            Bool
        }
    }
}
