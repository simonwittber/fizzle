using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class CrossFader : IHasGUID
    {
        public JackIn position = new JackIn();
        public JackIn gain = new JackIn();
        public AnimationCurve ramp = AnimationCurve.Linear(0, 0, 1, 1);
        public bool quant = false;

        public JackSignal inputA = new JackSignal();
        public JackSignal inputB = new JackSignal();
        public JackSignal inputC = new JackSignal();
        public JackSignal inputD = new JackSignal();
        public JackSignal inputE = new JackSignal();
        public JackSignal inputF = new JackSignal();
        public JackSignal inputG = new JackSignal();
        public JackSignal inputH = new JackSignal();

        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        public int ID { get { return output.id; } set { output.id = value; } }

        public float Sample(int t)
        {
            var smp = 0f;
            var apos = ramp.Evaluate(position);
            var fpos = (apos * 8) % 8;
            var ipos = Mathf.FloorToInt(fpos);
            var frac = fpos - ipos;
            var omf = 1f - frac;
            if (quant)
            {
                omf = 1;
                frac = 0;
            }
            output.Value = inputA.Value;
            smp += inputA.Value * (ipos == 0 ? omf : ipos == 7 ? frac : 0);
            smp += inputB.Value * (ipos == 1 ? omf : ipos == 0 ? frac : 0);
            smp += inputC.Value * (ipos == 2 ? omf : ipos == 1 ? frac : 0);
            smp += inputD.Value * (ipos == 3 ? omf : ipos == 2 ? frac : 0);
            smp += inputE.Value * (ipos == 4 ? omf : ipos == 3 ? frac : 0);
            smp += inputF.Value * (ipos == 5 ? omf : ipos == 4 ? frac : 0);
            smp += inputG.Value * (ipos == 6 ? omf : ipos == 5 ? frac : 0);
            smp += inputH.Value * (ipos == 7 ? omf : ipos == 6 ? frac : 0);

            if (multiply.connectedId != 0)
                smp *= multiply;
            if (add.connectedId != 0)
                smp += add;

            smp *= gain;
            output.Value = smp;
            return smp;
        }
    }
}