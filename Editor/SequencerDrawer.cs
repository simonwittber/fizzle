using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Sequencer))]
    public class SequencerDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            var rect = position;
            rect.width /= 2;
            rect.x += rect.width;
            EditorGUIUtility.labelWidth = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("code"), new GUIContent("S", "Sequence"));
            rect.x -= rect.width;
            DrawInputProperties(rect, property, "type", "envelope", "bpm", "glide", "frequencyMultiply", "transpose", "outputEnvelope");
        }
    }
}