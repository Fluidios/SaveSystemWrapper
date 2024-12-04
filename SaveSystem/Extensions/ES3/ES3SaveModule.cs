using Game.SaveSystem.Core;

namespace Game.SaveSystem.ES3Extension
{
    public class ES3SaveModule : SaveModuleBase
    {
        public override void CreateBackup(SMSettings settings)
        {
            ES3.CreateBackup(string.Format("SLOT_{0}/{1}",settings.SaveSlot, settings.FilePath));
        }

        public override void Delete(string key, SMSettings settings)
        {
            ES3.DeleteKey(key, new ES3Settings(string.Format("SLOT_{0}/{1}",settings.SaveSlot, settings.FilePath))
            {
                location = settings.SaveLocation == SMSettings.Location.Cache ? ES3.Location.Cache : ES3.Location.File
            });
        }

        public override void DeleteAll(SMSettings settings)
        {
            if(string.IsNullOrEmpty(settings.FilePath))
            {
                ES3.DeleteDirectory(new ES3Settings(string.Format("SLOT_{0}",settings.SaveSlot)));
            }
            else
            {
                ES3.DeleteFile(new ES3Settings(string.Format("SLOT_{0}/{1}",settings.SaveSlot, settings.FilePath)));
            }
        }

        public override T Load<T>(string key, T defaultValue, SMSettings settings)
        {
            return ES3.Load(key, defaultValue: defaultValue, new ES3Settings(string.Format("SLOT_{0}/{1}",settings.SaveSlot, settings.FilePath))
            {
                location = settings.SaveLocation == SMSettings.Location.Cache ? ES3.Location.Cache : ES3.Location.File
            });
        }

        public override void RestoreBackup(SMSettings settings)
        {
            if(!ES3.RestoreBackup(string.Format("SLOT_{0}/{1}",settings.SaveSlot, settings.FilePath)))
            {
                SMLogger.LogError("Backup file not found.");
            }
        }

        public override void Save<T>(string key, T value, SMSettings settings)
        {
            ES3.Save(key, value, new ES3Settings(string.Format("SLOT_{0}/{1}",settings.SaveSlot, settings.FilePath))
            {
                location = settings.SaveLocation == SMSettings.Location.Cache ? ES3.Location.Cache : ES3.Location.File
            });
        }

        public override void SaveCacheToDisk(SMSettings settings)
        {
            ES3.StoreCachedFile(string.Format("SLOT_{0}/{1}",settings.SaveSlot, settings.FilePath));
        }

        protected override void CreateCacheFile(SMSettings settings)
        {
            ES3.CacheFile(string.Format("SLOT_{0}/{1}",settings.SaveSlot, settings.FilePath));
        }
    }
}
