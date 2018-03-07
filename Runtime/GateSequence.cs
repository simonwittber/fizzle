using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class GateSequence : IRackItem
    {

        public JackSignal gate = new JackSignal();
        public int length = 32;
        public string code = "";
        public AnimationCurve envelope = AnimationCurve.Linear(0, 1, 1, 0);
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

        bool[] triggers;

        string lastCode;
        [System.NonSerialized] int index, lastLength;
        [System.NonSerialized] float lastGate, position;

        void Parse()
        {
            var parts = code.Split(',');
            triggers = new bool[length];
            var relIndex = 0;
            for (var i = 0; i < parts.Length; i++)
            {
                int step;
                if (int.TryParse(parts[i], out step))
                {
                    relIndex += step;
                    triggers[relIndex % length] = true;
                }
            }
            lastCode = code;
            lastLength = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Sample(float[] jacks, int sample)
        {
            if (triggers == null || lastLength != length || lastCode != code)
                Parse();
            if (triggers.Length == 0) return 0;
            var on = false;
            var gateValue = gate.Value(jacks);
            if (gateValue > 0 && lastGate < 0)
            {
                position = 0;
                index++;
                on = triggers[index % length];
            }
            else
            {
                position += (1f / Osc.SAMPLERATE);
            }
            lastGate = gateValue;
            if (position >= 1) position -= 1;
            outputEnvelope.Value(jacks, envelope.Evaluate(position));
            var smp = on ? 1 : -1;
            output.Value(jacks, smp);
            return smp;
        }


        public void OnAudioStart(FizzleSynth fs)
        {

        }
    }
}