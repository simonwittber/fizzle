using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(JackIn))]
    public class JackInDrawer : JackDrawer
    {

        protected override bool DrawJackButton(Rect rect, int id, SerializedProperty property)
        {
            if (id != 0)
                jackRects[rect] = id;
            Handles.color = Color.blue;
            if (property.FindPropertyRelative("xMulMinusOne").boolValue)
                Handles.DrawSolidDisc(rect.center - new Vector2(1, -1), Vector3.forward, 8);
            Handles.color = Color.white;
            if (property.FindPropertyRelative("oneMinusX").boolValue)
                Handles.DrawSolidDisc(rect.center - new Vector2(1, -1), Vector3.forward, 7);

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