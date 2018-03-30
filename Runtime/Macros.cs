namespace Fizzle
{
    [System.Serializable]
    public class Macros : RackItem, IRackItem
    {
        public JackMacro macroA = new JackMacro();
        public JackMacro macroB = new JackMacro();
        public JackMacro macroC = new JackMacro();
        public JackMacro macroD = new JackMacro();
        public JackMacro macroE = new JackMacro();

        public void OnAddToRack(FizzleSynth fs)
        {
            macroA.id = fs.TakeJackID();
            macroB.id = fs.TakeJackID();
            macroC.id = fs.TakeJackID();
            macroD.id = fs.TakeJackID();
            macroE.id = fs.TakeJackID();
        }

        public void OnAudioStart(FizzleSynth fs)
        {

        }

        public void OnRemoveFromRack(FizzleSynth fs)
        {
            fs.FreeJackID(macroA.id);
            fs.FreeJackID(macroB.id);
            fs.FreeJackID(macroC.id);
            fs.FreeJackID(macroD.id);
            fs.FreeJackID(macroE.id);
        }


        public float Sample(float[] jacks, int t)
        {
            var smp = 0f;
            macroA.Value(jacks, macroA.localValue);
            macroB.Value(jacks, macroB.localValue);
            macroC.Value(jacks, macroC.localValue);
            macroD.Value(jacks, macroD.localValue);
            macroE.Value(jacks, macroE.localValue);
            return smp;
        }
    }
}