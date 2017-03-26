using System;
using GPUTools.Physics.Scripts.Collisions;
using GPUTools.Physics.Scripts.Core;
using GPUTools.Physics.Scripts.Dynamics;
using GPUTools.Physics.Scripts.Joints;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Physics.Scripts.World
{
    public class GPWorldBuilder
    {
        public GPData Data = new GPData();

        private Matrix4x4[] matrices;

        private GPBody[] bodies;
        private GPSphereCollider[] sphereColliders;

        private GPPointJoint[] pointJoints;

        private GPBody[] kinematicsBodies;
        private GPSphereCollider[] kinematicsSphereColliders;

        public GPWorldBuilder()
        {
            Data.Gravity = new Vector4(0, -0.0044f, 0, 0);
            Data.Drag = 1;

            matrices = new Matrix4x4[0];

            bodies = new GPBody[0];
            sphereColliders = new GPSphereCollider[0];

            kinematicsBodies = new GPBody[0];
            kinematicsSphereColliders = new GPSphereCollider[0];

            pointJoints = new GPPointJoint[0];

/*            Data.Matrices = matrices;
            Data.Bodies = bodies;
            Data.SphereColliders = sphereColliders;
            Data.KinematicsBodies = kinematicsBodies;
            Data.KinematicsSphereColliders = kinematicsSphereColliders;
            Data.DistanceJoints = new GroupedData<GPDistanceJoint>();
            Data.PointJoints = pointJoints;*/
        }

        public void AddSphere(GPBody body, float radius, int groupId)
        {
            var bodyId = bodies.Length;
            var collider = new GPSphereCollider(bodyId, radius);

            ArrayUtils.Add(ref bodies, body);
            ArrayUtils.Add(ref sphereColliders, collider);
        }

        public void AddKinematicsSphere(GPBody body, float radius, int groupId)
        {
            var bodyId = kinematicsBodies.Length;
            var collider = new GPSphereCollider(bodyId, radius);

            ArrayUtils.Add(ref kinematicsBodies, body);
            ArrayUtils.Add(ref kinematicsSphereColliders, collider);
        }

        public void AddDitanceJoint(GPBody body1, GPBody body2, float distance)
        {
            var body1Id = Array.IndexOf(bodies, body1);
            var body2Id = Array.IndexOf(bodies, body2);

            Assert.IsTrue(body1Id >= 0 && body2Id >= 0, "Add body to world first");

            var joint = new GPDistanceJoint(body1Id, body2Id, distance, 1);
            Data.DistanceJoints.Add(joint);
        }

        public void AddDitanceJoint(int body1Id, int body2Id, float distance)
        {
            Assert.IsTrue(body1Id >= 0 && body2Id >= 0, "Add body to world first");

            var joint = new GPDistanceJoint(body1Id, body2Id, distance, 1);
            Data.DistanceJoints.Add(joint);
        }

        public void AddPointJoint(GPBody body, Vector3 point, Matrix4x4 matrix, float elasticity)
        {
            var bodyId = Array.IndexOf(bodies, body);
            
            Assert.IsTrue(bodyId >= 0, "Add body to world first");

            var joint = new GPPointJoint(bodyId, matrices.Length, point, elasticity);
            ArrayUtils.Add(ref pointJoints, joint);
            ArrayUtils.Add(ref matrices, matrix);
        }
    }
}
