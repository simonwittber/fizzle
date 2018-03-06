namespace Fizzle
{
    [System.Serializable]
    public struct JackOutBank
    {
        public JackOut[] bank;
        public int index;

        public JackOutBank(int size) : this()
        {
            this.index = 0;
            this.bank = new JackOut[size];
        }
    }



}