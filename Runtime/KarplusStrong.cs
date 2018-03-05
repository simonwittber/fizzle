using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class KarplusStrong : SynthBase
    {

        public JackSignal input = new JackSignal();
        public JackIn frequency = new JackIn() { localValue = 440 };

        int period = 0;

        [System.NonSerialized] float[] wave = new float[44100];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override float GetSample(float[] jacks)
        {
            period = (int)(Osc.SAMPLERATE / (Mathf.Epsilon + frequency.Value(jacks)));
            if (period <= 0 || period >= 44100) period = 1;
            var si = sampleIndex % period;
            if (sampleIndex < period)
                wave[si] += input.Value(jacks);
            else
            {
                wave[si] = ((si > 0 ? wave[si - 1] : 0) + wave[si]) * 0.5f;
            }
            return wave[si];
        }


    }

}