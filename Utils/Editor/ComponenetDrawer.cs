#if UNITY_EDITOR

namespace Main.Editor
{

    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(Component), true)]
    public class ComponenetDrawer : PropertyDrawer
    {
        private Rect buttonsPos;
        private GUIContent buttonContent = new GUIContent(EditorGUIUtility.IconContent("d_Search Icon"));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            buttonsPos = position;

            position.xMax = position.width - 5f;
            EditorGUI.PropertyField(position, property, label, true);

            buttonsPos.xMin = position.xMax;
            buttonsPos.x = position.xMax;
            buttonsPos.width = 25f;
            if (GUI.Button(buttonsPos, buttonContent))
            {
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                property.objectReferenceValue = ((Component)property.serializedObject.targetObject).GetComponentInChildren(fieldInfo.FieldType);
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
        }
    }
}

#endif