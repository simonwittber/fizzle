using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class Ladder : IRackItem
    {
        public enum SequencerType
        {
            Random,
            Up,
            Down,
            PingPong
        }

        public SequencerType type;
        public JackSignal gate = new JackSignal();
        public JackOutBank ladder = new JackOutBank(16);
        public JackOut output = new JackOut();

        public void OnAddToRack(FizzleSynth fs)
        {
            output.id = fs.TakeJackID();
            for (var i = 0; i < ladder.bank.Length; i++)
                ladder.bank[i].id = fs.TakeJackID();
        }

        public void OnRemoveFromRack(FizzleSynth fs)
        {
            fs.FreeJackID(output.id);
            for (var i = 0; i < ladder.bank.Length; i++)
                fs.FreeJackID(ladder.bank[i].id);
        }

        [System.NonSerialized] float lastGate, position;
        [System.NonSerialized] JackOut[] outs;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Sample(float[] jacks, int sample)
        {
            var gateValue = gate.Value(jacks);
            var crossedZero = false;
            if (gateValue > 0 && lastGate < 0)
            {
                crossedZero = true;
                ladder.index++;
                if (ladder.index >= 16)
                    ladder.index = 0;
            }
            lastGate = gateValue;
            for (var i = 0; i < 16; i++)
                ladder.bank[i].Value(jacks, i == ladder.index ? 1 : -1);

            output.Value(jacks, crossedZero ? 1 : -1);
        }

    }
}