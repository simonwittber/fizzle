using System.Runtime.CompilerServices;

namespace Fizzle
{
    public abstract class RackItem
    {
        public int sortOrder = 0;

        protected float _gain = 0;
        protected float RampedGain(float[] jacks, JackIn j)
        {
            _gain = Lerp(_gain, j.Value(jacks), 0.01f);
            return _gain;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Lerp(float start, float end, float t)
        {
            return end * t + start * (1f - t);
        }
    }

}