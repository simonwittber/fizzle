using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Macros))]
    public class MacrosDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawInputProperties(position, property, "macroA", "macroB", "macroC", "macroD", "macroE");
        }
    }
}