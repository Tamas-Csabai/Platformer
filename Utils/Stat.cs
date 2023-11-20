
using UnityEngine;

/*
#if UNITY_EDITOR
using UnityEditor;
#endif
*/

namespace Main
{
    [System.Serializable]
    public class Stat<T>
    {

        public System.Action<T> OnChanged;
        public System.Action<T> OnMaxChanged;

        [SerializeField] private T _value;
        [SerializeField] private T _maxValue;

        public T Value
        {
            get => _value;

            set
            {
                _value = value;

                OnChanged?.Invoke(_value);
            }
        }

        public T MaxValue
        {
            get => _maxValue;

            set
            {
                _maxValue = value;

                OnMaxChanged?.Invoke(_maxValue);
            }
        }

        public void Reset()
        {
            Value = MaxValue;
        }

    }
    
    /*
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Stat<int>))]
    public class Stat_Drawer : PropertyDrawer
    {

        private SerializedProperty _valueProperty;
        private SerializedProperty _maxValueProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _valueProperty = property.FindPropertyRelative("_value");
            _maxValueProperty = property.FindPropertyRelative("_maxValue");
        }
    }
#endif
    */

}
