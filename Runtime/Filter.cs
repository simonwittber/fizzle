using System.Runtime.CompilerServices;
using UnityEngine;


namespace Fizzle
{
    [System.Serializable]
    public class Filter : IHasGUID
    {
        public enum FilterType
        {
            PassThru,
            Highpass,
            Lowpass,
            Bandpass,
            Bandstop,
            Allpass,
            Waveshaper
        }

        public FilterType type;
        public JackSignal input = new JackSignal();
        public AnimationCurve waveshaper = AnimationCurve.Linear(0, 0, 1, 1);
        public JackIn cutoff = new JackIn(22050);
        public JackIn q = new JackIn(0);
        public JackIn gain = new JackIn(0.5f);
        public JackIn bias = new JackIn();
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        BQFilter bqFilter = new BQFilter();

        public uint ID { get { return output.id; } set { output.id = value; } }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Update()
        {
            if (input.connectedId == 0)
            {
                output.Value = 0;
                return 0;
            }
            var smp = _Update(input.Value);
            if (multiply.connectedId != 0)
                smp *= multiply.Value;
            if (add.connectedId != 0)
                smp += add.Value;
            smp = bias.Value + (smp * gain.Value);
            output.Value = smp;
            return smp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float _Update(float smp)
        {
            switch (type)
            {
                case FilterType.PassThru: return smp;
                case FilterType.Lowpass:
                    bqFilter.SetLowPass(cutoff.Value, q.Value);
                    return bqFilter.Update(smp);
                case FilterType.Highpass:
                    bqFilter.SetHighPass(cutoff.Value, q.Value);
                    return bqFilter.Update(smp);
                case FilterType.Bandpass:
                    bqFilter.SetBandPass(cutoff.Value, q.Value);
                    return bqFilter.Update(smp);
                case FilterType.Bandstop:
                    bqFilter.SetBandStop(cutoff.Value, q.Value);
                    return bqFilter.Update(smp);
                case FilterType.Allpass:
                    bqFilter.SetAllPass(cutoff.Value, q.Value);
                    return bqFilter.Update(smp);
                case FilterType.Waveshaper:
                    return waveshaper.Evaluate(smp);
            }
            return 0f;
        }
    }
}