
namespace Game.SaveSystem.Core
{
    public class SMSettings
    {
        private static string _defaultSaveFile;
        public string FilePath { get; private set; }
        public Location SaveLocation { get; set; }
        public int SaveSlot { get; set; }

        public SMSettings()
        {
            if(string.IsNullOrEmpty(_defaultSaveFile))
            {
                _defaultSaveFile = SMConfig.GetDefaultSaveFile();
            }
            FilePath = _defaultSaveFile;
            SaveLocation = Location.PersistentData;
            SaveSlot = 0;
        }
        public SMSettings(string filePath)
        {
            FilePath = CheckCustomSaveFilePath(filePath);
            SaveLocation = Location.PersistentData;
            SaveSlot = 0;
        }
        private static string CheckCustomSaveFilePath(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                if(string.IsNullOrEmpty(_defaultSaveFile))
                {
                    _defaultSaveFile = SMConfig.GetDefaultSaveFile();
                }
                filePath = _defaultSaveFile;
            }
            else if(filePath.EndsWith(".json") == false)
            {
                throw new System.ArgumentException("File path must end with .json");
            }
            return filePath;
        }

        public enum Location
        {
            PersistentData,
            Cache
        }
    }
}
