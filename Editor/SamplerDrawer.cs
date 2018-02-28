using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Sampler))]
    public class SamplerDrawer : FizzleDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 16;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height += 1;
            Handles.DrawSolidRectangleWithOutline(position, Color.white * 0.125f, Color.white * 0.5f);
            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 16;
            var rect = position;
            rect.height = 16;
            rect.width = 128;
            var clipProperty = property.FindPropertyRelative("sampleIndex");
            var fa = property.serializedObject.targetObject as FizzleSynth;
            var clipNames = (from i in fa.sampleBank select i.name).ToArray();
            var indexes = Enumerable.Range(0, clipNames.Length).ToArray();
            clipProperty.intValue = EditorGUI.IntPopup(rect, clipProperty.intValue, clipNames, indexes);
            rect.x += rect.width;
            rect.width = 32;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("channel"), new GUIContent("C", "Channel"));
            using (var cc = new EditorGUI.ChangeCheckScope())
            {

            }
            rect.x = position.width + 16;
            rect.width = 16;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("output"), GUIContent.none);
        }
    }
}