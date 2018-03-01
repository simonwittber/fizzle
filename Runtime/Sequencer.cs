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
        public JackIn frequency = new JackIn();
        public AnimationCurve envelope = AnimationCurve.Constant(0, 1, 1);
        public JackIn gain = new JackIn();
        public JackIn bias = new JackIn();
        public string code = "";
        public JackOut envelopeOutput = new JackOut();
        public JackOut output = new JackOut();

        public int ID
        {
            set
            {
                output.id = value;
                envelopeOutput.id = value + 1;
            }
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

        public void Sample(int sample)
        {
            if (frequency == 0)
            {
                output.Value = 0;
                return;
            }
            if (pitches == null || lastCode != code || lastType != type)
                Parse();
            var smp = _Sample(phase);
            var bpm = frequency / 60f;
            phase = phase + ((Osc.TWOPI * bpm) / Osc.SAMPLERATE);
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
            smp = bias + (smp * gain);
            output.Value = smp;
        }

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

        float _Sample(float phase)
        {
            var N = (phase / Osc.TWOPI);
            envelopeOutput.Value = envelope.Evaluate(N);
            return pitches[index % pitches.Length];
        }

        void Reverse()
        {
            System.Array.Reverse(pitches);
        }

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