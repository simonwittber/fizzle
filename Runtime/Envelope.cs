using UnityEngine;


namespace Fizzle
{
    [System.Serializable]
    public class Envelope : Osc
    {
        public override float Sample(float t, float dt, float duration)
        {
            var smp = 0f;
            smp = _Sample(t * (1f / frequency), dt, duration);
            smp = bias + (smp * gain);
            output.Value = smp;
            return smp;
        }
    }
}
