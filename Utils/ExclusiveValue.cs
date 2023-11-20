using System;

namespace Main
{
    public abstract class ExclusiveValue<T> where T : class
    {
        protected T _current;

        public Action<T> OnChangeCurrent;

        public T Current
        {
            get => _current;

            set
            {
                if (_current == value)
                    return;

                if (_current != null)
                    OnNotCurrent();

                _current = value;

                if(value != null)
                    OnCurrent();

                OnChangeCurrent?.Invoke(value);
            }
        }

        protected abstract void OnCurrent();

        protected abstract void OnNotCurrent();
    }
}
