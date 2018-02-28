using System;
using System.Collections;
using System.Collections.Generic;
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
        public Envelope[] envelopes = new Envelope[0];
        public Sampler[] samplers = new Sampler[0];
        public Osc[] oscillators = new Osc[0];
        public Filter[] filters = new Filter[0];
        public DelayLine[] delays = new DelayLine[0];
        public Equalizer[] equalizers = new Equalizer[0];
        public Mixer[] mixers = new Mixer[0];
        public AudioOut inputAudio = new AudioOut();
        public int[] activeJackOuts;
        public long cpuTime;

        Dictionary<int, float[]> sampleData = new Dictionary<int, float[]>();
        int[] sampleChannels;
        new AudioSource audio;

        [ContextMenu("Dump Jacks")]
        public void DumpJacks()
        {
            foreach (var j in JackIn.instances)
                Debug.Log(j);
            foreach (var j in JackSignal.instances)
                Debug.Log(j);
            foreach (var j in JackOut.instances)
                Debug.Log(j);
        }

        [ContextMenu("Play")]
        public void _Play()
        {
            Play(refresh: true);
        }

        public void Play(bool refresh = false)
        {
            audio = GetComponent<AudioSource>();
            sample = 0;
            Init();
            if (audio.clip == null || refresh) audio.clip = Generate();
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
            audio.clip = Generate();
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

        bool JackOutIsUsed(int index)
        {
            if (activeJackOuts == null) return false;
            foreach (var i in activeJackOuts) if (i == index) return true;
            return false;
        }

        System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
        void ReadAudio(float[] data)
        {
            clock.Start();
            if (abort) return;
            try
            {
                for (var i = 0; i < data.Length; i += 2)
                {
                    sample++;

                    foreach (var e in envelopes)
                        if (e.output != null && JackOutIsUsed(e.output.id))
                            e.Sample(sample);
                    foreach (var s in samplers)
                        if (s.output != null && JackOutIsUsed(s.output.id))
                        {
                            s.channels = sampleChannels[s.sampleIndex];
                            s.data = sampleData[s.sampleIndex];
                            s.Sample(sample);
                        }
                    foreach (var o in oscillators)
                        if (o.output != null && JackOutIsUsed(o.output.id))
                            o.Sample(sample);
                    foreach (var d in delays)
                        d.Update();
                    foreach (var f in filters)
                        f.Update();
                    foreach (var e in equalizers)
                        e.Update();
                    foreach (var m in mixers)
                        if (JackOutIsUsed(m.output.id))
                            m.Update();
                    data[i] = inputAudio.left.Value;
                    data[i + 1] = inputAudio.right.Value;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                abort = true;
            }
            clock.Stop();
            cpuTime = clock.ElapsedTicks / data.Length;
            clock.Reset();
        }
        bool abort = false;
        int sample = 0;
    }
}