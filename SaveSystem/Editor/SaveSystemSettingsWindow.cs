using System.Collections;
using System.Collections.Generic;
using Game.SaveSystem.Core;
using UnityEditor;
using UnityEngine;

namespace Game.SaveSystem.Editor
{
    public class SaveSystemSettingsWindow : EditorWindow
    {
        public SMConfig Config { get; private set; }

        [UnityEditor.MenuItem("Tools/SaveManager/Settings")]
        public static void OpenInspector()
        {
            var config = AssetDatabase.LoadAssetAtPath<SMConfig>(
                AssetDatabase.GUIDToAssetPath(
                    AssetDatabase.FindAssets("t:SMConfig")[0]
                ));

            SaveSystemSettingsWindow window = GetWindow<SaveSystemSettingsWindow>("Save System Settings");
            window.Config = config;
            window.Show();
        }

        protected virtual void OnGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Config file:", Config, typeof(SMConfig), false);
            GUI.enabled = true;

            GUILayout.Label("Save System Settings");

            ScriptableObject target = Config;
            SerializedObject so = new (target);
            SerializedProperty defaultSaveFileProperty = so.FindProperty("_defaultSaveFile");
            SerializedProperty useBackupFilesProtectionProperty = so.FindProperty("_useBackupFilesProtection");
            SerializedProperty activeSaveModuleProperty = so.FindProperty("_activeSaveModule");

            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.PropertyField(defaultSaveFileProperty, true);
            EditorGUILayout.PropertyField(useBackupFilesProtectionProperty, true);
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(activeSaveModuleProperty.FindPropertyRelative("TypeName"), true);
            EditorGUILayout.PropertyField(activeSaveModuleProperty.FindPropertyRelative("TypeAssembly"), true);
            EditorGUILayout.PropertyField(activeSaveModuleProperty.FindPropertyRelative("ParentTypeName"), true);
            EditorGUILayout.PropertyField(activeSaveModuleProperty.FindPropertyRelative("ParentTypeAssembly"), true);
            GUI.enabled = true;
            EditorGUILayout.PropertyField(activeSaveModuleProperty, true);

            if(GUILayout.Button("Reset Active Save Module"))
            {
                Config.ResetActiveSaveModule();
            }

            if(EditorGUI.EndChangeCheck())
            {
                if(!defaultSaveFileProperty.stringValue.EndsWith(".json"))
                {
                    defaultSaveFileProperty.stringValue = defaultSaveFileProperty.stringValue + ".json";
                }
                so.ApplyModifiedProperties();
            }
        }
    }
}
