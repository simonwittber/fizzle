using UnityEditor;
using UnityEngine;

namespace Fizzle
{


    [CustomPropertyDrawer(typeof(Mixer))]
    public class MixerDrawer : FizzleDrawer
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
            rect.width = position.width / 13;
            var w = rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputA"), new GUIContent("1", "Input"));
            rect.x += rect.width;
            rect.width = w * 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gainA"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputB"), new GUIContent("2", "Input"));
            rect.x += rect.width;
            rect.width = w * 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gainB"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputC"), new GUIContent("3", "Input"));
            rect.x += rect.width;
            rect.width = w * 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gainC"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputD"), new GUIContent("4", "Input"));
            rect.x += rect.width;
            rect.width = w * 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gainD"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w;
            rect.x = position.width + 16;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("output"), GUIContent.none);

        }
    }
}