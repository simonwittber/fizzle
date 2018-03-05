using UnityEngine;


namespace Fizzle
{

    [System.Serializable]
    public class Sampler : IHasGUID
    {
        public int sampleIndex;
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackIn gain = new JackIn() { localValue = 0.5f };
        public JackIn bias = new JackIn();
        public JackOut output = new JackOut();

        public uint ID { get { return output.id; } set { output.id = value; } }
        public int channel;

        internal float[] data;
        public int channels;

        public float Sample(float[] jacks, int sample)
        {
            if (data == null || data.Length == 0 || channels == 0) return 0;
            var smp = data[(sample + channel) % (data.Length / channels)];
            if (multiply.connectedId != 0)
                smp *= multiply.Value(jacks);
            if (add.connectedId != 0)
                smp += add.Value(jacks);
            smp = bias.Value(jacks) + (smp * gain.Value(jacks));
            output.Value(jacks, smp);
            return smp;
        }
    }
}