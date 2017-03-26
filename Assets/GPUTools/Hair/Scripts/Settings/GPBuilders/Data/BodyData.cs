using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.GPBuilders.Data
{
    public struct BodyData
    {
        public Vector3 Color;
        public float Interpolation;
        public float WavinessScale;
        public float WavinessFrequency;
        
        public static int Size()
        {
            return sizeof(float) * 6;
        }
    }
}
