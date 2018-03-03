using UnityEditor;
using UnityEngine;

namespace Fizzle
{

    [CustomPropertyDrawer(typeof(Mixer))]
    public class MixerDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            DrawInputProperties(position, property, "inputA", "gainA", "inputB", "gainB", "inputC", "gainC", "inputD", "gainD");
        }
    }
}