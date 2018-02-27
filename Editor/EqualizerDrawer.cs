using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Equalizer))]
    public class EqualizerDrawer : FizzleDrawer
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
            var rect = position;
            rect.height = 16;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("input"), GUIContent.none);
            rect.x += rect.width;
            rect.width = (position.width - 32) / 5;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("lg"), new GUIContent("L", "Low Gain"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("mg"), new GUIContent("M", "Mid Gain"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("hg"), new GUIContent("H", "High Gain"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("lowFreq"), new GUIContent("A", "Low Freq"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("highFreq"), new GUIContent("B", "High Freq"));
            rect.x += rect.width;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("output"), GUIContent.none);
        }
    }
}