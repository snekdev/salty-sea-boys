using GPUTools.Physics.Scripts.World;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Physics
{
    public class GPHairPhysics : MonoBehaviour
    {
        private GPWorld world;

        public void Initialize(GPData data)
        {
            world = new GPWorld(Instantiate(data.Shader), data);
        }

        public void UpdateSettings()
        {
            world.UpdateAllBuffers();
        }

        private void LateUpdate()
        {
            world.Update();
        }

        private void OnDestroy()
        {
            world.Dispose();
        }

        private void OnDrawGizmos()
        {
            world.DebugDraw();
        }

        public ComputeBuffer GetBodiesBuffer()
        {
            return world.Wraper.GetBuffer("bodies");
        }
    }
}
