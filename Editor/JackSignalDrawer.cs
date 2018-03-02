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
            return GUI.Button(rect, new GUIContent("", property.displayName), "radio");
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
            CreateToggleMenuItem(menu, "Transform/= X * -1", property, "xMulMinusOne");
            CreateToggleMenuItem(menu, "Transform/= 1 - X", property, "oneMinusX");
            CreateToggleMenuItem(menu, "Gain/Half", property, "attenuate");
            CreateToggleMenuItem(menu, "Gain/Double", property, "amplify");
            menu.ShowAsContext();
        }

        void CreateToggleMenuItem(GenericMenu menu, string path, SerializedProperty property, string propertyName)
        {
            var p = property.FindPropertyRelative(propertyName);
            menu.AddItem(new GUIContent(path), p.boolValue, () =>
            {
                p.boolValue = !p.boolValue;
                property.serializedObject.ApplyModifiedProperties();
            });
        }
    }
}