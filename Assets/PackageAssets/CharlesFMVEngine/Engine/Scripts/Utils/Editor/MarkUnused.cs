﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CharlesEngine
{
    public class MarkUnused
    {
        private const string UnusedLabel = "Unused"; // All unused assets will be tagged with this label
        
        [MenuItem("Tools/Charles Engine/Tools/Mark Unused Assets", false, 59)]
        private static void MarkUnusedAssets()
        {
            EditorUtility.DisplayProgressBar("Searching assets", "Finding dependencies", 0.1f);

            // find all scene GUIDs
            var allScenes = AssetDatabase.FindAssets("t:scene");
            // translate GUIDs to paths
            var allScenesPaths = allScenes.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            // gather all dependencies (once only)
            var dependencies = new HashSet<string>(AssetDatabase.GetDependencies(allScenesPaths));

            // list all types you are interested in
            var allItems = new List<string>(AssetDatabase.FindAssets("t:Sprite t:VideoClip t:AudioClip"));

            var unusedCounter = 0;
            var progress = 0;
            foreach (var guid in allItems)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var loadedAsset = AssetDatabase.LoadMainAssetAtPath(path);
                var isUsed = dependencies.Contains(path);

                LabelAsset(loadedAsset, isUsed);

                if (!isUsed)
                {
                    unusedCounter++;
                }

                progress++;
                EditorUtility.DisplayProgressBar("Searching assets", "Labeling assets",
                    (float) progress / allItems.Count * 0.9f + 0.1f);
            }

            AssetDatabase.Refresh();
            Debug.Log("Found " + unusedCounter + " unused assets");

            // Set search in Project browser to show all unused assets
            try
            {
                var projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
                var window = EditorWindow.GetWindow(projectBrowserType);
                MethodInfo setSearchMethodInfo = projectBrowserType.GetMethod("SetSearch",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                    null, new[] {typeof(string)}, null);
                setSearchMethodInfo.Invoke(window, new object[] {"l:" + UnusedLabel});
            }
            catch
            {
                // this might possibly break in the future
            }

            EditorUtility.ClearProgressBar();
        }

        // Labels asset as unused or removes the label if used
        private static void LabelAsset(UnityEngine.Object asset, bool isUsed)
        {
            var labelList = new List<string>(AssetDatabase.GetLabels(asset));
            bool changed = false;
            if (labelList.Contains(UnusedLabel) && isUsed)
            {
                labelList.Remove(UnusedLabel);
                changed = true;
            }
            else if (!labelList.Contains(UnusedLabel) && !isUsed)
            {
                labelList.Add(UnusedLabel);
                changed = true;
            }

            if (changed)
            {
                AssetDatabase.SetLabels(asset, labelList.ToArray());
                EditorUtility.SetDirty(asset);
            }
        }

 
   private static IEnumerable<Scene> GetSavedScenes() {
            string[] guids = AssetDatabase.FindAssets("t:Scene");
            foreach (string guid in guids) {
                yield return EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(guid));
            }
        }
    }
}