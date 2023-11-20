using System.Collections.Generic;

namespace Main
{
    public class ObjectPool<T>
    {

        public delegate T CreateObject();

        protected T _prefab;
        protected LinkedList<T> _objects = new LinkedList<T>();
        protected CreateObject _createObject;

        public ObjectPool(T prefab, CreateObject createObjectMethod)
        {
            _prefab = prefab;
            _createObject = createObjectMethod;
        }

        public void Return(T obj)
        {
            _objects.AddLast(obj);
        }

        public T Get()
        {
            if (_objects.Count == 0)
                AddNew();

            T obj = _objects.First.Value;

            _objects.RemoveFirst();

            return obj;
        }

        public void Remove(T obj)
        {
            _objects.Remove(obj);
        }

        public void Add(T obj)
        {
            _objects.AddLast(obj);
        }

        public void AddNew()
        {
            _objects.AddLast(_createObject.Invoke());
        }

    }
}
