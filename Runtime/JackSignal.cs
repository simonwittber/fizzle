using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Fizzle
{
    [System.Serializable]
    public struct JackSignal
    {
        public bool oneMinusX;
        public bool xMulMinusOne;
        public bool attenuate;
        public bool amplify;

        public uint connectedId;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Value(float[] jacks)
        {

            var value = jacks[connectedId];
            if (xMulMinusOne) value *= -1;
            if (oneMinusX) value = 1 - value;
            if (attenuate) value *= 0.5f;
            if (amplify) value *= 2f;
            return value;
        }

        public override string ToString()
        {
            return $"{GetType().Name}:{connectedId}";
        }

    }



}