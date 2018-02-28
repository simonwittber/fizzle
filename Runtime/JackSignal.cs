using System.Collections.Generic;

namespace Fizzle
{
    [System.Serializable]
    public class JackSignal
    {
        public static readonly HashSet<JackSignal> instances = new HashSet<JackSignal>();

        public bool oneMinusX = false;
        public bool xMulMinusOne = false;

        public JackSignal()
        {
            instances.Add(this);
        }

        public int connectedId;
        public float Value
        {
            get
            {
                var value = Jack.GetValue(connectedId);
                if (xMulMinusOne) value *= -1;
                if (oneMinusX) value = 1 - value;
                return value;
            }
        }

        public static implicit operator float(JackSignal j)
        {
            if (j == null) return 0f;
            return j.Value;
        }

        public override string ToString()
        {
            return $"{GetType().Name}:{connectedId}";
        }

    }



}