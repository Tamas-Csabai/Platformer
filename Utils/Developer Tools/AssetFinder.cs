
#if UNITY_EDITOR

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Editor
{
    public class AssetFinder : EditorWindow
    {

        private static AssetFinder instance;

        [MenuItem("Window/Developer Tool/Asset Finder")]
        public static void ShowWindow()
        {
            instance = GetWindow<AssetFinder>("AssetFinder", true);
            instance.position = new Rect(Screen.width / 2f - 250f, Screen.height / 2f - 250f, 500f, 500f);
        }

        private List<Object> assets = new();
        private Vector2 assetsScrollPosition;

        private void OnGUI()
        {
            if(GUILayout.Button("Find All"))
            {
                FindAll();
            }

            if(assets.Count == 0)
            {
                GUILayout.Label("None", EditorStyles.centeredGreyMiniLabel);
            }
            else
            {
                assetsScrollPosition = EditorGUILayout.BeginScrollView(assetsScrollPosition);

                foreach (GameObject asset in assets)
                {
                    EditorGUILayout.ObjectField(asset.name, asset, typeof(GameObject), false);
                }

                EditorGUILayout.EndScrollView();
            }
        }

        [Button]
        public void FindAll()
        {
            
        } 

    }
}

#endif