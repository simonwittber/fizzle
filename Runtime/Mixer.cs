using System.Runtime.CompilerServices;
using UnityEngine;


namespace Fizzle
{

    [System.Serializable]
    public class Mixer : RackItem, IRackItem
    {
        public JackSignal inputA = new JackSignal();
        public JackSignal inputB = new JackSignal();
        public JackSignal inputC = new JackSignal();
        public JackSignal inputD = new JackSignal();

        public JackIn gainA = new JackIn() { localValue = 1f };
        public JackIn gainB = new JackIn() { localValue = 1f };
        public JackIn gainC = new JackIn() { localValue = 1f };
        public JackIn gainD = new JackIn() { localValue = 1f };

        public JackIn gain = new JackIn() { localValue = 0.5f };
        public JackIn bias = new JackIn();
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


        public float Sample(float[] jacks, int t)
        {
            var smp = 0f;
            if (inputA.connectedId != 0) smp += inputA.Value(jacks) * gainA.Value(jacks);
            if (inputB.connectedId != 0) smp += inputB.Value(jacks) * gainB.Value(jacks);
            if (inputC.connectedId != 0) smp += inputC.Value(jacks) * gainC.Value(jacks);
            if (inputD.connectedId != 0) smp += inputD.Value(jacks) * gainD.Value(jacks);
            if (multiply.connectedId != 0)
                smp *= multiply.Value(jacks);
            if (add.connectedId != 0)
                smp += add.Value(jacks);
            smp = bias.Value(jacks) + (smp * gain.Value(jacks));
            output.Value(jacks, smp);
            return smp;
        }
    }
}