using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class CrossFader : RackItem, IRackItem
    {
        public JackIn position = new JackIn();
        public JackIn gain = new JackIn() { localValue = 0.5f };
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

        public void OnAddToRack(FizzleSynth fs)
        {
            output.id = fs.TakeJackID();
        }

        public void OnAudioStart(FizzleSynth fs)
        {
        }

        public void OnRemoveFromRack(FizzleSynth fs)
        {
            fs.FreeJackID(output.id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Sample(float[] jacks, int t)
        {
            var smp = 0f;
            var apos = ramp.Evaluate(position.Value(jacks));
            var fpos = (apos * 8) % 8;
            var ipos = (int)(fpos);
            var frac = fpos - ipos;
            var omf = 1f - frac;
            if (quant)
            {
                omf = 1;
                frac = 0;
            }
            output.Value(jacks, inputA.Value(jacks));
            smp += inputA.Value(jacks) * (ipos == 0 ? omf : ipos == 7 ? frac : 0);
            smp += inputB.Value(jacks) * (ipos == 1 ? omf : ipos == 0 ? frac : 0);
            smp += inputC.Value(jacks) * (ipos == 2 ? omf : ipos == 1 ? frac : 0);
            smp += inputD.Value(jacks) * (ipos == 3 ? omf : ipos == 2 ? frac : 0);
            smp += inputE.Value(jacks) * (ipos == 4 ? omf : ipos == 3 ? frac : 0);
            smp += inputF.Value(jacks) * (ipos == 5 ? omf : ipos == 4 ? frac : 0);
            smp += inputG.Value(jacks) * (ipos == 6 ? omf : ipos == 5 ? frac : 0);
            smp += inputH.Value(jacks) * (ipos == 7 ? omf : ipos == 6 ? frac : 0);

            if (multiply.connectedId != 0)
                smp *= multiply.Value(jacks);
            if (add.connectedId != 0)
                smp += add.Value(jacks);

            smp *= gain.Value(jacks);
            output.Value(jacks, smp);
            return smp;
        }
    }
}