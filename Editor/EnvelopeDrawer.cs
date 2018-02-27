using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Envelope))]
    public class EnvelopeDrawer : FizzleDrawer
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
            var rect = position;

            rect.height = position.height - 2;
            rect.y += 1;
            rect.width = 64;
            var typeProperty = property.FindPropertyRelative("type");
            EditorGUI.PropertyField(rect, typeProperty, GUIContent.none);
            rect.x += rect.width;
            rect.width = 16;
            using (new EditorGUI.DisabledGroupScope(typeProperty.enumValueIndex > 0))
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("shape"), GUIContent.none);
            rect.x += rect.width;
            rect.width = (position.width - 64 - 16 - 16) / 3;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("frequency"), new GUIContent("L", "Length"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gain"), new GUIContent("A", "Amplitude"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("bias"), new GUIContent("B", "Bias"));
            rect.x += rect.width;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("output"), GUIContent.none);
        }
    }
}