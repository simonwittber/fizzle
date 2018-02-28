using UnityEngine;


namespace Fizzle
{
    public interface IHasInit
    {
        void Init();
    }

    public interface IHasGUID
    {
        int ID { get; set; }
    }

    [System.Serializable]
    public class Osc : IHasGUID, IHasInit
    {
        public const float TWOPI = Mathf.PI * 2;
        public const int SAMPLERATE = 44100;

        public bool superSample = true;

        public enum OscType
        {
            WaveShape,
            Sin,
            Tan,
            Noise,
            Square,
            Saw,
            Triangle,
            PWM1,
            PWM2,
            PWM3
        }

        public OscType type;
        public AnimationCurve shape = new AnimationCurve();

        public JackIn frequency = new JackIn();
        public JackIn gain = new JackIn();
        public JackIn bias = new JackIn();
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        public int ID { get { return output.id; } set { output.id = value; } }

        float[] noiseBuffer;
        protected bool isReady = false;
        protected float phase;


        public virtual float Sample(int t)
        {
            if (!isReady) return 0;

            var smp = _Sample(phase);
            if (multiply.connectedId != 0)
                smp *= multiply;
            if (add.connectedId != 0)
                smp += add;
            phase = phase + ((TWOPI * frequency) / SAMPLERATE);
            if (phase > TWOPI)
                phase = phase - TWOPI;
            smp = bias + (smp * gain);
            output.Value = smp;
            return smp;
        }

        protected float _Sample(float phase)
        {
            switch (type)
            {
                case OscType.Sin:
                    return Mathf.Sin(phase);
                case OscType.WaveShape:
                    return shape.Evaluate(phase / TWOPI);
                case OscType.Square:
                    return phase < Mathf.PI ? 1f : -1f;
                case OscType.PWM1:
                    return phase < Mathf.PI / 2 ? 1f : -1f;
                case OscType.PWM2:
                    return phase < Mathf.PI / 4 ? 1f : -1f;
                case OscType.PWM3:
                    return phase < Mathf.PI / 8 ? 1f : -1f;
                case OscType.Tan:
                    return Mathf.Clamp(Mathf.Tan(phase / 2), -1, 1);
                case OscType.Saw:
                    return 1f - (1f / Mathf.PI * phase);
                case OscType.Triangle:
                    if (phase < Mathf.PI)
                        return -1f + (2 * 1f / Mathf.PI) * phase;
                    else
                        return 3f - (2 * 1f / Mathf.PI) * phase;
                case OscType.Noise:
                    var index = Mathf.FloorToInt((phase / TWOPI) * noiseBuffer.Length);
                    return noiseBuffer[index % noiseBuffer.Length];
                default:
                    return 0;
            }
        }

        public void Init()
        {
            noiseBuffer = new float[SAMPLERATE];
            for (var i = 0; i < noiseBuffer.Length; i++)
                noiseBuffer[i] = Mathf.Lerp(-1, 1, Random.value);
            isReady = true;
        }
    }


}