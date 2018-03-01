using UnityEngine;


namespace Fizzle
{
    [System.Serializable]
    public class Envelope : Osc
    {


        public override float Sample(int t)
        {
            if (!isReady) return 0;
            if (frequency == 0) return 0;
            var smp = _Sample(phase);
            phase = phase + ((TWOPI * (1f / frequency)) / SAMPLERATE);
            if (phase > TWOPI)
                phase = phase - TWOPI;
            smp = bias + (smp * gain);
            output.Value = smp;
            return smp;
        }

    }
}
