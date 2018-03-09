using System.Linq;
using System.Runtime.CompilerServices;
using DifferentMethods.Extensions.Collections;
using UnityEngine;

namespace Fizzle
{

    [System.Serializable]
    public class Sequencer : RackItem, IRackItem
    {
        public enum SequencerType
        {
            Random,
            Up,
            Down,
            PingPong
        }

        public struct NoteTrigger : System.IComparable<NoteTrigger>
        {
            public float hz;
            public float volume;
            public int duration;
            public int beat;
            public int noteNumber;

            public int CompareTo(NoteTrigger other)
            {
                return beat.CompareTo(other.beat);
            }

            public override string ToString()
            {
                return $"B{beat} - {hz}:{duration}:{volume}";
            }
        }

        public SequencerType type;
        public JackSignal gate = new JackSignal();
        public AnimationCurve envelope = AnimationCurve.Constant(0, 1, 1);
        public JackIn glide = new JackIn() { localValue = 1f };
        public JackIn frequencyMultiply = new JackIn() { localValue = 1f };
        public JackIn transpose = new JackIn();

        public string code = "";
        public JackOut outputEnvelope = new JackOut();
        public JackOut outputTrigger = new JackOut();
        public JackOut output = new JackOut();

        public void OnAddToRack(FizzleSynth fs)
        {
            output.id = fs.TakeJackID();
            outputTrigger.id = fs.TakeJackID();
            outputEnvelope.id = fs.TakeJackID();
        }

        public void OnRemoveFromRack(FizzleSynth fs)
        {
            fs.FreeJackID(output.id);
            fs.FreeJackID(outputTrigger.id);
            fs.FreeJackID(outputEnvelope.id);
        }

        NoteTrigger[] sequence;
        string lastCode;
        SequencerType lastType;
        int index, beatIndex, beatDuration, lastBeat, position;
        float lastGate, outputFreq = 0f;

        PriorityQueue<NoteTrigger> notes = new PriorityQueue<NoteTrigger>();
        NoteTrigger activeNote;

        void Parse()
        {
            var parts = (from i in code.Split(',') select i.Trim()).ToArray();
            sequence = new NoteTrigger[parts.Length];
            for (var i = 0; i < parts.Length; i++)
            {
                var noteTrigger = new NoteTrigger() { duration = 1, volume = 0.9f };
                var pda = (from x in parts[i].Split(':') select x.Trim()).ToArray();
                if (pda.Length >= 1)
                {
                    float hz;
                    if (float.TryParse(pda[0], out hz))
                        noteTrigger.hz = hz;
                    else
                    {
                        noteTrigger.hz = Note.Frequency(pda[0]);
                        noteTrigger.noteNumber = Note.Number(noteTrigger.hz);
                    }
                }
                if (pda.Length >= 2)
                {
                    int duration;
                    if (int.TryParse(pda[1], out duration))
                        noteTrigger.duration = duration;
                }
                if (pda.Length >= 3)
                {
                    float volume;
                    if (float.TryParse(pda[2], out volume))
                        noteTrigger.volume = volume;
                }
                sequence[i] = noteTrigger;
            }
            if (type == SequencerType.Down)
                Reverse();
            lastCode = code;
            lastType = type;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Sample(float[] jacks, int sample)
        {
            if (sequence == null || lastCode != code || lastType != type)
            {
                Parse();
                ScheduleNoteTriggers(0);
            }
            position++;
            var gateValue = gate.Value(jacks);
            outputTrigger.Value(jacks, -1);
            if (gateValue > 0 && lastGate < 0)
            {
                beatDuration = (sample - lastBeat);
                lastBeat = sample;
                NextBeat(jacks);
            }
            var hz = activeNote.hz;
            var tr = (int)transpose.Value(jacks);
            if (tr != 0 && activeNote.noteNumber >= 0)
                hz = Note.Frequency(activeNote.noteNumber + tr);

            hz *= frequencyMultiply.Value(jacks);
            var N = position * 1f / (beatDuration * activeNote.duration);
            lastGate = gateValue;
            output.Value(jacks, hz);
            var e = beatDuration > 0 ? envelope.Evaluate(N) : 1;
            outputEnvelope.Value(jacks, activeNote.volume * e);
            return activeNote.hz;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void NextBeat(float[] jacks)
        {
            beatIndex++;
            if (!notes.IsEmpty)
            {
                var note = notes.Peek();
                if (note.beat <= beatIndex)
                {
                    position = 0;
                    outputTrigger.Value(jacks, 1);
                    activeNote = notes.Pop();
                }
            }
            if (notes.IsEmpty)
            {
                ChangeNoteTriggerPattern();
                ScheduleNoteTriggers(beatIndex + activeNote.duration);
            }
        }

        void ScheduleNoteTriggers(int startBeat)
        {
            var b = startBeat;
            notes.Clear();
            for (var i = 0; i < sequence.Length; i++)
            {
                var n = sequence[i];
                n.beat = b;
                // Debug.Log($"SCHEDULED: {n}");
                notes.Push(n);
                b += n.duration;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ChangeNoteTriggerPattern()
        {
            switch (type)
            {
                case SequencerType.Random:
                    Shuffle();
                    break;
                case SequencerType.PingPong:
                    Reverse();
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Reverse()
        {
            System.Array.Reverse(sequence);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Shuffle()
        {
            int n = sequence.Length;
            while (n > 1)
            {
                n--;
                int k = (int)(Entropy.Next() * (n + 1));
                var value = sequence[k];
                sequence[k] = sequence[n];
                sequence[n] = value;
            }

        }

        public void OnAudioStart(FizzleSynth fs)
        {
            beatIndex = -1;
            Parse();
            ScheduleNoteTriggers(0);
        }
    }
}