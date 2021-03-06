using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Filter))]
    public class FilterDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            DrawInputProperties(position, property, "input", "type", "waveshaper", "cutoff", "q");
        }
    }
}