namespace GPUTools.Physics.Scripts.Core
{
    public struct ArrayRange
    {
        public int Start;
        public int Num;
        public int End;

        public ArrayRange(int start, int num)
        {
            Start = start;
            Num = num;
            End = Start + num;
        }
    }
}
