using UnityEditor;
using UnityEngine;

namespace Fizzle
{

    [CustomPropertyDrawer(typeof(Osc))]
    public class OscDrawer : FizzleDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            DrawInputProperties(position, property, "type", "shape", "frequency", "detune", "phaseOffset", "duty");
        }

        public override void OnContextMenu(Rect position, SerializedProperty property, GUIContent label)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Super Sample?"), property.FindPropertyRelative("superSample").boolValue, () =>
            {
                property.FindPropertyRelative("superSample").boolValue = !property.FindPropertyRelative("superSample").boolValue;
                property.serializedObject.ApplyModifiedProperties();
            });
            menu.AddItem(new GUIContent("Band Limited?"), property.FindPropertyRelative("bandlimited").boolValue, () =>
            {
                property.FindPropertyRelative("bandlimited").boolValue = !property.FindPropertyRelative("bandlimited").boolValue;
                property.serializedObject.ApplyModifiedProperties();
            });
            menu.ShowAsContext();
        }
    }
}