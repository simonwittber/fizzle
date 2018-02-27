using System.Collections.Generic;

namespace Fizzle
{
    [System.Serializable]
    public class JackSignal
    {
        public static readonly HashSet<JackSignal> instances = new HashSet<JackSignal>();

        public JackSignal()
        {
            instances.Add(this);
        }

        public int connectedId;
        public float Value
        {
            get
            {
                return Jack.GetValue(connectedId);
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