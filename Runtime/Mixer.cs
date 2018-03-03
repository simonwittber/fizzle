using System.Runtime.CompilerServices;
using UnityEngine;


namespace Fizzle
{

    [System.Serializable]
    public class Mixer : IHasGUID
    {
        public JackSignal inputA = new JackSignal();
        public JackSignal inputB = new JackSignal();
        public JackSignal inputC = new JackSignal();
        public JackSignal inputD = new JackSignal();

        public JackIn gainA = new JackIn(1);
        public JackIn gainB = new JackIn(1);
        public JackIn gainC = new JackIn(1);
        public JackIn gainD = new JackIn(1);

        public JackIn gain = new JackIn(0.5f);
        public JackIn bias = new JackIn();
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        public uint ID { get { return output.id; } set { output.id = value; } }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Update()
        {
            var smp = 0f;
            if (inputA.connectedId != 0) smp += inputA.Value * gainA.Value;
            if (inputB.connectedId != 0) smp += inputB.Value * gainB.Value;
            if (inputC.connectedId != 0) smp += inputC.Value * gainC.Value;
            if (inputD.connectedId != 0) smp += inputD.Value * gainD.Value;
            if (multiply.connectedId != 0)
                smp *= multiply.Value;
            if (add.connectedId != 0)
                smp += add.Value;
            smp = bias.Value + (smp * gain.Value);
            output.Value = smp;
            return smp;
        }
    }
}