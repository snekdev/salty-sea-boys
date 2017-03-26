using UnityEngine;

namespace GPUTools.Physics.Scripts.Collisions
{
    public struct GPLineSphereCollider
    {
        public int BodyId;

        public Vector3 A;
        public Vector3 B;

        public float RadiusA;
        public float RadiusB;

        public GPLineSphereCollider(int bodyId, Vector3 a, Vector3 b, float radiusA, float radiusB)
        {
            BodyId = bodyId;
            A = a;
            B = b;
            RadiusA = radiusA;
            RadiusB = radiusB;
        }

        public static int Size()
        {
            return sizeof(int) + sizeof(float)*8;
        }
    }
}
