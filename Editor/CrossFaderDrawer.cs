using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(CrossFader))]
    public class CrossFaderDrawer : FizzleDrawer
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
            rect.width = 32;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("ramp"), new GUIContent("P", "Position Curve"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("quant"), new GUIContent("Q", "Quantize Position"));
            rect.x += rect.width;
            rect.width = 64;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("position"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gain"), GUIContent.none);
            rect.x += rect.width;
            rect.width = (position.xMax - rect.x) / 8;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputA"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputB"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputC"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputD"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputE"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputF"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputG"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("inputH"), GUIContent.none);
        }
    }
}