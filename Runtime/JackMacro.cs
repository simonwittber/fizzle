namespace Fizzle
{
    [System.Serializable]
    public struct JackMacro
    {
        public uint id;
        public string name;
        public float localValue;

        public float Value(float[] jacks)
        {
            return localValue;
        }

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