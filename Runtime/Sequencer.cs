using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{

    [System.Serializable]
    public class Sequencer : IRackItem
    {
        public enum SequencerType
        {
            Random,
            Up,
            Down,
            PingPong
        }

        public SequencerType type;
        public JackSignal gate = new JackSignal();
        public JackIn gateLength = new JackIn() { localValue = 0.5f };
        public AnimationCurve envelope = AnimationCurve.Constant(0, 1, 1);
        public JackIn glide = new JackIn() { localValue = 1f };
        public JackIn frequencyMultiply = new JackIn() { localValue = 1f };
        public JackIn transpose = new JackIn();
        public string code = "";
        public JackOut outputEnvelope = new JackOut();
        public JackOut output = new JackOut();

        public void OnAddToRack(FizzleSynth fs)
        {
            output.id = fs.TakeJackID();
            outputEnvelope.id = fs.TakeJackID();
        }

        public void OnRemoveFromRack(FizzleSynth fs)
        {
            fs.FreeJackID(output.id);
            fs.FreeJackID(outputEnvelope.id);
        }

        float[] pitches;

        string lastCode;
        SequencerType lastType;
        [System.NonSerialized] int index;
        [System.NonSerialized] float lastGate, position, outputFreq = 0f;

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
        public void Sample(float[] jacks, int sample)
        {
            if (pitches == null || lastCode != code || lastType != type)
                Parse();

            var gateValue = gate.Value(jacks);
            if (gateValue > 0 && lastGate < 0)
            {
                position = 0;
                index++;
                if (index >= pitches.Length)
                {
                    index = 0;
                    ChangePitchPattern();
                }
            }
            else
            {
                position += (1f / Osc.SAMPLERATE);
            }
            lastGate = gateValue;

            var smp = _Sample(jacks);
            smp = transpose.Value(jacks) + (smp * frequencyMultiply.Value(jacks));
            output.Value(jacks, smp);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float _Sample(float[] jacks)
        {
            var N = position / gateLength.Value(jacks);
            outputEnvelope.Value(jacks, envelope.Evaluate(N));
            if (pitches.Length == 0) return 0;
            var f = pitches[index % pitches.Length];
            if (transpose.Value(jacks) != 0)
            {
                var number = Note.Number(f);
                if (number > 0)
                    f = Note.Frequency(number + (int)(transpose.Value(jacks)));
            }
            outputFreq = Mathf.Lerp(outputFreq, f, glide.Value(jacks));
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
                int k = (int)(Entropy.Next() * (n + 1));
                var value = pitches[k];
                pitches[k] = pitches[n];
                pitches[n] = value;
            }

        }

    }
}