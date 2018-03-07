using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(SynthBase), true)]
    public class SynthBaseDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            DrawInputProperties(position, property, "gate", "frequency");
        }
    }

    [CustomPropertyDrawer(typeof(KarplusStrong))]
    public class KarplusStrongDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            DrawInputProperties(position, property, "gate", "frequency");
        }
    }

    [CustomPropertyDrawer(typeof(Perc))]
    public class PercDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            DrawInputProperties(position, property, "gate", "frequency", "noise", "noiseEnvelope", "type", "waveshaper", "cutoff", "q", "amplitudeEnvelope", "pitchEnvelope");
        }
    }
}