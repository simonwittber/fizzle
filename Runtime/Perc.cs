using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class Perc : SynthBase
    {

        public JackIn frequency = new JackIn() { localValue = 440 };
        public JackIn noise = new JackIn();
        public AnimationCurve noiseEnvelope = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve pitchEnvelope = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve amplitudeEnvelope = AnimationCurve.Linear(0, 0, 1, 1);
        public Filter.FilterType type;
        public AnimationCurve waveshaper = AnimationCurve.Linear(0, -1, 1, 1);
        public JackIn cutoff = new JackIn() { localValue = 22049 };
        public JackIn q = new JackIn();

        [System.NonSerialized] BQFilter bqFilter = new BQFilter();

        int period = 0;
        [System.NonSerialized] float phase = 0, lastC, lastQ;
        [System.NonSerialized] Filter.FilterType lastType;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override float GetSample(float[] jacks)
        {
            var smp = 0f;
            var amp = amplitudeEnvelope.Evaluate(position);
            smp = Mathf.Sin(phase) * amp;
            var n = ((Entropy.Next() * 2) - 1) * noise.Value(jacks) * noiseEnvelope.Evaluate(position);
            var c = cutoff.Value(jacks);
            var r = q.Value(jacks);
            smp += UpdateFilter(jacks, n);
            phase = phase + ((TWOPI * (pitchEnvelope.Evaluate(position) * frequency.Value(jacks))) / SAMPLERATE);
            if (phase > TWOPI)
                phase -= TWOPI;

            return smp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float UpdateFilter(float[] jacks, float smp)
        {
            var c = cutoff.Value(jacks) + Mathf.Epsilon;
            var r = q.Value(jacks) + Mathf.Epsilon;
            if (c != lastC || r != lastQ || type != lastType)
            {
                lastC = c; lastQ = r; lastType = type;
                switch (type)
                {
                    case Filter.FilterType.Lowpass:
                        bqFilter.SetLowPass(c, r);
                        break;
                    case Filter.FilterType.Highpass:
                        bqFilter.SetHighPass(c, r);
                        break;
                    case Filter.FilterType.Bandpass:
                        bqFilter.SetBandPass(c, r);
                        break;
                    case Filter.FilterType.Bandstop:
                        bqFilter.SetBandStop(c, r);
                        break;
                    case Filter.FilterType.Allpass:
                        bqFilter.SetAllPass(c, r);
                        break;
                }
            }

            switch (type)
            {
                case Filter.FilterType.PassThru: return smp;
                case Filter.FilterType.Lowpass:
                case Filter.FilterType.Highpass:
                case Filter.FilterType.Bandpass:
                case Filter.FilterType.Bandstop:
                case Filter.FilterType.Allpass:
                    return bqFilter.Update(smp);
                case Filter.FilterType.Waveshaper:
                    return waveshaper.Evaluate(smp);
            }
            return 0f;
        }


    }

}