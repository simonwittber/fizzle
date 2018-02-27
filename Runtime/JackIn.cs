using System.Collections.Generic;

namespace Fizzle
{
    [System.Serializable]
    public class JackIn
    {
        public static readonly HashSet<JackIn> instances = new HashSet<JackIn>();

        public int connectedId;
        public float localValue;

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
                return Jack.GetValue(connectedId);
            }
            set
            {
                localValue = value;
            }
        }

        public static implicit operator float(JackIn j)
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