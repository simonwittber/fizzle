using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(JackOut))]
    public class JackOutDrawer : JackDrawer
    {
        // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        // {
        //     var idProperty = property.FindPropertyRelative("id");
        //     idProperty.intValue = property.propertyPath.GetHashCode();
        //     base.OnGUI(position, property, label);
        // }

        protected override bool DrawJackButton(Rect rect, int id, SerializedProperty property)
        {
            jackOuts[id] = rect;
            var clicked = GUI.Button(rect, new GUIContent("", id.ToString()), "radio");
            return clicked;
        }

        // protected override void DrawSubProperties(Rect rect, SerializedProperty property)
        // {
        //     rect.width = 128;
        //     var idProperty = property.FindPropertyRelative("id");
        //     EditorGUI.PrefixLabel(rect, new GUIContent("ID"));
        //     rect.x += 32;
        //     EditorGUI.PropertyField(rect, idProperty, GUIContent.none);
        // }

        protected override SerializedProperty GetIdProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative("id");
        }

        protected override void OnClick(SerializedProperty property)
        {
            hotJackCurveSrc = Event.current.mousePosition;
            hotJackOut = property;
            Connect();
            property.serializedObject.ApplyModifiedProperties();
        }

        protected override void OnReset(SerializedProperty property)
        {
            var idProperty = GetIdProperty(property);
            Undo.RegisterCompleteObjectUndo(property.serializedObject.targetObject, "Remove Connection");
            Expire(idProperty.intValue);
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}