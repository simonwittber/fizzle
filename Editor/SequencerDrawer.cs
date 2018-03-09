using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Sequencer))]
    public class SequencerDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property, "outputEnvelope", "outputTrigger");
            var rect = position;
            rect.width /= 2;
            DrawInputProperties(rect, property, "type", "envelope", "gate", "glide", "frequencyMultiply", "transpose");
            rect.x += rect.width;
            DrawInputProperties(rect, property, "code");
        }
    }
}