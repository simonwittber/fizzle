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

        protected override bool DrawJackButton(Rect rect, int id, SerializedProperty property)
        {
            if (id != 0)
            {
                jackRects[rect] = id;
            }
            Handles.color = Color.red;
            if (property.FindPropertyRelative("xMulMinusOne").boolValue)
                Handles.DrawSolidDisc(rect.center - new Vector2(1, -1), Vector3.forward, 8);
            Handles.color = Color.white;
            if (property.FindPropertyRelative("oneMinusX").boolValue)
                Handles.DrawSolidDisc(rect.center - new Vector2(1, -1), Vector3.forward, 7);

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

        protected override void OnContext(SerializedProperty property)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("= X * -1"), property.FindPropertyRelative("xMulMinusOne").boolValue, () =>
            {
                property.FindPropertyRelative("xMulMinusOne").boolValue = !property.FindPropertyRelative("xMulMinusOne").boolValue;
                property.serializedObject.ApplyModifiedProperties();
            });
            menu.AddItem(new GUIContent("= 1 - X"), property.FindPropertyRelative("oneMinusX").boolValue, () =>
            {
                property.FindPropertyRelative("oneMinusX").boolValue = !property.FindPropertyRelative("oneMinusX").boolValue;
                property.serializedObject.ApplyModifiedProperties();
            });

            menu.ShowAsContext();
        }
    }
}