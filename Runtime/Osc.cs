using System;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace Fizzle
{

    [System.Serializable]
    public class Osc : RackItem, IRackItem
    {
        public const float TWOPI = Mathf.PI * 2;
        public const int SAMPLERATE = 44100;

        public enum OscType
        {
            WaveShape,
            Sin,
            Tan,
            Noise,
            Square,
            Saw,
            Triangle,
            PWM
        }

        public OscType type;
        public AnimationCurve shape = new AnimationCurve();
        public float phaseOffset = 0;
        public JackIn detune = new JackIn();
        public JackIn frequency = new JackIn();
        public JackIn duty = new JackIn();
        public JackIn gain = new JackIn() { localValue = 0.5f };
        public JackIn bias = new JackIn();
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        [NonSerialized] float[] noiseBuffer;
        [NonSerialized] protected bool isReady = false;
        [NonSerialized] protected float phase;

        public bool bandlimited = true;
        public bool superSample = true;


        int noiseIndex = 0;
        float[] xv = new float[2], yv = new float[2];

        float ph;

        public void OnAddToRack(FizzleSynth fs)
        {
            output.id = fs.TakeJackID();
        }

        public void OnRemoveFromRack(FizzleSynth fs)
        {
            fs.FreeJackID(output.id);
        }


        float BandLimit(float smp)
        {
            //This is a LPF at 22049hz.
            xv[0] = xv[1];
            xv[1] = smp / 1.000071238f;
            yv[0] = yv[1];
            yv[1] = (xv[0] + xv[1]) + (-0.9998575343f * yv[0]);
            return yv[1];
        }


        public virtual float Sample(float[] jacks, int t)
        {
            if (!isReady) return 0;

            var smp = 0f;
            if (superSample)
            {
                var s = (1f / SAMPLERATE) / 8;
                var p = ph;
                for (var i = 0; i < 8; i++)
                {
                    smp += _Sample(jacks, p);
                    p += s;
                    if (p > TWOPI)
                        p -= TWOPI;
                }
                smp /= 8;
            }
            else
                smp = _Sample(jacks, phase);
            if (bandlimited) smp = BandLimit(smp);

            if (multiply.connectedId != 0)
                smp *= multiply.Value(jacks);
            if (add.connectedId != 0)
                smp += add.Value(jacks);
            ph = phase;
            phase = phase + ((TWOPI * (frequency.Value(jacks) + detune.Value(jacks))) / SAMPLERATE);
            if (phase > TWOPI)
                phase -= TWOPI;
            smp = bias.Value(jacks) + (smp * RampedGain(jacks, gain));

            output.Value((jacks), smp);
            return smp;
        }


        protected float _Sample(float[] jacks, float phase)
        {
            switch (type)
            {
                case OscType.Sin:
                    return Mathf.Sin(phase);
                case OscType.WaveShape:
                    return shape.Evaluate(phase / TWOPI);
                case OscType.Square:
                    return phase < Mathf.PI ? 1f : -1f;
                case OscType.PWM:
                    return phase < Mathf.PI * duty.Value(jacks) ? 1f : -1f;
                case OscType.Tan:
                    return Mathf.Clamp(Mathf.Tan(phase / 2), -1, 1);
                case OscType.Saw:
                    return 1f - (1f / Mathf.PI * phase);
                case OscType.Triangle:
                    if (phase < Mathf.PI)
                        return -1f + (2 * 1f / Mathf.PI) * phase;
                    else
                        return 3f - (2 * 1f / Mathf.PI) * phase;
                case OscType.Noise:
                    return noiseBuffer[(noiseIndex++) % noiseBuffer.Length];
                default:
                    return 0;
            }
        }

        public void OnAudioStart(FizzleSynth fs)
        {
            noiseBuffer = new float[SAMPLERATE * 10];
            for (var i = 0; i < noiseBuffer.Length; i++)
                noiseBuffer[i] = Mathf.Lerp(-1, 1, UnityEngine.Random.value);
            phase = phaseOffset * TWOPI;
            ph = phase;
            isReady = true;
        }

    }

}