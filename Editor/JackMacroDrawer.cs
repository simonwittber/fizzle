using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(JackMacro))]
    public class JackMacroDrawer : JackOutDrawer
    {


        protected override void DrawSubProperties(Rect rect, SerializedProperty property)
        {
            // var idProperty = GetIdProperty(property);
            rect.width /= 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("name"), GUIContent.none);
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("localValue"), GUIContent.none);
        }

    }
}