using System.Runtime.CompilerServices;
using UnityEngine;


namespace Fizzle
{
    [System.Serializable]
    public class SynthBase : RackItem, IRackItem
    {
        public const float TWOPI = Mathf.PI * 2;
        public const int SAMPLERATE = 44100;

        public JackSignal gate = new JackSignal();

        public JackIn gain = new JackIn() { localValue = 0.5f };
        public JackIn bias = new JackIn();

        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        protected float position = 0;
        protected int sampleIndex = 0;
        float lastGate = 0;

        public void OnAddToRack(FizzleSynth fs)
        {
            output.id = fs.TakeJackID();
        }

        public void OnRemoveFromRack(FizzleSynth fs)
        {
            fs.FreeJackID(output.id);
        }

        protected virtual void OnGate()
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual float Sample(float[] jacks, int t)
        {
            var gateValue = gate.Value(jacks);
            if (gateValue > 0 && lastGate < 0)
            {
                position = 0;
                sampleIndex = 0;
                OnGate();
            }
            else
            {
                position += (1f / Osc.SAMPLERATE);
                sampleIndex++;
            }
            lastGate = gateValue;
            var smp = GetSample(jacks);

            if (multiply.connectedId != 0)
                smp *= multiply.Value(jacks);
            if (add.connectedId != 0)
                smp += add.Value(jacks);

            smp = bias.Value(jacks) + (smp * gain.Value(jacks));
            output.Value(jacks, smp);
            return smp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual float GetSample(float[] jacks)
        {
            return 0;
        }

        public virtual void OnAudioStart(FizzleSynth fs)
        {

        }
    }

}