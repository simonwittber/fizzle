
using System.Collections.Generic;
using UnityEngine;

namespace Fizzle
{

    public static class Note
    {
        static Dictionary<string, int> notes = new Dictionary<string, int>(108);
        static Dictionary<int, float> noteNumbers = new Dictionary<int, float>(108);
        static Dictionary<float, int> freqToNoteNumber = new Dictionary<float, int>(108);

        public static int Number(string name)
        {
            int n;
            if (notes.TryGetValue(name, out n))
                return n;
            return -1;
        }

        public static int Number(float frequency)
        {
            int n;
            if (freqToNoteNumber.TryGetValue(frequency, out n))
                return n;
            return -1;
        }

        public static float Frequency(int number)
        {
            float f;
            if (noteNumbers.TryGetValue(number, out f))
                return f;
            return 0;
        }

        public static float Frequency(string name)
        {
            float f;
            var i = Number(name);
            if (i > 0 && noteNumbers.TryGetValue(i, out f))
                return f;
            return 0;
        }

        static Note()
        {
            var count = 0;
            System.Action<string, float> Add = (name, freq) =>
            {
                notes.Add(name, count);
                noteNumbers.Add(count, freq);
                freqToNoteNumber[freq] = count;
                count++;
            };
            Add("Z", 0);
            Add("C0", 16.35f);
            Add("C#0", 17.32f);
            Add("D0", 18.35f);
            Add("D#0", 19.45f);
            Add("E0", 20.60f);
            Add("F0", 21.83f);
            Add("F#0", 23.12f);
            Add("G0", 24.50f);
            Add("G#0", 25.96f);
            Add("A0", 27.50f);
            Add("A#0", 29.14f);
            Add("B0", 30.87f);
            Add("C1", 32.70f);
            Add("C#1", 34.65f);
            Add("D1", 36.71f);
            Add("D#1", 38.89f);
            Add("E1", 41.20f);
            Add("F1", 43.65f);
            Add("F#1", 46.25f);
            Add("G1", 49.0f);
            Add("G#1", 51.91f);
            Add("A1", 55.0f);
            Add("A#1", 58.27f);
            Add("B1", 61.74f);
            Add("C2", 65.41f);
            Add("C#2", 69.30f);
            Add("D2", 73.42f);
            Add("D#2", 77.78f);
            Add("E2", 82.41f);
            Add("F2", 87.31f);
            Add("F#2", 92.50f);
            Add("G2", 98.0f);
            Add("G#2", 103.83f);
            Add("A2", 110.0f);
            Add("A#2", 116.54f);
            Add("B2", 123.47f);
            Add("C3", 130.81f);
            Add("C#3", 138.59f);
            Add("D3", 146.83f);
            Add("D#3", 155.56f);
            Add("E3", 164.81f);
            Add("F3", 174.61f);
            Add("F#3", 185.0f);
            Add("G3", 196.0f);
            Add("G#3", 207.65f);
            Add("A3", 220.0f);
            Add("A#3", 233.08f);
            Add("B3", 246.94f);
            Add("C4", 261.63f);
            Add("C#4", 277.18f);
            Add("D4", 293.66f);
            Add("D#4", 311.13f);
            Add("E4", 329.63f);
            Add("F4", 349.23f);
            Add("F#4", 369.99f);
            Add("G4", 392.0f);
            Add("G#4", 415.30f);
            Add("A4", 440.0f);
            Add("A#4", 466.16f);
            Add("B4", 493.88f);
            Add("C5", 523.25f);
            Add("C#5", 554.37f);
            Add("D5", 587.33f);
            Add("D#5", 622.25f);
            Add("E5", 659.26f);
            Add("F5", 698.46f);
            Add("F#5", 739.99f);
            Add("G5", 783.99f);
            Add("G#5", 830.61f);
            Add("A5", 880.0f);
            Add("A#5", 932.33f);
            Add("B5", 987.77f);
            Add("C6", 1046.50f);
            Add("C#6", 1108.73f);
            Add("D6", 1174.66f);
            Add("D#6", 1244.51f);
            Add("E6", 1318.51f);
            Add("F6", 1396.91f);
            Add("F#6", 1479.98f);
            Add("G6", 1567.98f);
            Add("G#6", 1661.22f);
            Add("A6", 1760.0f);
            Add("A#6", 1864.66f);
            Add("B6", 1975.53f);
            Add("C7", 2093.0f);
            Add("C#7", 2217.46f);
            Add("D7", 2349.32f);
            Add("D#7", 2489.02f);
            Add("E7", 2637.02f);
            Add("F7", 2793.83f);
            Add("F#7", 2959.96f);
            Add("G7", 3135.96f);
            Add("G#7", 3322.44f);
            Add("A7", 3520.0f);
            Add("A#7", 3729.31f);
            Add("B7", 3951.07f);
            Add("C8", 4186.01f);
            Add("C#8", 4434.92f);
            Add("D8", 4698.64f);
            Add("D#8", 4978.03f);
            Add("E8", 5274.04f);
            Add("F8", 5587.65f);
            Add("F#8", 5919.91f);
            Add("G8", 6271.93f);
            Add("G#8", 6644.88f);
            Add("A8", 7040.0f);
            Add("A#8", 7458.62f);
            Add("B8", 7902.13f);
        }
    }
}