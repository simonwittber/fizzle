using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(JackMacro))]
    public class JackMacroDrawer : JackOutDrawer
    {


        protected override void DrawSubProperties(Rect rect, SerializedProperty property)
        {
            var idProperty = GetIdProperty(property);
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("localValue"), GUIContent.none);
        }

    }
}