using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Sequencer))]
    public class SequencerDrawer : FizzleDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 16;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height += 1;
            Handles.DrawSolidRectangleWithOutline(position, Color.white * 0.125f, Color.white * 0.5f);
            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 16;
            var rect = position;
            rect.height = 16;
            rect.width = 64;
            var typeProperty = property.FindPropertyRelative("type");
            EditorGUI.PropertyField(rect, typeProperty, GUIContent.none);
            rect.x += rect.width;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("envelope"), GUIContent.none);
            rect.x += rect.width;
            rect.width = 64;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("frequency"), new GUIContent("L", "Length"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gain"), new GUIContent("A", "Amplitude"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("bias"), new GUIContent("B", "Bias"));
            rect.x += rect.width;
            rect.width = (position.xMax - rect.xMax) + 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("code"), new GUIContent("I", "Instructions"));
            rect.x = position.width;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("envelopeOutput"), GUIContent.none);
            rect.x = position.width + 16;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("output"), GUIContent.none);
        }
    }
}