using System.Collections.Generic;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class JackOut
    {
        public static readonly HashSet<JackOut> instances = new HashSet<JackOut>();

        public uint id;

        public JackOut()
        {
            instances.Add(this);
        }

        public float Value
        {
            set
            {
                Jack.values[id] = value;
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name}:{id}";
        }
    }



}