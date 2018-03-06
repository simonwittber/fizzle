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

        [System.NonSerialized] float[,] wave = new float[6, 44100];
        int activeString = 0;

        protected override void OnGate()
        {
            activeString = (activeString + 1) % 6;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override float GetSample(float[] jacks)
        {
            period = (int)(Osc.SAMPLERATE / (Mathf.Epsilon + frequency.Value(jacks)));
            if (period <= 0 || period >= 44100) period = 1;
            var si = sampleIndex % period;

            var doFilter = true;
            var smp = 0f;
            if (sampleIndex < period)
            {
                smp += input.Value(jacks);
                wave[activeString, si] += smp;
                doFilter = false;
            }

            for (var i = 0; i < 6; i++)
            {
                if (!(doFilter && activeString == i))
                    smp += wave[i, si] = ((si > 0 ? wave[i, si - 1] : 0) + wave[i, si]) * 0.5f;
            }

            return smp;
        }


    }

}