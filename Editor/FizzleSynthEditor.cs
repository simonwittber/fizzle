using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            serializedObject.ApplyModifiedProperties();
            lock (typeof(JackDrawer))
            {
                JackDrawer.BeginJackDrawers();
                DrawRack(ref fa.macros, serializedObject.FindProperty("macros"), Color.cyan);
                DrawRack(ref fa.sequencers, serializedObject.FindProperty("sequencers"), Color.cyan);
                DrawRack(ref fa.envelopes, serializedObject.FindProperty("envelopes"), Color.cyan);
                DrawRack(ref fa.oscillators, serializedObject.FindProperty("oscillators"), Color.green);
                DrawRack(ref fa.karplusStrongs, serializedObject.FindProperty("karplusStrongs"), Color.cyan);
                DrawRack(ref fa.crossFaders, serializedObject.FindProperty("crossFaders"), Color.gray);
                DrawRack(ref fa.filters, serializedObject.FindProperty("filters"), Color.red);
                DrawRack(ref fa.delays, serializedObject.FindProperty("delays"), Color.blue);
                DrawRack(ref fa.mixers, serializedObject.FindProperty("mixers"), Color.black);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("inputAudio"), new GUIContent("Audio Out"));
                JackDrawer.EndJackDrawers();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Play"))
                play = true;
            if (GUILayout.Button("Stop"))
                stop = true;
            if (GUILayout.Button("Save"))
            {
                var activePath = (string)typeof(ProjectWindowUtil).GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
                var path = EditorUtility.SaveFilePanelInProject("Save clip as WAV", target.name, "wav", "", activePath);
                if (path.Length != 0)
                {
                    fa.Stop();
                    fa.Init();
                    AudioClipExporter.Save(path, fa.GetData());
                    AssetDatabase.Refresh();
                }
            }
            // if (GUILayout.Button("Export Tones"))
            // {
            //     var activePath = (string)typeof(ProjectWindowUtil).GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
            //     var root = EditorUtility.SaveFolderPanel("Export Tones", target.name, target.name);
            //     if (root.Length != 0)
            //     {
            //         try
            //         {

            //             EditorUtility.DisplayProgressBar("Export Tones", "", 0);
            //             var m = fa.macros[0];
            //             var progress = 0;
            //             foreach (var i in new[] { "A1", "A2", "A3", "A4", "A5", "A6", "A7" })
            //             {
            //                 var path = System.IO.Path.Combine(root, $"{fa.name}_{i}.wav");
            //                 fa.Stop();
            //                 m.macroA.localValue = Note.Frequency(i);
            //                 fa.Init();
            //                 AudioClipExporter.Save(path, fa.GetData());
            //                 if (EditorUtility.DisplayCancelableProgressBar("Export Tones", i, 1f * progress / 7))
            //                     break;
            //                 progress++;
            //             }
            //             AssetDatabase.Refresh();
            //         }
            //         finally
            //         {
            //             EditorUtility.ClearProgressBar();
            //         }
            //     }
            // }
            GUILayout.EndHorizontal();
        }

        public static string GetSelectedPathOrFallback()
        {
            string path = "Assets";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                Debug.Log(obj);
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            return path;
        }

        void DrawRack<T>(ref T[] items, SerializedProperty property, Color c) where T : new()
        {
            var fa = target as FizzleSynth;
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
            for (var i = 0; i < items.Length; i++)
            {
                var brect = rect;
                brect.width = 16;
                brect.x -= 8;
                GUI.color = Color.red;
                if (GUI.Button(brect, new GUIContent("", "Right click to remove"), "radio"))
                {
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
                var cItem = items[toRemove] as IRackItem;
                if (cItem != null)
                    cItem.OnRemoveFromRack(fa);
                property.DeleteArrayElementAtIndex(toRemove);
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private T AddRackItem<T>(ref T[] items, SerializedProperty property, T item) where T : new()
        {
            Undo.RecordObject(property.serializedObject.targetObject, "Add");
            var fs = target as FizzleSynth;
            var cItem = item as IRackItem;
            if (cItem != null)
            {
                cItem.OnAddToRack(fs);
                cItem.OnAudioStart(fs);
            }
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
                fa.Play();
            }

            if (stop)
            {
                stop = false;
                fa.Stop();
            }
        }

    }

}