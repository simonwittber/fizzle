using UnityEditor;
using UnityEngine;

namespace Fizzle
{

    [CustomPropertyDrawer(typeof(Osc))]
    public class OscDrawer : FizzleDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 16;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            position.height += 1;
            Handles.DrawSolidRectangleWithOutline(position, Color.white * 0.125f, Color.white * 0.5f);
            // EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.indentLevel = 0;
            // position.width = EditorGUIUtility.currentViewWidth;
            EditorGUIUtility.labelWidth = 16;
            var rect = position;

            rect.height = 16;
            rect.width = 64;
            var typeProperty = property.FindPropertyRelative("type");
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && rect.Contains(Event.current.mousePosition))
                OnContextMenu(position, property, label);
            EditorGUI.PropertyField(rect, typeProperty, GUIContent.none);

            rect.x += rect.width;
            rect.width = 16;
            using (new EditorGUI.DisabledGroupScope(typeProperty.enumValueIndex > 0))
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("shape"), GUIContent.none);
            rect.x += rect.width;
            rect.width = (position.width - 64 - 16 - 16) / 5;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("frequency"), new GUIContent("F", "Frequency"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("gain"), new GUIContent("A", "Amplitude"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("bias"), new GUIContent("B", "Bias"));
            rect.x += rect.width;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("detune"), new GUIContent("D", "Detune"));
            DrawOutputJacks(position, property);
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