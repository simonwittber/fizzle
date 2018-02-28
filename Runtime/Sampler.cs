using UnityEngine;


namespace Fizzle
{
    [System.Serializable]
    public class Sampler : IHasGUID
    {
        public int sampleIndex;

        public JackOut output = new JackOut();

        public int ID { get { return output.id; } set { output.id = value; } }
        public int channel;

        internal float[] data;
        public int channels;

        public float Sample(int sample)
        {
            if (data == null || data.Length == 0 || channels == 0) return 0;
            var smp = data[(sample + channel) % (data.Length / channels)];
            output.Value = smp;
            return smp;
        }
    }
}