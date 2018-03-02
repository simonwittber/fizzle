using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Filter))]
    public class FilterDrawer : FizzleDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 18;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // position.height += 1;
            Handles.DrawSolidRectangleWithOutline(position, Color.white * 0.125f, Color.white * 0.5f);
            // EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.indentLevel = 0;
            // position.width = EditorGUIUtility.currentViewWidth;
            EditorGUIUtility.labelWidth = 16;
            DrawOutputJacks(position, property);
            position.width -= 32;
            var rect = position;
            rect.height = position.height - 2;
            rect.y += 1;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("input"), GUIContent.none);
            rect.x += rect.width;
            rect.width = 64;
            var typeProperty = property.FindPropertyRelative("type");
            EditorGUI.PropertyField(rect, typeProperty, GUIContent.none);
            rect.x += rect.width;

            var filterType = (Filter.FilterType)(typeProperty.enumValueIndex);
            if (filterType == Filter.FilterType.Waveshaper)
            {
                rect.width = 32;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("waveshaper"), new GUIContent("W", "Waveshaper Curve"));
            }
            else
            {
                rect.width = 128;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("cutoff"), new GUIContent("F", "Cutoff Freqeuncy"));
                rect.x += rect.width;
                rect.width = 128;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("q"), new GUIContent("Q", "Resonance"));
            }

        }
    }
}