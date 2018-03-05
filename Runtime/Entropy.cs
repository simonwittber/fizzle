using System;
using UnityEngine;

namespace Fizzle
{
    public static class Entropy
    {
        static float[] values;
        [ThreadStatic] static int index = 0;

        public static float Next()
        {
            index++;
            if (index >= values.Length) index = 0;
            return values[index];
        }

        public static float Gradient(float t)
        {
            var firstIndex = (int)(t * (values.Length - 1)) % values.Length;
            var nextIndex = (firstIndex + 1) % values.Length;
            var frac = t - (int)(t);
            return Mathf.Lerp(values[firstIndex], values[nextIndex], frac);
        }

        static Entropy()
        {
            var rnd = new System.Random(1024);
            values = new float[8 * 1024];
            for (var i = 0; i < values.Length; i++)
                values[i] = (rnd.Next() / int.MaxValue);
        }


    }
}