using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(JackIn))]
    public class JackInDrawer : JackDrawer
    {

        protected override bool DrawJackButton(Rect rect, int id)
        {
            if (id != 0)
                jackRects[rect] = id;
            return GUI.Button(rect, GUIContent.none, "radio");
        }

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

        protected override SerializedProperty GetIdProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative("connectedId");
        }

        protected override void OnClick(SerializedProperty property)
        {
            hotJackCurveSrc = Event.current.mousePosition;
            hotJackIn = property;
            Connect();
            property.serializedObject.ApplyModifiedProperties();
        }

        protected override void OnReset(SerializedProperty property)
        {
            var idProperty = GetIdProperty(property);
            idProperty.intValue = 0;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}