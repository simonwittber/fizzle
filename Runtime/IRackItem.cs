namespace Fizzle
{
    public interface IRackItem
    {
        void OnAddToRack(FizzleSynth fs);
        void OnRemoveFromRack(FizzleSynth fs);
        float Sample(float[] jacks, int t);
        void OnAudioStart(FizzleSynth fs);
    }

    public abstract class RackItem
    {
        public int sortOrder = 0;
    }

}