using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(JackSignal))]
    public class JackSignalDrawer : JackDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // jackIns.Add(property);
            base.OnGUI(position, property, label);
        }

        protected override bool DrawJackButton(Rect rect, int id)
        {
            if (id != 0)
            {
                jackRects[rect] = id;
            }
            return GUI.Button(rect, new GUIContent("", id.ToString()), "radio");
        }

        protected override SerializedProperty GetIdProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative("connectedId");
        }

        protected override void OnClick(SerializedProperty property)
        {
            hotJackIn = property;
            hotJackCurveSrc = Event.current.mousePosition;
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