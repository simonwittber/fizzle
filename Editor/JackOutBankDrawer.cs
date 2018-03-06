using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(JackOutBank))]
    public class JackOutBankDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var rect = position;
            var bank = property.FindPropertyRelative("bank");
            var index = property.FindPropertyRelative("index").intValue;
            rect.width = 16;
            for (var i = 0; i < bank.arraySize; i++)
            {
                if (i % 4 == 0) rect.x += rect.width;
                if (index == i)
                {
                    Handles.color = Color.white;
                    Handles.DrawSolidDisc(rect.center - new Vector2(1, -1), Vector3.forward, 7);
                }
                EditorGUI.PropertyField(rect, bank.GetArrayElementAtIndex(i), GUIContent.none);
                rect.x += rect.width;
            }

        }
    }
}