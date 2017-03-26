namespace GPUTools.Physics.Scripts.Collisions
{
    public struct GPSphereCollider
    {
        public int BodyId;
        public float Radius;

        public GPSphereCollider(int bodyId, float radius)
        {
            BodyId = bodyId;
            Radius = radius;
        }

        public static int Size()
        {
            return sizeof(float) + sizeof(int);
        }
    }
}
