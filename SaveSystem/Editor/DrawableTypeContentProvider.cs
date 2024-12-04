using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game.SaveSystem.Editor
{
    public class DrawableTypeContentProvider  : ScriptableObject, ISearchWindowProvider
    {
        public string DefaultMessage = "Choose key";
            public bool CloseWhenAudioSelected = true;
            public List<string> _paths;
            public Action _onAudioKeyResetCallback;
            public Action<string> _onAudioKeySelectedCallback;

            public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
            {
                List<SearchTreeEntry> searchTree = new List<SearchTreeEntry>();
                searchTree.Add(new SearchTreeGroupEntry(new GUIContent(DefaultMessage), 0));

                if(_onAudioKeyResetCallback != null)
                {
                    SearchTreeEntry resetEntry = new SearchTreeEntry(new GUIContent("RESET"));
                    resetEntry.level = 1;
                    resetEntry.userData = "RESET";
                    searchTree.Add(resetEntry);
                }
                
                _paths.Sort((a, b) =>
                {
                    string[] splitsOfA = a.Split('/');
                    string[] splitsOfB = b.Split("/");
                    for (int i = 0; i < splitsOfA.Length; i++)
                    {
                        if (i >= splitsOfB.Length)
                        {
                            return 1;
                        }
                        int value = splitsOfA[i].CompareTo(splitsOfB[i]);
                        if (value != 0)
                        {
                            //Make sure that child folders come before items in the parent folder
                            if (splitsOfA.Length != splitsOfB.Length && (i == splitsOfA.Length - 1 || i == splitsOfB.Length - 1))
                                return splitsOfA.Length < splitsOfB.Length ? 1 : -1;

                            return value;
                        }
                    }

                    return 0;
                });

                List<string> groups = new List<string>();
                foreach (var item in _paths)
                {
                    string[] entryTitle = item.Split('.');
                    //Create entry
                    SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()));
                    entry.level = 1;
                    entry.userData = entryTitle.Length > 1 ? item.Substring(0, item.Length - entryTitle.Last().Length - 1) : null;
                    searchTree.Add(entry);
                }

                return searchTree;
            }

            public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
            {
                if((string)SearchTreeEntry.userData == "RESET") {
                    _onAudioKeyResetCallback?.Invoke();
                    return true;
                }
                else
                {
                    _onAudioKeySelectedCallback?.Invoke(string.Format("{0}.{1}" ,SearchTreeEntry.userData, SearchTreeEntry.name));
                }
                return CloseWhenAudioSelected;
            }
    }
}