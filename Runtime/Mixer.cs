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
        public JackSignal inputE = new JackSignal();
        public JackSignal inputF = new JackSignal();
        public JackSignal inputG = new JackSignal();
        public JackSignal inputH = new JackSignal();
        public JackIn gainA = new JackIn(1);
        public JackIn gainB = new JackIn(1);
        public JackIn gainC = new JackIn(1);
        public JackIn gainD = new JackIn(1);
        public JackIn gainE = new JackIn(1);
        public JackIn gainF = new JackIn(1);
        public JackIn gainG = new JackIn(1);
        public JackIn gainH = new JackIn(1);

        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        public int ID { get { return output.id; } set { output.id = value; } }

        public float Update()
        {
            var smp = 0f;
            smp += inputA.Value * gainA.Value;
            smp += inputB.Value * gainB.Value;
            smp += inputC.Value * gainC.Value;
            smp += inputD.Value * gainD.Value;
            smp += inputE.Value * gainE.Value;
            smp += inputF.Value * gainF.Value;
            smp += inputG.Value * gainG.Value;
            smp += inputH.Value * gainH.Value;
            if (multiply.connectedId != 0)
                smp *= multiply;
            if (add.connectedId != 0)
                smp += add;
            output.Value = smp;
            return smp;
        }
    }
}