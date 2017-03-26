using System;

namespace GPUTools.Hair.Scripts.Settings.GPBuilders.Tools
{
    public class MultiArray<T>
    {
        private T[] fullArray = new T[0];
        private int poiner = 0;

        public void Reset()
        {
            poiner = 0;
        }

        public void AddArray(T[] array)
        {
            TryIncreaseLenght(poiner + array.Length);

            for (var i = 0; i < array.Length; i++)
            {
                fullArray[i + poiner] = array[i];
            }

            poiner += array.Length;
        }

        private void TryIncreaseLenght(int resquiresLenght)
        {
            if (resquiresLenght > fullArray.Length)
                Array.Resize(ref fullArray, resquiresLenght);
        }

        public T[] FullArray
        {
            get { return fullArray; }
        }
    }
}
