using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{

    [System.Serializable]
    public class Sequencer : IHasGUID
    {
        public enum SequencerType
        {
            Random,
            Up,
            Down,
            PingPong
        }

        public SequencerType type;
        public JackIn bpm = new JackIn(120);
        public AnimationCurve envelope = AnimationCurve.Constant(0, 1, 1);
        public JackIn glide = new JackIn(1);
        public JackIn frequencyMultiply = new JackIn(1);
        public JackIn transpose = new JackIn();
        public string code = "";
        public JackOut outputEnvelope = new JackOut();
        public JackOut output = new JackOut();

        public uint ID
        {
            set
            {
                output.id = value;
                outputEnvelope.id = value + 128;
            }
            get { return output.id; }
        }

        float[] pitches;
        float phase;
        string lastCode;
        SequencerType lastType;
        int index;

        void Parse()
        {
            var parts = code.Split(',');
            pitches = new float[parts.Length];
            for (var i = 0; i < parts.Length; i++)
            {
                float hz;
                if (!float.TryParse(parts[i], out hz))
                    hz = Note.Frequency(parts[i]);
                pitches[i] = hz;
            }
            if (type == SequencerType.Down)
                Reverse();
            lastCode = code;
            lastType = type;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Sample(int sample)
        {
            if (bpm.Value == 0)
            {
                output.Value = 0;
                return;
            }
            if (pitches == null || lastCode != code || lastType != type)
                Parse();
            var smp = _Sample(phase);
            var bpmF = bpm.Value / 60f;
            phase = phase + ((Osc.TWOPI * bpmF) / Osc.SAMPLERATE);
            if (phase > Osc.TWOPI)
            {
                index++;
                if (index == pitches.Length)
                {
                    index = 0;
                    ChangePitchPattern();
                }
                phase = phase - Osc.TWOPI;
            }
            smp = transpose.Value + (smp * frequencyMultiply.Value);
            output.Value = smp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ChangePitchPattern()
        {
            switch (type)
            {
                case SequencerType.Random:
                    Shuffle();
                    break;
                case SequencerType.PingPong:
                    Reverse();
                    break;
            }
        }

        float outputFreq = 0f;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float _Sample(float phase)
        {
            var N = (phase / Osc.TWOPI);
            outputEnvelope.Value = envelope.Evaluate(N);
            var f = pitches[index % pitches.Length];
            if (transpose.Value != 0)
            {
                var number = Note.Number(f);
                if (number > 0)
                    f = Note.Frequency(number + Mathf.FloorToInt(transpose.Value));
            }
            outputFreq = Mathf.Lerp(outputFreq, f, glide.Value);
            return outputFreq;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Reverse()
        {
            System.Array.Reverse(pitches);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Shuffle()
        {
            int n = pitches.Length;
            while (n > 1)
            {
                n--;
                int k = Mathf.FloorToInt(Entropy.Next * (n + 1));
                var value = pitches[k];
                pitches[k] = pitches[n];
                pitches[n] = value;
            }
        }

    }
}