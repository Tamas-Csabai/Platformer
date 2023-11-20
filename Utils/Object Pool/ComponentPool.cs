using UnityEngine;

namespace Main
{
    public class ComponentPool : MonoBehaviour
    {

        [SerializeField] private Component prefab;
        [SerializeField] private int bufferComponenetsCount = 0;
        [SerializeField] private Component[] preBufferedComponenets;

        private ObjectPool<Component> _objectPool;

        private void Awake()
        {
            _objectPool = new ObjectPool<Component>(prefab, CreateObject);

            if(preBufferedComponenets != null)
            {
                for (int i = 0; i < preBufferedComponenets.Length; i++)
                    Add(preBufferedComponenets[i]);
            }

            BufferComponenets(bufferComponenetsCount);
        }

        private Component CreateObject()
        {
            Component newComponent = Instantiate(prefab);
            newComponent.gameObject.AddComponent<ComponentPoolElement>().Initialize(this, newComponent);
            newComponent.transform.SetParent(transform, false);
            newComponent.gameObject.SetActive(false);
            return newComponent;
        }

        public void BufferComponenets(int bufferCount)
        {
            for (int i = 0; i < bufferCount; i++)
                _objectPool.AddNew();
        }

        public void SetPrefab(Component prefab)
        {
            this.prefab = prefab;
        }

        public void Remove(Component component)
        {
            _objectPool.Remove(component);
        }

        public Component Get()
        {
            Component component = _objectPool.Get();
            component.gameObject.SetActive(true);
            return component;
        }

        public void Return(Component component)
        {
            component.transform.SetParent(transform);
            component.gameObject.SetActive(false);
            _objectPool.Return(component);
        }

        public void Add(Component component)
        {
            component.gameObject.AddComponent<ComponentPoolElement>().Initialize(this, component);
            component.transform.SetParent(transform, false);
            component.gameObject.SetActive(false);
            _objectPool.Add(component);
        }

    }
}
