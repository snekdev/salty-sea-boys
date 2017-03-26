using System;

namespace GPUTools.Physics.Scripts.Core
{
    public static class ArrayUtils
    {
        public static void Add<T>(ref T[] array, T item)
        {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = item;
        }
        

        public static void AddRange<T>(ref T[] dst, ref T[] src)
        {
            var startIndex = dst.Length;
            Array.Resize(ref dst, dst.Length + src.Length);

            for (var i = 0; i < src.Length; i++)
            {
                dst[startIndex + i] = src[i];
            }
        }
    }
}
