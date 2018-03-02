using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(DelayLine))]
    public class DelayLineDrawer : FizzleDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 16;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height += 1;
            Handles.DrawSolidRectangleWithOutline(position, Color.white * 0.125f, Color.white * 0.5f);
            // EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 16;
            DrawOutputJacks(position, property);
            position.width -= 32;
            var rect = position;
            rect.height = 16;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("input"), GUIContent.none);
            rect.x += rect.width;
            rect.width = (position.width - 32) / 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("feedback"), new GUIContent("f", "Feedback"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("delay"), new GUIContent("D", "delay"));
        }
    }
}