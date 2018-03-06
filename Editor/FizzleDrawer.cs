using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    public class FizzleDrawer : PropertyDrawer
    {
        public static OscDrawer hotOut;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 16;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

        }

        public virtual void OnContextMenu(Rect position, SerializedProperty property, GUIContent label)
        {

        }

        public virtual float DrawOutputJacks(Rect position, SerializedProperty property)
        {
            var gp = property.FindPropertyRelative("gain");
            var bp = property.FindPropertyRelative("bias");
            var mp = property.FindPropertyRelative("multiply");
            var ap = property.FindPropertyRelative("add");
            var op = property.FindPropertyRelative("output");

            var width = (gp == null ? 0 : 42) +
                        (bp == null ? 0 : 42) +
                        (mp == null ? 0 : 16) +
                        (ap == null ? 0 : 16) +
                        (op == null ? 0 : 16);

            var rect = position;
            rect.height = 16;
            rect.x = (position.xMax - width);
            rect.width = 42;
            if (gp != null)
            {
                EditorGUI.PropertyField(rect, gp, GUIContent.none);
                rect.x += rect.width;
            }
            if (bp != null)
            {
                EditorGUI.PropertyField(rect, bp, GUIContent.none);
                rect.x += rect.width;
            }
            rect.width = 16;
            if (mp != null)
            {
                EditorGUI.PropertyField(rect, mp, GUIContent.none);
                rect.x += rect.width;
            }
            if (ap != null)
            {
                EditorGUI.PropertyField(rect, ap, GUIContent.none);
                rect.x += rect.width;
            }
            if (op != null)
            {
                EditorGUI.PropertyField(rect, op, GUIContent.none);
            }
            return width;
        }

        protected void DrawInputProperties(Rect position, SerializedProperty property, params string[] names)
        {
            var rect = position;
            rect.height = 16;
            var width = position.width;
            var count = names.Length;
            foreach (var i in names)
            {
                var p = property.FindPropertyRelative(i);
                if (p == null) continue;
                if (p.propertyType == SerializedPropertyType.AnimationCurve)
                {
                    width -= 32;
                    count--;
                }
                if (p.propertyType == SerializedPropertyType.Boolean)
                {
                    width -= 32;
                    count--;
                }
                if (p.propertyType == SerializedPropertyType.Float)
                {
                    width -= 64;
                    count--;
                }
                if (p.name.StartsWith("input") || p.name.StartsWith("output") || p.name.StartsWith("gate"))
                {
                    width -= 16;
                    count--;
                }
            }
            width /= count;
            foreach (var i in names)
            {
                var p = property.FindPropertyRelative(i);
                if (p == null) continue;
                rect.width = width;
                var label = GUIContent.none;
                EditorGUIUtility.labelWidth = 0;
                if (p.propertyType == SerializedPropertyType.AnimationCurve)
                {
                    rect.width = 32;
                    EditorGUIUtility.labelWidth = 16;
                    label = new GUIContent(p.displayName.ToUpper().Substring(0, 1), p.displayName);
                }
                else if (p.propertyType == SerializedPropertyType.Boolean)
                {
                    rect.width = 32;
                    EditorGUIUtility.labelWidth = 16;
                    label = new GUIContent(p.displayName.ToUpper().Substring(0, 1), p.displayName);
                }
                else if (p.name.StartsWith("input") || p.name.StartsWith("output") || p.name.StartsWith("gate"))
                {
                    rect.width = 16;
                    label = GUIContent.none;
                }
                else if (p.propertyType == SerializedPropertyType.Float)
                {
                    rect.width = 64;
                    EditorGUIUtility.labelWidth = 16;
                    label = new GUIContent(p.displayName.ToUpper().Substring(0, 1), p.displayName);
                }

                EditorGUI.PropertyField(rect, p, label);
                rect.x += rect.width;
            }
        }
    }
}