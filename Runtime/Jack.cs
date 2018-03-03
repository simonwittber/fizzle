using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fizzle
{

    public static class Jack
    {
        [System.ThreadStatic]
        public static float[] values;
    }

}