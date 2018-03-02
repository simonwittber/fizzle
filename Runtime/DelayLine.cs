using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class DelayLine : IHasGUID
    {
        public JackSignal input = new JackSignal();
        public JackIn delay = new JackIn(0.5f);
        public JackIn feedback = new JackIn(0.5f);
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        public int ID { get { return output.id; } set { output.id = value; } }

        int position = 0;
        float[] buffer = new float[1];

        public float Update()
        {
            if (input.connectedId == 0)
            {
                output.Value = 0;
                return 0;
            }
            var length = Mathf.FloorToInt((1f / (1f / Osc.SAMPLERATE)) * delay);
            if (length <= 0) length = 1;
            if (length != buffer.Length)
                buffer = new float[length];
            if (++position >= (buffer.Length))
                position = 0;
            var last = buffer[position];
            buffer[position] = input + (last * feedback);
            if (multiply.connectedId != 0)
                last *= multiply;
            if (add.connectedId != 0)
                last += add;
            output.Value = last;
            return last;
        }
    }
}