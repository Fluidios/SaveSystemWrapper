using System;
using System.Collections.Generic;
using System.Reflection;
using Game.SaveSystem.Core;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game.SaveSystem.Editor
{
    [CustomPropertyDrawer(typeof(DrawableType<>))]
    public class DrawableTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var typeProperty = property.FindPropertyRelative("TypeName");
            if (GUI.Button(position, typeProperty.stringValue))
            {
                DrawableTypeContentProvider provider = ScriptableObject.CreateInstance<DrawableTypeContentProvider>();
                    LoadAllDerivedTypes(
                        property.FindPropertyRelative("ParentTypeName").stringValue,
                        property.FindPropertyRelative("ParentTypeAssembly").stringValue,
                        out var keys,
                        out var assemblies
                    );
                    if (string.IsNullOrEmpty(typeProperty.stringValue) && !keys.Contains(typeProperty.stringValue)) provider.DefaultMessage = "Current Type is not exist!";
                    provider._paths = keys;
                    provider.CloseWhenAudioSelected = true;
                    provider._onAudioKeySelectedCallback = SetKey;
                    SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);

                    void SetKey(string key)
                    {
                        var keyProperty = property.FindPropertyRelative("TypeName");
                        keyProperty.stringValue = key;
                        var assemblyProperty = property.FindPropertyRelative("TypeAssembly");
                        assemblyProperty.stringValue = assemblies[key].FullName;
                        property.serializedObject.ApplyModifiedProperties();
                    }
            }

            EditorGUI.EndProperty();
        }
        private void LoadAllDerivedTypes(string parentTypeName, string parentTypeAssembly, out List<string> keys, out Dictionary<string, Assembly> assemblies)
        {
            keys = new ();
            assemblies = new ();
            Type parentType = Type.GetType($"{parentTypeName}, {parentTypeAssembly}");

            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(parentType))
                    {
                        keys.Add(type.FullName);
                        assemblies.Add(keys[^1], assembly);
                    }
                }
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
