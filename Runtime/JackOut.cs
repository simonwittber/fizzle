using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public struct JackOut
    {
        public uint id;


        public void Value(float[] jacks, float value)
        {
            jacks[id] = value;
        }

        public override string ToString()
        {
            return $"{GetType().Name}:{id}";
        }
    }

}