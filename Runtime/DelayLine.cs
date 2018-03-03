using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class DelayLine : IHasGUID
    {
        public JackSignal input = new JackSignal();
        public JackIn delay = new JackIn(0.5f);
        public JackIn feedback = new JackIn(0.5f);
        public JackIn gain = new JackIn(0.5f);
        public JackIn bias = new JackIn();
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        public uint ID { get { return output.id; } set { output.id = value; } }

        int position = 0;
        float[] buffer = new float[1];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Update()
        {
            if (input.connectedId == 0)
            {
                output.Value = 0;
                return 0;
            }
            var length = Mathf.FloorToInt((1f / (1f / Osc.SAMPLERATE)) * delay.Value);
            if (length <= 0) length = 1;
            if (length != buffer.Length)
                buffer = new float[length];
            if (++position >= (buffer.Length))
                position = 0;
            var last = buffer[position];
            buffer[position] = input.Value + (last * feedback.Value);
            if (multiply.connectedId != 0)
                last *= multiply.Value;
            if (add.connectedId != 0)
                last += add.Value;
            last = bias.Value + (last * gain.Value);
            output.Value = last;
            return last;
        }
    }
}