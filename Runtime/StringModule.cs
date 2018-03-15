using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class StringModule : SynthBase
    {

        public JackIn frequency = new JackIn() { localValue = 440 };


        int period = 0;
        [System.NonSerialized] int loopCount = 0;
        [System.NonSerialized] float[] wave = new float[44100];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override float GetSample(float[] jacks)
        {
            var freq = frequency.Value(jacks);
            if (Mathf.Approximately(freq, 0)) return 0;

            var periodF = (Osc.SAMPLERATE / (Mathf.Epsilon + frequency.Value(jacks)));
            period = (int)periodF;
            var frac = periodF - period;

            var si = sampleIndex % period;

            var smp = Lerp(wave[si], wave[si + 1], frac);

            return smp;
        }

        public override void OnAudioStart(FizzleSynth fs)
        {
            for (var pass = 0; pass < 15; pass++)
            {
                for (var i = 0; i < wave.Length; i++)
                {
                    if (pass == 0)
                        wave[i] = Entropy.Next() * 2 - 1;
                    else
                        wave[i] = (wave[i] + (i == 0 ? 0 : wave[i - 1])) * 0.5f;
                }
            }
        }

    }

}