using UnityEngine;


namespace Fizzle
{

    [System.Serializable]
    public class Sampler : IHasGUID
    {
        public int sampleIndex;
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        public int ID { get { return output.id; } set { output.id = value; } }
        public int channel;

        internal float[] data;
        public int channels;

        public float Sample(int sample)
        {
            if (data == null || data.Length == 0 || channels == 0) return 0;
            var smp = data[(sample + channel) % (data.Length / channels)];
            if (multiply.connectedId != 0)
                smp *= multiply;
            if (add.connectedId != 0)
                smp += add;
            output.Value = smp;
            return smp;
        }
    }
}