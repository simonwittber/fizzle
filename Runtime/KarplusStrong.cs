using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class KarplusStrong : SynthBase
    {
        public float delay = 0.1f;
        public JackSignal input = new JackSignal();
        public JackIn pulseDecay = new JackIn();
        public JackIn feedback = new JackIn();
        public JackIn cutoff = new JackIn();
        public JackIn q = new JackIn() { localValue = 0.5f };

        BQFilter bqFilter = new BQFilter();


        int delayIndex = 0;
        float[] buffer = new float[1];
        float gateAmp;
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // protected override float GetSample()
        // {
        //     // bqFilter.SetLowPass(cutoff.Value, q.Value);
        //     // gateAmp = Active ? Mathf.Lerp(gateAmp, 1, pulseDecay.Value * 10) : Mathf.Lerp(gateAmp, 0, pulseDecay.Value);
        //     // var smp = input.Value * gateAmp;
        //     // return UpdateDelay(smp);
        // }
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // float UpdateDelay(float input)
        // {
        //     // var length = Mathf.FloorToInt((1f / (1f / SAMPLERATE)) * delay);
        //     // if (length <= 0) length = 1;
        //     // if (length != buffer.Length)
        //     //     buffer = new float[length];
        //     // if (++delayIndex >= (buffer.Length))
        //     //     delayIndex = 0;
        //     // var last = buffer[delayIndex];
        //     // buffer[delayIndex] = input + bqFilter.Update(last * feedback.Value);
        //     // return last;
        // }

    }

}