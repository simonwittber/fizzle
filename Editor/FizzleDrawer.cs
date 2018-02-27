using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    public class FizzleDrawer : PropertyDrawer
    {
        public static OscDrawer hotOut;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && position.Contains(Event.current.mousePosition))
            {
                OnContextMenu(position, property, label);
            }
        }

        public virtual void OnContextMenu(Rect position, SerializedProperty property, GUIContent label)
        {

        }
    }
}