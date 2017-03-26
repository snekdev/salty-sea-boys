namespace GPUTools.Physics.Scripts.Core
{
    public class CascadableArray<T>
    {
        public T[] Data;

        public ArrayRange AddData(T[] data)
        {
            var start = Data.Length;
            ArrayUtils.AddRange(ref Data, ref data);

            return new ArrayRange(start, data.Length);
        }

        public void SetData(T[] data, ArrayRange range)
        {
            for (var i = 0; i < data.Length; i++)
            {
                Data[range.Start + i] = data[i];
            }
        }

        public void GetData(ref T[] data, ArrayRange range)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = Data[range.Start + i];
            }
        }
    }
}
