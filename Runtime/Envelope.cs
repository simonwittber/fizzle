using System.Runtime.CompilerServices;
using UnityEngine;


namespace Fizzle
{
    [System.Serializable]
    public class Envelope : Osc
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float Sample(float[] jacks, int t)
        {
            if (!isReady) return 0;
            if (frequency.Value(jacks) == 0) return 0;
            var smp = _Sample(phase);
            phase = phase + ((TWOPI * (1f / frequency.Value(jacks))) / SAMPLERATE);
            if (phase > TWOPI)
                phase = phase - TWOPI;
            smp = bias.Value(jacks) + (smp * gain.Value(jacks));
            output.Value(jacks, smp);
            return smp;
        }

    }
}
