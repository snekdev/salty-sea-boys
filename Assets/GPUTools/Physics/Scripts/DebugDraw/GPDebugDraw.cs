using GPUTools.Physics.Scripts.Core;
using GPUTools.Physics.Scripts.Dynamics;
using GPUTools.Physics.Scripts.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.DebugDraw
{
    public class GPDebugDraw
    {
        private readonly ComputeWrapper wrapper;

        private GPBody[] bodies;
        private GPDistanceJoint[] joints;

        public GPDebugDraw(ComputeWrapper wrapper)
        {
            this.wrapper = wrapper;

            CacheBuffers();
        }

        private void CacheBuffers()
        {
            bodies = new GPBody[wrapper.GetBuffer("bodies").count];

            var jointsBuffer = wrapper.GetBuffer("distanceJoints");
            joints = new GPDistanceJoint[jointsBuffer.count];
            jointsBuffer.GetData(joints);
        }

        private void DrawBodies()
        {
            wrapper.GetBuffer("bodies").GetData(bodies);

            Gizmos.color = Color.green;
            foreach (var joint in joints)
            {
                var b1 = bodies[joint.Body1Id];
                var b2 = bodies[joint.Body2Id];

                Gizmos.DrawLine(b1.Position, b2.Position);
            }

            /*foreach (var body in bodies)
            {
                Gizmos.DrawWireSphere(body.Position, 0.01f);
            }*/
        }

        public void Draw()
        {
            DrawBodies();
        }

    }
}
