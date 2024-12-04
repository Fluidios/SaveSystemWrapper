namespace Game.SaveSystem.Core
{
    public abstract class SaveModuleBase
    {
        public SaveModuleBase() { }
        public abstract void Save<T>(string key, T value, SMSettings settings);
        public abstract T Load<T>(string key, T defaultValue, SMSettings settings);
        public abstract void Delete(string key, SMSettings settings);
        public abstract void DeleteAll(SMSettings settings);
        protected abstract void CreateCacheFile(SMSettings settings);
        public abstract void SaveCacheToDisk(SMSettings settings);
        public abstract void CreateBackup(SMSettings settings);
        public abstract void RestoreBackup(SMSettings settings);
    }
}
