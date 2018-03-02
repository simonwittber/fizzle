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
            DrawOutputJacks(position, property);
            position.width -= 32;
            var rect = position;
            rect.height = 16;
            var w = 16;
            var space = (rect.width - (w * 16) - 16) / 4;
            rect.width = w;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputA"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w * 3 + space;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gainA"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputB"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w * 3 + space;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gainB"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputC"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w * 3 + space;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gainC"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputD"), GUIContent.none);
            rect.x += rect.width;
            rect.width = w * 3 + space;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gainD"), GUIContent.none);



        }
    }
}