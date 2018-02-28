using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    public class FizzleDrawer : PropertyDrawer
    {
        public static OscDrawer hotOut;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

        }

        public virtual void OnContextMenu(Rect position, SerializedProperty property, GUIContent label)
        {

        }
    }
}