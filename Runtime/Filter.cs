using System.Runtime.CompilerServices;
using UnityEngine;


namespace Fizzle
{
    [System.Serializable]
    public class Filter : RackItem, IRackItem
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
        public AnimationCurve waveshaper = AnimationCurve.Linear(0, -1, 1, 1);
        public JackIn cutoff = new JackIn() { localValue = 22049 };
        public JackIn q = new JackIn();
        public JackIn gain = new JackIn() { localValue = 0.5f };
        public JackIn bias = new JackIn();
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        BQFilter bqFilter = new BQFilter();
        float lastC, lastQ;
        FilterType lastType;

        public void OnAddToRack(FizzleSynth fs)
        {
            output.id = fs.TakeJackID();
        }

        public void OnRemoveFromRack(FizzleSynth fs)
        {
            fs.FreeJackID(output.id);
        }


        public float Sample(float[] jacks, int t)
        {
            if (input.connectedId == 0)
            {
                output.Value(jacks, 0);
                return 0;
            }
            var smp = _Update(jacks, input.Value(jacks));
            if (multiply.connectedId != 0)
                smp *= multiply.Value(jacks);
            if (add.connectedId != 0)
                smp += add.Value(jacks);
            smp = bias.Value(jacks) + (smp * gain.Value(jacks));
            output.Value(jacks, smp);
            return smp;
        }

        float _Update(float[] jacks, float smp)
        {
            var c = cutoff.Value(jacks);
            var r = q.Value(jacks);

            switch (type)
            {
                case FilterType.Lowpass:
                    bqFilter.SetLowPass(c, r);
                    break;
                case FilterType.Highpass:
                    bqFilter.SetHighPass(c, r);
                    break;
                case FilterType.Bandpass:
                    bqFilter.SetBandPass(c, r);
                    break;
                case FilterType.Bandstop:
                    bqFilter.SetBandStop(c, r);
                    break;
                case FilterType.Allpass:
                    bqFilter.SetAllPass(c, r);
                    break;
            }

            switch (type)
            {
                case FilterType.PassThru: return smp;
                case FilterType.Lowpass:
                case FilterType.Highpass:
                case FilterType.Bandpass:
                case FilterType.Bandstop:
                case FilterType.Allpass:
                    return bqFilter.Update(smp);
                case FilterType.Waveshaper:
                    return waveshaper.Evaluate(smp);
            }
            return 0f;
        }

        public void OnAudioStart(FizzleSynth fs)
        {

        }
    }
}