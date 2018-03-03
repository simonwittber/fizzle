using System.Runtime.CompilerServices;
using UnityEngine;


namespace Fizzle
{
    [System.Serializable]
    public class Envelope : Osc
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float Sample(int t)
        {
            if (!isReady) return 0;
            if (frequency.Value == 0) return 0;
            var smp = _Sample(phase);
            phase = phase + ((TWOPI * (1f / frequency.Value)) / SAMPLERATE);
            if (phase > TWOPI)
                phase = phase - TWOPI;
            smp = bias.Value + (smp * gain.Value);
            output.Value = smp;
            return smp;
        }

    }
}
