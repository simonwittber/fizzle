using System.Collections.Generic;

namespace Fizzle
{
    [System.Serializable]
    public class JackSignal
    {
        public static readonly HashSet<JackSignal> instances = new HashSet<JackSignal>();

        public bool oneMinusX = false;
        public bool xMulMinusOne = false;
        public bool attenuate = false;
        public bool amplify = false;

        public JackSignal()
        {
            instances.Add(this);
        }

        public uint connectedId;
        public float Value
        {
            get
            {
                var value = Jack.values[connectedId];
                if (xMulMinusOne) value *= -1;
                if (oneMinusX) value = 1 - value;
                if (attenuate) value *= 0.5f;
                if (amplify) value *= 2f;
                return value;
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name}:{connectedId}";
        }

    }



}