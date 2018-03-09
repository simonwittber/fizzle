using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Envelope))]
    public class EnvelopeDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            DrawInputProperties(position, property, "type", "shape", "frequency", "detune", "phaseOffset", "duty");
        }
    }
}