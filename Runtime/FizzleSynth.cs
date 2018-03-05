using System;
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
        float[] jacks;
        bool enableProfile = false;

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

        // IEnumerator Start()
        // {
        //     var data = new float[Mathf.FloorToInt(44100 * 2 * Time.deltaTime)];
        //     while (true)
        //     {
        //         enableProfile = true;
        //         ReadAudio(data);
        //         enableProfile = false;
        //         yield return null;
        //     }
        // }

        [ContextMenu("Benchmark")]
        void _Benchmark()
        {
            enableProfile = true;
            var data = new float[44100 * 2];
            var clock = new System.Diagnostics.Stopwatch();
            for (var i = 0; i < 10; i++)
            {
                clock.Start();
                ReadAudio(data);
                clock.Stop();
            }
            enableProfile = false;
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
            jacks = new float[256];
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
            var lengthSamples = (int)(sampleRate * duration);
            var data = new float[lengthSamples * 2];
            ReadAudio(data);
            return data;
        }

        public AudioClip Generate()
        {
            return AudioClip.Create("Fizzle", (int)(sampleRate * duration), 2, sampleRate, true, ReadAudio);
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
            if (abort) return;
            if (enableProfile) Profiler.BeginSample("ReadAudio");
            try
            {
                clock.Reset();
                clock.Start();
                for (var i = 0; i < data.Length; i += 2)
                {
                    ProcessAudio();
                    data[i] = inputAudio.left.Value(jacks);
                    data[i + 1] = inputAudio.right.Value(jacks);
                }
                clock.Stop();
                var maxTime = ((data.Length / 2) * 0.02267573696f);
                cpuTime = Mathf.Round(((clock.ElapsedMilliseconds) / maxTime) * 1000) / 10;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                abort = true;
            }
            if (enableProfile) Profiler.EndSample();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ProcessAudio()
        {
            sample++;
            for (var i = 0; i < sequencers.Length; i++)
                sequencers[i].Sample(jacks, sample);
            for (var i = 0; i < envelopes.Length; i++)
                envelopes[i].Sample(jacks, sample);
            for (var i = 0; i < samplers.Length; i++)
            {
                var s = samplers[i];
                s.channels = sampleChannels[s.sampleIndex];
                s.data = sampleData[s.sampleIndex];
            }
            // s.Sample(sample);
            for (var i = 0; i < oscillators.Length; i++)
                oscillators[i].Sample(jacks, sample);
            for (var i = 0; i < crossFaders.Length; i++)
                crossFaders[i].Sample(jacks, sample);
            for (var i = 0; i < delays.Length; i++)
                delays[i].Update(jacks);
            for (var i = 0; i < filters.Length; i++)
                filters[i].Update(jacks);
            for (var i = 0; i < mixers.Length; i++)
                mixers[i].Update(jacks);
        }

        bool abort = false;
        int sample = 0;
    }
}