using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class DelayLine : IRackItem
    {
        public JackSignal input = new JackSignal();
        public JackIn delay = new JackIn() { localValue = 0.5f };
        public JackIn feedback = new JackIn() { localValue = 0.5f };
        public JackIn gain = new JackIn() { localValue = 0.5f };
        public JackIn bias = new JackIn();
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        int position = 0;
        float[] buffer = new float[1];

        public void OnAddToRack(FizzleSynth fs)
        {
            output.id = fs.TakeJackID();
        }

        public void OnRemoveFromRack(FizzleSynth fs)
        {
            fs.FreeJackID(output.id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Update(float[] jacks)
        {
            if (input.connectedId == 0)
            {
                output.Value(jacks, 0);
                return 0;
            }
            var length = (int)((1f / (1f / Osc.SAMPLERATE)) * delay.Value(jacks));
            if (length <= 0) length = 1;
            if (length != buffer.Length)
                buffer = new float[length];
            if (++position >= (buffer.Length))
                position = 0;
            var last = buffer[position];
            buffer[position] = input.Value(jacks) + (last * feedback.Value(jacks));
            if (multiply.connectedId != 0)
                last *= multiply.Value(jacks);
            if (add.connectedId != 0)
                last += add.Value(jacks);
            last = bias.Value(jacks) + (last * gain.Value(jacks));
            output.Value(jacks, last);
            return last;
        }
    }
}