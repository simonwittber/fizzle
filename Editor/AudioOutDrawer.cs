using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(AudioOut))]
    public class AudioOutDrawer : FizzleDrawer
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
            rect.width = 64;
            EditorGUI.LabelField(rect, label);
            rect.x += rect.width;
            rect.height = 16;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("left"), new GUIContent("L", "Left"));
            rect.x += rect.width + 32;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("right"), new GUIContent("R", "Right"));
            rect.x += rect.width + 32;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("monitor"), new GUIContent("M", "Send to Monitor"));

        }
    }
}