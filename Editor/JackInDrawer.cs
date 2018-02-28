using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(JackIn))]
    public class JackInDrawer : JackSignalDrawer
    {


        protected override void DrawSubProperties(Rect rect, SerializedProperty property)
        {
            var idProperty = GetIdProperty(property);
            using (new EditorGUI.DisabledScope(idProperty.intValue != 0))
            {
                // var min = property.FindPropertyRelative("minRange").floatValue;
                // var max = property.FindPropertyRelative("maxRange").floatValue;
                // EditorGUI.Slider(rect, property.FindPropertyRelative("localValue"), min, max, GUIContent.none);
                EditorGUI.indentLevel = 0;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("localValue"), GUIContent.none);
            }
        }


    }
}