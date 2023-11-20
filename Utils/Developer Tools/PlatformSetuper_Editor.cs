using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Main.Editor
{
    public class PlatformSetuper_Editor : MonoBehaviour
    {

        [SerializeField] private bool isStaticObject = false;
        [SerializeField] private Material assignMaterial;

        [Header("Collider")]
        [SerializeField] private bool addBoxCollider = false;
        [SerializeField] private bool addSphereCollider = false;
        [SerializeField] private bool addMeshCollider = false;

#if UNITY_EDITOR

        [Button]
        private void SetupChildrenPlatforms()
        {
            GameObject[] gameObjects = new GameObject[transform.childCount];

            int index = 0;
            foreach (Transform child in transform)
            {
                gameObjects[index] = child.gameObject;
                index++;
            }

            SetupPlatforms(gameObjects);
        }

        [Button]
        private void SetupSelectedPlatforms()
        {
            SetupPlatforms(Selection.gameObjects);
        }

        private void SetupPlatforms(GameObject[] selectedObjects)
        {
            if(selectedObjects == null || selectedObjects.Length == 0)
            {
                Debug.LogError("<b>Null or Empty objects!");
                return;
            }

            for (int i = 0; i < selectedObjects.Length; i++)
            {
                EditorUtility.SetDirty(selectedObjects[i]);

                GameObject rootObject = new GameObject(selectedObjects[i].name);
                EditorUtility.SetDirty(rootObject);
                EditorUtility.SetDirty(rootObject.transform);
                rootObject.transform.SetPositionAndRotation(selectedObjects[i].transform.position, selectedObjects[i].transform.rotation);
                rootObject.isStatic = isStaticObject;

                selectedObjects[i].transform.SetParent(rootObject.transform, true);
                selectedObjects[i].transform.ResetLocal();
                selectedObjects[i].isStatic = isStaticObject;

                if (addBoxCollider)
                    selectedObjects[i].AddComponent<BoxCollider>();

                if (addSphereCollider)
                    selectedObjects[i].AddComponent<SphereCollider>();

                if (addMeshCollider)
                    selectedObjects[i].AddComponent<MeshCollider>();

                if(assignMaterial != null)
                {
                    MeshRenderer meshRenderer = selectedObjects[i].GetComponent<MeshRenderer>();
                    EditorUtility.SetDirty(meshRenderer);
                    meshRenderer.material = assignMaterial;
                    meshRenderer.staticShadowCaster = isStaticObject;
                    EditorUtility.SetDirty(meshRenderer);
                }

                EditorUtility.SetDirty(selectedObjects[i]);
                EditorUtility.SetDirty(rootObject);
                EditorUtility.SetDirty(rootObject.transform);

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
        }

#endif

    }
}
