using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    public class FizzleDrawer : PropertyDrawer
    {
        public static OscDrawer hotOut;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

        }

        public virtual void OnContextMenu(Rect position, SerializedProperty property, GUIContent label)
        {

        }

        public virtual void DrawOutputJacks(Rect position, SerializedProperty property)
        {
            var rect = position;
            rect.height = 16;
            rect.x = (position.width - 16);
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("multiply"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("add"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("output"), GUIContent.none);
        }
    }
}