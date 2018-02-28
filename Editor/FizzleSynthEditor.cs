﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Fizzle
{


    [CustomEditor(typeof(FizzleSynth))]
    public partial class FizzleSynthEditor : Editor
    {
        bool play = false;
        bool stop = false;


        // Oscilloscope oscilloscope = new Oscilloscope();

        void OnEnable()
        {
            EditorApplication.update -= Update;
            EditorApplication.update += Update;
        }

        void OnDisable()
        {
            EditorApplication.update -= Update;
        }

        public override void OnInspectorGUI()
        {
            var fa = target as FizzleSynth;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("realtime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sampleBank"), true);
            serializedObject.ApplyModifiedProperties();
            lock (typeof(JackDrawer))
            {
                JackDrawer.BeginJackDrawers();
                DrawRack(ref fa.envelopes, serializedObject.FindProperty("envelopes"), Color.cyan);
                DrawRack(ref fa.samplers, serializedObject.FindProperty("samplers"), Color.magenta);
                DrawRack(ref fa.oscillators, serializedObject.FindProperty("oscillators"), Color.green);
                DrawRack(ref fa.filters, serializedObject.FindProperty("filters"), Color.red);
                DrawRack(ref fa.delays, serializedObject.FindProperty("delays"), Color.blue);
                DrawRack(ref fa.equalizers, serializedObject.FindProperty("equalizers"), Color.yellow);
                DrawRack(ref fa.mixers, serializedObject.FindProperty("mixers"), Color.black);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("inputAudio"), new GUIContent("Audio Out"));
                fa.activeJackOuts = JackDrawer.EndJackDrawers();
            }
            if (EditorGUI.EndChangeCheck())
            {

            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Play"))
                play = true;
            if (GUILayout.Button("Stop"))
                stop = true;
            if (GUILayout.Button("Save"))
                AudioClipExporter.Save("Fizzle.wav", fa.GetData());
            GUILayout.EndHorizontal();
            // var rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth - 32, 32);
            // oscilloscope.duration = fa.duration;
            // if (fa.inputAudio.monitor.connectedId != 0)
            //     oscilloscope.Update(rect, Color.white, fa.Monitor);

        }

        private void DrawRack<T>(ref T[] items, SerializedProperty property, Color c) where T : new()
        {
            var height = (items.Length * 18) + 18;
            var position = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, height);
            c.a = 0.025f;
            Handles.DrawSolidRectangleWithOutline(position, c, Color.white * 0.25f);
            var rect = position;
            rect.height = 18;
            rect.width = 16;
            if (GUI.Button(rect, new GUIContent("", "Add"), "radio"))
            {
                AddRackItem(ref items, property, new T());
            }
            rect.x += 32;
            rect.width = position.width - 128;
            EditorGUI.LabelField(rect, property.displayName, EditorStyles.miniBoldLabel);
            rect.width = position.width;
            rect.x = position.x;
            rect.y += rect.height;
            var toRemove = -1;
            var toDuplicate = -1;
            for (var i = 0; i < items.Length; i++)
            {
                var brect = rect;
                brect.width = 16;
                GUI.color = Color.red;
                if (GUI.Button(brect, new GUIContent("", "Right click to remove, Left click to duplicate"), "radio"))
                {
                    if (Event.current.button == 0)
                        toDuplicate = i;
                    if (Event.current.button == 1)
                        toRemove = i;
                }
                GUI.color = Color.white;
                rect.xMin += brect.width + 3;
                var itemProperty = property.GetArrayElementAtIndex(i);
                using (var cc = new EditorGUI.ChangeCheckScope())
                {
                    EditorGUI.PropertyField(rect, itemProperty);
                    if (cc.changed)
                        itemProperty.serializedObject.ApplyModifiedProperties();
                }
                rect.xMin -= brect.width + 3;
                rect.y += rect.height;
            }
            if (toRemove >= 0)
            {
                property.DeleteArrayElementAtIndex(toRemove);
                property.serializedObject.ApplyModifiedProperties();
            }
            if (toDuplicate >= 0)
            {
                AddRackItem(ref items, property, items[toDuplicate]);
                property.serializedObject.ApplyModifiedProperties();
            }

        }

        private static T AddRackItem<T>(ref T[] items, SerializedProperty property, T item) where T : new()
        {
            Undo.RecordObject(property.serializedObject.targetObject, "Add");
            var guidItem = item as IHasGUID;
            if (guidItem != null)
                guidItem.ID = System.Guid.NewGuid().GetHashCode();
            var initItem = item as IHasInit;
            if (initItem != null)
                initItem.Init();
            System.Array.Resize(ref items, Mathf.Max(1, items.Length + 1));
            items[items.Length - 1] = item;
            property.serializedObject.Update();
            return item;
        }

        void Update()
        {
            var fa = target as FizzleSynth;
            if (play)
            {
                play = false;
                fa.Play(refresh: true);
            }

            if (stop)
            {
                stop = false;
                fa.Stop();
            }
        }


    }
}