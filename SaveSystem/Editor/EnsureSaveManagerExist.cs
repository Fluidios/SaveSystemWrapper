using UnityEditor;
using UnityEngine;

namespace Game.SaveSystem.Editor
{
    public class EnsureSaveManagerExist : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
            {
                
                var configs = AssetDatabase.FindAssets("t:SMConfig");
                if(configs.Length > 1) 
                {
                    Debug.LogError("There are multiple SMConfig assets in the project. Please remove the duplicates.");
                    foreach(var config in configs)
                    {
                        Debug.LogError(AssetDatabase.GUIDToAssetPath(config));
                    }
                    return;
                }
                else if(configs.Length == 0)
                {
                    Debug.LogWarning("No SMConfig asset found. Creating one...");
                    var config = ScriptableObject.CreateInstance<Game.SaveSystem.Core.SMConfig>();

                    var pathToCoreAssembly = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("Game.SaveSystem.Core")[0]);
                    var parentFolder = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(pathToCoreAssembly));
                    var resourcesDirPath = parentFolder + "/Resources";
                    if(!System.IO.Directory.Exists(resourcesDirPath))
                    {
                        System.IO.Directory.CreateDirectory(resourcesDirPath);
                        Debug.Log($"Created Resources folder at {resourcesDirPath}");
                    }
                    var configPath = System.IO.Path.Combine(resourcesDirPath, "SMConfig.asset");
                    AssetDatabase.CreateAsset(config, configPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    Debug.Log($"Created SMConfig asset at {configPath}");

                }
            }
    }
}