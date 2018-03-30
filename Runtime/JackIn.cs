using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Fizzle
{
    [System.Serializable]
    public struct JackIn
    {

        public uint connectedId;
        public float localValue;
        public bool oneMinusX;
        public bool xMulMinusOne;
        public bool attenuate;
        public bool amplify;


        public float Value(float[] jacks)
        {
            if (connectedId == 0) return localValue;
            var value = jacks[connectedId];
            if (xMulMinusOne) value *= -1;
            if (oneMinusX) value = 1 - value;
            if (attenuate) value *= 0.5f;
            if (amplify) value *= 2f;
            return value;
        }


        public void Value(float[] jacks, float value)
        {
            localValue = value;
        }

        public override string ToString()
        {
            return $"{GetType().Name}:{connectedId}";
        }
    }




}