using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{

    [System.Serializable]
    public class DelayLine : RackItem, IRackItem
    {
        public JackSignal input = new JackSignal();
        public JackIn delay = new JackIn() { localValue = 0.5f };
        public JackIn feedback = new JackIn() { localValue = 0.5f };
        public JackIn gain = new JackIn() { localValue = 0.5f };
        public JackIn bias = new JackIn();
        public JackSignal multiply = new JackSignal();
        public JackSignal add = new JackSignal();
        public JackOut output = new JackOut();

        [System.NonSerialized] float[] buffer = new float[44100 * 10];

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
            if (input.connectedId == 0)
            {
                output.Value(jacks, 0);
                return 0;
            }

            var bufferLength = buffer.Length;

            var timeTarget = delay.Value(jacks) * Osc.SAMPLERATE;
            time = 0.0001f * timeTarget + 0.9999f * time;
            var samples = (int)time;
            var frac = time - samples;

            var pastIndex = index - samples;
            while (pastIndex < 0) pastIndex = bufferLength + pastIndex;

            var A = buffer[pastIndex];
            var B = buffer[(pastIndex + 1) % bufferLength];
            var smp = (B - A) * frac + A;

            var inp = input.Value(jacks);

            buffer[index] = inp + (smp * feedback.Value(jacks));
            index++;
            if (index >= bufferLength) index -= bufferLength;


            if (multiply.connectedId != 0)
                smp *= multiply.Value(jacks);
            if (add.connectedId != 0)
                smp += add.Value(jacks);
            smp = bias.Value(jacks) + (smp * gain.Value(jacks));
            output.Value(jacks, smp);
            return smp;
        }
        [System.NonSerialized] float time = 0;
        [System.NonSerialized] int index = 0;
    }


}