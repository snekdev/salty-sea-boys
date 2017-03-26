using UnityEngine;

namespace GPUTools.Physics.Scripts.Dynamics
{
    public struct GPBody
    {
        public Vector3 Position;
        public Vector3 LastPosition;

        public GPBody(Vector3 position)
        {
            Position = position;
            LastPosition = position;
        }

        public static int Size()
        {
            return sizeof(float)*6;
        }
    }
}
