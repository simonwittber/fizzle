using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    // [CustomPropertyDrawer(typeof(Wave))]
    public class WaveDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 66;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.indentLevel = 0;
            EditorGUI.DrawRect(position, new Color(0.35f, 0.35f, 0.35f, 1));
            EditorGUI.PrefixLabel(position, label);

            var shape = property.FindPropertyRelative("shape");
            var frequency = property.FindPropertyRelative("frequency");
            var amplitude = property.FindPropertyRelative("amplitude");
            var bias = property.FindPropertyRelative("bias");
            var rect = position;
            rect.y += 16;
            rect.x += 1;
            rect.width = 48;
            rect.height = 47;
            shape.animationCurveValue = EditorGUI.CurveField(rect, GUIContent.none, shape.animationCurveValue, Color.green, new Rect(0, 0, 1, 1));
            rect.height = 16;
            rect.x += rect.width;
            rect.width = position.width - rect.width;
            EditorGUIUtility.labelWidth = 16;
            EditorGUI.PropertyField(rect, amplitude, new GUIContent("A", "Amplitude"));
            rect.y += 16;
            EditorGUI.PropertyField(rect, frequency, new GUIContent("F", "Frequency"));
            rect.y += 16;
            EditorGUI.PropertyField(rect, bias, new GUIContent("B", "Bias"));
        }
    }
}