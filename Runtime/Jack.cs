using System.Collections.Generic;
using UnityEngine;

namespace Fizzle
{

    public static class Jack
    {
        static Dictionary<int, float> values;

        static Jack()
        {
            values = new Dictionary<int, float>();
        }

        public static float GetValue(int id)
        {
            float f;
            if (values.TryGetValue(id, out f))
                return f;
            return 0f;
        }

        public static void SetValue(int id, float f)
        {
            values[id] = f;
        }
    }

}