using System;
using UnityEngine;

namespace Game.SaveSystem.Core
{
    public class SMConfig : ScriptableObject
    {
        [SerializeField] private string _defaultSaveFile = "save.json";
        [SerializeField] private bool _useBackupFilesProtection = false;
        [SerializeField] private DrawableType<SaveModuleBase> _activeSaveModule = new DrawableType<SaveModuleBase>(typeof(PlayerPrefsSaveModule));
        public Type ActiveSaveModuleType => Type.GetType($"{_activeSaveModule.TypeName}, {_activeSaveModule.TypeAssembly}");
        public string DefaultSaveFile => _defaultSaveFile;

        public static SaveModuleBase GetNewSaveModuleInstance()
        {
            var config = Resources.Load<SMConfig>("SMConfig");
            if (config == null)
            {
                SMLogger.LogError("SMConfig asset not found in Resources.");
                return new PlayerPrefsSaveModule();
            }
            var saveModuleType = config.ActiveSaveModuleType;
            return (SaveModuleBase)System.Activator.CreateInstance(saveModuleType);
        }
        public static string GetDefaultSaveFile()
        {
            var config = Resources.Load<SMConfig>("SMConfig");
            if (config == null)
            {
                SMLogger.LogError("SMConfig asset not found in Resources.");
                return "save.json";
            }
            return config.DefaultSaveFile;
        }
        public static bool UseBackupFilesProtection
        {
            get
            {
                var config = Resources.Load<SMConfig>("SMConfig");
                if (config == null)
                {
                    SMLogger.LogError("SMConfig asset not found in Resources.");
                    return false;
                }
                return config._useBackupFilesProtection;
            }
        }
        #if UNITY_EDITOR
        public void ResetActiveSaveModule()
        {
            _activeSaveModule = new DrawableType<SaveModuleBase>(typeof(PlayerPrefsSaveModule));
        }
        #endif
    }
}
