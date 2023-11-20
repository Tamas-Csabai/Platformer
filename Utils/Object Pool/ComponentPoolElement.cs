using UnityEngine;

namespace Main
{
    public class ComponentPoolElement : MonoBehaviour
    {

        private ComponentPool _componentPool;
        private Component _component;

        public void Initialize(ComponentPool componentPool, Component component)
        {
            _componentPool = componentPool;
            _component = component;
        }

        private void OnDestroy()
        {
            _componentPool.Remove(_component);
        }

    }
}
