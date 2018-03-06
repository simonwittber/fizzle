using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Ladder))]
    public class LadderDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            DrawInputProperties(position, property, "gate", "ladder");
        }
    }
}