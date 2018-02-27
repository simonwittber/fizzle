namespace Fizzle
{
    [System.Serializable]
    public class AudioOut
    {
        public JackSignal left = new JackSignal();
        public JackSignal right = new JackSignal();
        public JackSignal monitor = new JackSignal();
    }
}