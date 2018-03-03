﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public AudioClip[] sampleBank;
        public Sequencer[] sequencers = new Sequencer[0];
        public Envelope[] envelopes = new Envelope[0];
        public Sampler[] samplers = new Sampler[0];
        public Osc[] oscillators = new Osc[0];
        public KarplusStrong[] karplusStrongModules = new KarplusStrong[0];
        public CrossFader[] crossFaders = new CrossFader[0];
        public Filter[] filters = new Filter[0];
        public DelayLine[] delays = new DelayLine[0];
        public Mixer[] mixers = new Mixer[0];

        public AudioOut inputAudio = new AudioOut();
        public int[] activeJackOuts;
        public float cpuTime;

        Dictionary<int, float[]> sampleData = new Dictionary<int, float[]>();
        int[] sampleChannels;
        new AudioSource audio;

        public List<uint> freeJackID;
        float[] sharedValues;

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
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            _Benchmark();
            yield return new WaitForSeconds(1);
            _Benchmark();
        }

        [ContextMenu("Benchmark")]
        void _Benchmark()
        {
            var data = new float[44100 * 2];
            var clock = new System.Diagnostics.Stopwatch();
            for (var i = 0; i < 10; i++)
            {
                clock.Start();
                ReadAudio(data);
                clock.Stop();
            }
            Debug.Log($"10 seconds of audio generated in : {clock.ElapsedMilliseconds} ms ({clock.ElapsedMilliseconds / 1000.0 * 10}%)");
        }

        [ContextMenu("Play")]
        public void Play()
        {
            audio = GetComponent<AudioSource>();
            sample = 0;
            Init();
            audio.clip = Generate();
            abort = false;
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
            sharedValues = new float[256];
            foreach (var o in oscillators)
                o.Init();
            foreach (var e in envelopes)
                e.Init();
            sampleChannels = new int[sampleBank.Length];
            for (var i = 0; i < sampleBank.Length; i++)
            {
                var data = new float[sampleBank[i].samples];
                sampleData[i] = data;
                sampleBank[i].GetData(data, 0);
                sampleChannels[i] = sampleBank[i].channels;
            }
            audio = GetComponent<AudioSource>();
        }

        public float[] GetData()
        {
            var lengthSamples = Mathf.FloorToInt(sampleRate * duration);
            var data = new float[lengthSamples * 2];
            ReadAudio(data);
            return data;
        }

        public AudioClip Generate()
        {
            return AudioClip.Create("Fizzle", Mathf.FloorToInt(sampleRate * duration), 2, sampleRate, true, ReadAudio);
        }

        bool JackOutIsUsed(uint index)
        {
            if (activeJackOuts == null) return false;
            foreach (var i in activeJackOuts) if (i == index) return true;
            return false;
        }

        System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
        void ReadAudio(float[] data)
        {
            clock.Reset();
            clock.Start();
            if (abort) return;
            try
            {

                Jack.values = sharedValues;
                for (var i = 0; i < data.Length; i += 2)
                {
                    sample++;
                    // Profiler.BeginSample("ReadAudio");
                    foreach (var s in sequencers)
                        if (s != null)
                            s.Sample(sample);
                    foreach (var e in envelopes)
                        if (e != null)
                            e.Sample(sample);
                    foreach (var s in samplers)
                        if (s != null)
                        {
                            s.channels = sampleChannels[s.sampleIndex];
                            s.data = sampleData[s.sampleIndex];
                            s.Sample(sample);
                        }
                    foreach (var o in oscillators)
                        if (o != null)
                            o.Sample(sample);
                    foreach (var o in karplusStrongModules)
                        if (o != null)
                            o.Sample(sample);
                    foreach (var c in crossFaders)
                        if (c != null)
                            c.Sample(sample);
                    foreach (var d in delays)
                        if (d != null)
                            d.Update();
                    foreach (var f in filters)
                        if (f != null)
                            f.Update();
                    foreach (var m in mixers)
                        if (m != null)
                            m.Update();
                    data[i] = inputAudio.left.Value;
                    data[i + 1] = inputAudio.right.Value;
                    // Profiler.EndSample();
                }
                Jack.values = null;

            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                abort = true;
            }
            clock.Stop();


            var maxTime = (data.Length * 0.02267573696f);

            cpuTime = Mathf.Round(((clock.ElapsedMilliseconds) / maxTime) * 1000) / 10;
        }
        bool abort = false;
        int sample = 0;
    }
}