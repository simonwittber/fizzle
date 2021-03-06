﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;

namespace Fizzle
{

    [ExecuteInEditMode]
    [RequireComponent(typeof(AudioSource))]
    public class FizzleSynth : MonoBehaviour
    {
        const int sampleRate = Osc.SAMPLERATE;
        public bool realtime = true;
        public float duration = 5;
        public Macros[] macros = new Macros[0];
        public Sequencer[] sequencers = new Sequencer[0];
        public KarplusStrong[] karplusStrongs = new KarplusStrong[0];
        public Envelope[] envelopes = new Envelope[0];
        public Osc[] oscillators = new Osc[0];
        public CrossFader[] crossFaders = new CrossFader[0];
        public Filter[] filters = new Filter[0];
        public DelayLine[] delays = new DelayLine[0];
        public Mixer[] mixers = new Mixer[0];
        public AudioOut inputAudio = new AudioOut();

        [System.NonSerialized] public float cpuTime, bufferReady;
        internal Dictionary<int, float[]> sampleData = new Dictionary<int, float[]>();
        internal int[] sampleChannels;
        new AudioSource audio;

        public List<uint> freeJackID;
        [System.NonSerialized] float[] jacks;

        public uint TakeJackID()
        {
            var id = freeJackID[freeJackID.Count - 1];
            freeJackID.RemoveAt(freeJackID.Count - 1);
            return id;
        }

        public void FreeJackID(uint id)
        {
            freeJackID.Add(id);
        }

        void Reset()
        {
            freeJackID = new List<uint>();
            freeJackID.AddRange(from i in Enumerable.Range(0, 128) select (uint)i);
            var m = AddRackItem(ref mixers, new Mixer());
            Connect(ref inputAudio.left, ref m.output);
            Connect(ref inputAudio.right, ref m.output);
            var b = AddRackItem(ref envelopes, new Envelope());
            var s = AddRackItem(ref sequencers, new Sequencer());
            Connect(ref s.gate, ref b.output);
            b.frequency.localValue = 1; b.gain.localValue = 1; b.bandlimited = false; b.type = Osc.OscType.Sin;
            AddRackItem(ref oscillators, new Osc());
        }

        void Connect(ref JackSignal inp, ref JackOut outp)
        {
            inp.connectedId = outp.id;
        }

        private T AddRackItem<T>(ref T[] items, T item) where T : new()
        {
            var cItem = item as IRackItem;
            if (cItem != null)
            {
                cItem.OnAddToRack(this);
                cItem.OnAudioStart(this);
            }
            System.Array.Resize(ref items, Mathf.Max(1, items.Length + 1));
            items[items.Length - 1] = item;
            return item;
        }

        [ContextMenu("Play")]
        public void Play()
        {
            Init();
            audio.Play();
        }

        public void Stop()
        {
            audio.Stop();
        }

        void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            audio = GetComponent<AudioSource>();
            sample = 0;
            jacks = new float[256];
            for (var i = 0; i < macros.Length; i++)
                macros[i].OnAudioStart(this);
            for (var i = 0; i < sequencers.Length; i++)
                sequencers[i].OnAudioStart(this);
            for (var i = 0; i < karplusStrongs.Length; i++)
                karplusStrongs[i].OnAudioStart(this);
            for (var i = 0; i < envelopes.Length; i++)
                envelopes[i].OnAudioStart(this);
            for (var i = 0; i < oscillators.Length; i++)
                oscillators[i].OnAudioStart(this);
            for (var i = 0; i < crossFaders.Length; i++)
                crossFaders[i].OnAudioStart(this);
            for (var i = 0; i < delays.Length; i++)
                delays[i].OnAudioStart(this);
            for (var i = 0; i < filters.Length; i++)
                filters[i].OnAudioStart(this);
            for (var i = 0; i < mixers.Length; i++)
                mixers[i].OnAudioStart(this);
            audio = GetComponent<AudioSource>();
            ProcessAudio();
        }

        public float[] GetData()
        {
            lock (this)
            {
                Init();
                var lengthSamples = (int)(sampleRate * duration);
                var data = new float[lengthSamples * 2];
                OnAudioFilterRead(data, 2);
                return data;
            }
        }

        public AudioClip Generate()
        {
            var clip = AudioClip.Create(this.name, (int)(sampleRate * duration), 2, sampleRate, false);
            clip.SetData(GetData(), 0);
            return clip;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            lock (this)
                try
                {
                    for (var i = 0; i < data.Length; i += 2)
                    {
                        ProcessAudio();
                        var left = inputAudio.left.Value(jacks);
                        var right = inputAudio.right.Value(jacks);
                        data[i + 0] = left;
                        data[i + 1] = right;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
        }


        void ProcessAudio()
        {
            sample++;
            for (var i = 0; i < macros.Length; i++)
                macros[i].Sample(jacks, sample);
            for (var i = 0; i < sequencers.Length; i++)
                sequencers[i].Sample(jacks, sample);
            for (var i = 0; i < karplusStrongs.Length; i++)
                karplusStrongs[i].Sample(jacks, sample);
            for (var i = 0; i < envelopes.Length; i++)
                envelopes[i].Sample(jacks, sample);
            for (var i = 0; i < oscillators.Length; i++)
                oscillators[i].Sample(jacks, sample);
            for (var i = 0; i < crossFaders.Length; i++)
                crossFaders[i].Sample(jacks, sample);
            for (var i = 0; i < delays.Length; i++)
                delays[i].Sample(jacks, sample);
            for (var i = 0; i < filters.Length; i++)
                filters[i].Sample(jacks, sample);
            for (var i = 0; i < mixers.Length; i++)
                mixers[i].Sample(jacks, sample);
        }

        int sample = 0;
    }
}

