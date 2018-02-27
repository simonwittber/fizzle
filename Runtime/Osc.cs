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
        public bool superSample = true;

        public enum OscType
        {
            WaveShape,
            Sin,
            Tan,
            Noise,
            Square,
            Algo1
        }

        public OscType type;
        public AnimationCurve shape = new AnimationCurve();

        public JackIn frequency = new JackIn();
        public JackIn gain = new JackIn();
        public JackIn bias = new JackIn();
        public JackOut output = new JackOut();

        public int ID { get { return output.id; } set { output.id = value; } }

        float[] noiseBuffer;
        bool isReady = false;


        public virtual float Sample(float t, float dt, float duration)
        {
            var smp = 0f;
            if (superSample)
            {
                for (var x = -1f; x <= 0f; x += 0.125f)
                {
                    var v = _Sample(t * (frequency + (x * dt)), dt, duration);
                    smp += 0.125f * v;
                }
            }
            else
            {
                smp = _Sample(t * frequency, dt, duration);
            }
            smp = bias + (smp * gain);
            output.Value = smp;
            return smp;
        }

        protected float _Sample(float t, float dt, float duration)
        {
            if (!isReady) return 0;
            if (Mathf.Abs(t) < Mathf.Epsilon) t = Mathf.Epsilon * Mathf.Sign(t);
            switch (type)
            {
                case OscType.Sin:
                    return Mathf.Sin(t / Mathf.Deg2Rad);
                case OscType.WaveShape:
                    return shape.Evaluate(Mathf.Max(0, t));
                case OscType.Square:
                    return Mathf.Sin(Mathf.PI * 2 * t) > 0 ? 0.5f : -0.5f;
                case OscType.Tan:
                    return Mathf.Clamp(Mathf.Tan(Mathf.PI * t), -1, 1);
                case OscType.Noise:
                    var index = Mathf.Max(0, Mathf.FloorToInt(Mathf.PI * 2 * t) % noiseBuffer.Length);
                    return noiseBuffer[index];
                case OscType.Algo1:
                    {
                        var x = (byte)Mathf.FloorToInt(t / 8192);
                        return (Mathf.InverseLerp(0, 255, (x & x % 255) - (x * 3 & x >> 13 & x >> 6))) * 0.5f - 1;
                    }
            }
            return 0f;
        }

        public void Init()
        {
            // if (type == OscType.WaveShape && shape.postWrapMode == WrapMode.ClampForever)
            //     shape.postWrapMode = WrapMode.Loop;
            noiseBuffer = new float[1024 * 1024];
            for (var i = 0; i < noiseBuffer.Length; i++)
            {
                noiseBuffer[i] = Mathf.Lerp(-1, 1, Random.value);
            }
            isReady = true;
        }
    }


}