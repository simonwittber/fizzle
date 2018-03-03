using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(Sampler))]
    public class SamplerDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            var rect = position;
            rect.height = 16;
            rect.width = 96;
            var clipProperty = property.FindPropertyRelative("sampleIndex");
            var fa = property.serializedObject.targetObject as FizzleSynth;
            var clipNames = (from i in fa.sampleBank select i.name).ToArray();
            var indexes = Enumerable.Range(0, clipNames.Length).ToArray();
            clipProperty.intValue = EditorGUI.IntPopup(rect, clipProperty.intValue, clipNames, indexes);
            rect.x += rect.width;
            DrawInputProperties(rect, property, "channel");
        }
    }
}