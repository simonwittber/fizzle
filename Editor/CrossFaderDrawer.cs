using System;
using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [CustomPropertyDrawer(typeof(CrossFader))]
    public class CrossFaderDrawer : FizzleDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= DrawOutputJacks(position, property);
            DrawInputProperties(position, property, "ramp", "quant", "position", "inputA", "inputB", "inputC", "inputD", "inputE", "inputF", "inputG", "inputH");
        }


    }
}