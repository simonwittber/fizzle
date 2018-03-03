using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Fizzle
{
    [System.Serializable]
    public class JackIn
    {
        public static readonly HashSet<JackIn> instances = new HashSet<JackIn>();

        public uint connectedId;
        public float localValue;
        public bool oneMinusX = false;
        public bool xMulMinusOne = false;
        public bool attenuate = false;
        public bool amplify = false;


        public JackIn(float defaultValue = 0f)
        {
            localValue = defaultValue;
            instances.Add(this);
        }

        public float Value
        {
            get
            {
                if (connectedId == 0) return localValue;
                var value = Jack.values[connectedId];
                if (xMulMinusOne) value *= -1;
                if (oneMinusX) value = 1 - value;
                if (attenuate) value *= 0.5f;
                if (amplify) value *= 2f;
                return value;
            }
            set
            {
                localValue = value;
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name}:{connectedId}";
        }
    }



}