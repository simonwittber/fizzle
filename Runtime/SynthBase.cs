using System.Runtime.CompilerServices;
using UnityEngine;


namespace Fizzle
{
    [System.Serializable]
    public class SynthBase : IHasGUID
    {
        public const float TWOPI = Mathf.PI * 2;
        public const int SAMPLERATE = 44100;

        public JackSignal gate = new JackSignal();

        public JackIn gain = new JackIn(0.5f);
        public JackIn bias = new JackIn();

        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        public uint ID { get { return output.id; } set { output.id = value; } }

        protected float position = 0;
        protected bool Active
        {
            get { return gate.Value > 0; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual float Sample(int t)
        {
            var smp = GetSample();

            if (multiply.connectedId != 0)
                smp *= multiply.Value;
            if (add.connectedId != 0)
                smp += add.Value;


            smp = bias.Value + (smp * gain.Value);

            output.Value = smp;
            return smp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual float GetSample()
        {
            return 0;
        }

    }

}