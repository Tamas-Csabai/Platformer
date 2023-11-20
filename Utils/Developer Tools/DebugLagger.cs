using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class DebugLagger : MonoBehaviour
    {

        [SerializeField] private int instantiateCount = 100000;

#if UNITY_EDITOR
        private List<GameObject> _debugObjects = new List<GameObject>();

        [Button]
        private void Instantiate()
        {
            GameObject go = new GameObject("= Debug Object =");
            _debugObjects.Add(go);

            for (int i = 0; i < instantiateCount; i++)
            {
                _debugObjects.Add(Instantiate(go));
            }

            DestroyAll();
        }

        private void DestroyAll()
        {
            foreach (GameObject obj in _debugObjects)
            {
                Destroy(obj);
            }
        }
#endif

    }
}
