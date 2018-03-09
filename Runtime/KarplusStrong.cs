using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class KarplusStrong : SynthBase
    {

        public JackIn frequency = new JackIn() { localValue = 440 };
        public JackIn burstPeriod = new JackIn() { localValue = 1 };
        public JackIn decay = new JackIn() { localValue = 1 };

        int period = 0;
        [System.NonSerialized] int loopCount = 0;
        [System.NonSerialized] float[,] wave = new float[6, 44100];

        int activeString = 0;

        protected override void OnGate()
        {
            activeString = (activeString + 1) % 6;
            loopCount = 0;
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override float GetSample(float[] jacks)
        {
            var freq = frequency.Value(jacks);
            if (Mathf.Approximately(freq, 0)) return 0;

            period = (int)(Osc.SAMPLERATE / (Mathf.Epsilon + frequency.Value(jacks)));

            var si = sampleIndex % period;
            if (si == 0) loopCount++;

            var smp = 0f;
            if (sampleIndex < period)
            {
                smp = (Entropy.Next() * 2) - 1;
                wave[activeString, si] += smp;
            }

            var doFilter = sampleIndex > period;
            for (var i = 0; i < 6; i++)
            {
                if (!(doFilter && activeString == i))
                {
                    var prev = (si > 0 ? wave[i, si - 1] : 0);
                    var mustFilter = sampleIndex < (period * burstPeriod.Value(jacks));
                    var mightFilter = false;
                    if (mustFilter)
                        mightFilter = (Entropy.Next() < (1f / decay.Value(jacks)));
                    if (mustFilter || mightFilter)
                        wave[i, si] = (prev + wave[i, si]) * 0.5f;
                    smp += wave[i, si];
                }
            }

            return smp;
        }

    }

}