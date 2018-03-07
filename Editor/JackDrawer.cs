using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Fizzle
{

    public class JackDrawer : PropertyDrawer
    {
        protected static SerializedProperty hotJackIn = null;
        protected static SerializedProperty hotJackOut = null;
        protected static Vector2 hotJackCurveSrc;

        protected static Dictionary<int, Rect> jackOuts = new Dictionary<int, Rect>();
        protected static Dictionary<Rect, int> jackRects = new Dictionary<Rect, int>();

        protected void Expire(int id)
        {
            // foreach (var i in JackIn.instances)
            //     if (i.connectedId == id) i.connectedId = 0;
            // foreach (var i in JackSignal.instances)
            //     if (i.connectedId == id) i.connectedId = 0;
        }

        public static void BeginJackDrawers()
        {
            jackOuts.Clear();
            jackRects.Clear();
        }

        public static void EndJackDrawers()
        {
            foreach (var kv in jackRects)
            {
                var rect = kv.Key;
                var id = kv.Value;
                Rect src;
                if (jackOuts.TryGetValue(id, out src))
                {
                    var distance = (10) + ((rect.center - src.center).magnitude * 0.1f);
                    Handles.DrawBezier(rect.center, src.center, rect.center + Vector2.down * distance, src.center + Vector2.up * distance, Color.black * 0.75f, null, 5);
                    var patchColor = PatchCableColor.GetColor(id);
                    Handles.DrawBezier(rect.center, src.center, rect.center + Vector2.down * distance, src.center + Vector2.up * distance, patchColor, null, 3);
                    Handles.color = patchColor * 0.5f;
                    Handles.DrawSolidDisc(rect.center - new Vector2(1, -1), Vector3.forward, 2);
                    Handles.DrawSolidDisc(src.center - new Vector2(1, -1), Vector3.forward, 2);
                }
            }
            if (Event.current.keyCode == KeyCode.Escape)
            {
                hotJackIn = hotJackOut = null;
                EditorWindow.focusedWindow.Repaint();
            }
            if (hotJackIn != null || hotJackOut != null)
            {
                Handles.DrawBezier(hotJackCurveSrc, Event.current.mousePosition, hotJackCurveSrc + Vector2.down * 20, Event.current.mousePosition + Vector2.up * 20, Color.yellow * 0.95f, null, 3);
                EditorWindow.focusedWindow.Repaint();
            }
            jackOuts.Clear();
            jackRects.Clear();
        }

        protected virtual void OnReset(SerializedProperty property)
        {

        }

        protected virtual void OnContext(SerializedProperty property)
        {

        }

        protected virtual void OnClick(SerializedProperty property)
        {

        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            if (property.isExpanded)
                return false;
            return true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var rect = position;
            if (label != GUIContent.none)
            {
                rect.width = EditorGUIUtility.labelWidth;
                EditorGUI.PrefixLabel(rect, label);
                rect.x += rect.width;
            }
            rect.width = 16;
            var idProperty = GetIdProperty(property);
            if (property.name.StartsWith("input"))
                GUI.color = Color.yellow;
            else if (property.name.StartsWith("gain"))
                GUI.color = Color.blue;
            else if (property.name.StartsWith("output"))
                GUI.color = Color.cyan;
            else if (property.name.StartsWith("gate"))
                GUI.color = Color.green;
            else
                GUI.color = Color.white;
            if (DrawJackButton(rect, idProperty.intValue, property))
            {
                if (Event.current.button == 1)
                    OnContext(property);
                else if (Event.current.button == 2)
                    OnReset(property);
                else
                    OnClick(property);
            }
            GUI.color = Color.white;
            rect.x += rect.width;
            rect.width = (position.xMax - rect.xMax) + 16;
            DrawSubProperties(rect, property);
        }

        protected virtual SerializedProperty GetIdProperty(SerializedProperty property)
        {
            throw new NotImplementedException();
        }

        protected virtual bool DrawJackButton(Rect rect, int id, SerializedProperty property)
        {
            return GUI.Button(rect, GUIContent.none, "radio");
        }

        protected virtual void DrawSubProperties(Rect rect, SerializedProperty property)
        {
        }

        protected static void Connect()
        {
            if (hotJackIn != null && hotJackOut != null)
            {
                hotJackIn.FindPropertyRelative("connectedId").intValue = hotJackOut.FindPropertyRelative("id").intValue;
                hotJackIn = null;
                hotJackOut = null;
            }
        }
    }
}