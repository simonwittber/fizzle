namespace Fizzle
{
    public interface IHasInit
    {
        void Init();
    }

    public interface IRackItem
    {
        void OnAddToRack(FizzleSynth fs);
        void OnRemoveFromRack(FizzleSynth fs);
    }

}