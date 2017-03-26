using GPUTools.Physics.Scripts.Collisions;
using GPUTools.Physics.Scripts.Core;
using GPUTools.Physics.Scripts.DebugDraw;
using GPUTools.Physics.Scripts.Dynamics;
using GPUTools.Physics.Scripts.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.World
{
    public class GPWorld
    {
        private static int integrateKernel = 0;
        private static int collisionsKernel = 1;
        private static int distanceJointsKernel = 2;
        private static int pointJointsKernel = 3;
        private static int resetKernel = 4;
        private static int bodyBroadKernel = 5;
        private static int bodyNarrowKernel = 6;

        private ComputeWrapper wraper;

        private GPData data;
        private ComputeShader shader;

        private GPDebugDraw debug;

        private int bodiesThreadGroupsNum;

        private bool assignBuffers = true;

        public GPWorld(ComputeShader shader, GPData data)
        {
            this.shader = shader;
            this.data = data;
            
            wraper = new ComputeWrapper(shader);

            InitBuffers();
            debug = new GPDebugDraw(wraper);

        }

        private void InitBuffers()
        {
            TryAddBufferOrArray("matrices", data.MatricesBuffer, data.Matrices, sizeof(float)*16);

            TryCreateBuffer("bodies", data.Bodies, GPBody.Size());
            TryCreateBuffer("sphereColliders", data.SphereColliders, GPSphereCollider.Size());

            TryCreateBuffer("kinematicBodies", data.KinematicsBodies, GPBody.Size());
            TryCreateBuffer("kinematicSphereColliders", data.KinematicsSphereColliders, GPSphereCollider.Size());
            TryCreateBuffer("kinematicsLineSphereColliders", data.KinematicsLineSphereColliders, GPLineSphereCollider.Size());

            TryCreateBuffer("distanceJoints", data.DistanceJoints.Data, GPDistanceJoint.Size());
            TryCreateBuffer("pointJoints", data.PointJoints, GPPointJoint.Size());

            TryCreateBuffer("bodyMeshVertexBuffer", data.MeshVertices, GPPointJoint.Size());
            TryCreateBuffer("narrowMeshIndexBuffer", new int[data.Bodies.Length*50], sizeof(int));

            bodiesThreadGroupsNum = Mathf.CeilToInt(wraper.GetBuffer("bodies").count / 1024f);
        }

        private void TryAddBufferOrArray<T>(string name, ComputeBuffer buffer, T[] array, int stride)
        {
            if (buffer == null)
            {
                TryCreateBuffer(name, array, stride);
                return;
            }
            
            wraper.AddBuffer(name, buffer);
        }

        private void TryCreateBuffer<T>(string name, T[] array, int stride)
        {
            if(array != null && array.Length > 0)
                wraper.CreateBuffer(name, array, stride);
        }

        private void TryUpdateBuffer<T>(string name, T[] array)
        {
            if (array != null && array.Length > 0)
                wraper.SetBufferData(name, array);
        }

        public void UpdateAllBuffers()
        {
            if (data.MatricesBuffer == null)
                TryUpdateBuffer("matrices", data.Matrices);

            TryUpdateBuffer("bodies", data.Bodies);
            TryUpdateBuffer("sphereColliders", data.SphereColliders);

            TryUpdateBuffer("kinematicBodies", data.KinematicsBodies);
            TryUpdateBuffer("kinematicSphereColliders", data.KinematicsSphereColliders);
            TryUpdateBuffer("kinematicsLineSphereColliders", data.KinematicsLineSphereColliders);

            TryUpdateBuffer("distanceJoints", data.DistanceJoints.Data);
            TryUpdateBuffer("pointJoints", data.PointJoints);
            TryUpdateBuffer("bodyMeshVertexBuffer", data.MeshVertices);
        }

        private void UpdateRuntimeBuffers()
        {
            if(data.MatricesBuffer == null)
                TryUpdateBuffer("matrices", data.Matrices);

            TryUpdateBuffer("kinematicBodies", data.KinematicsBodies);
            TryUpdateBuffer("kinematicsLineSphereColliders", data.KinematicsLineSphereColliders);
        }

        public void Update()
        {
            UpdateRuntimeBuffers();
            SetParams();

            //if(assignBuffers)
                wraper.DispatchKernel(bodyBroadKernel, bodiesThreadGroupsNum, 1, 1, assignBuffers);

            for (var j = 0; j < data.Iterations; j++)
            {
                wraper.DispatchKernel(integrateKernel, bodiesThreadGroupsNum, 1,1, assignBuffers);

                UpdateDistanceJoints();
                wraper.DispatchKernel(collisionsKernel, bodiesThreadGroupsNum, 1, 1, assignBuffers);

                wraper.DispatchKernel(pointJointsKernel, bodiesThreadGroupsNum, 1, 1, assignBuffers);
                wraper.DispatchKernel(bodyNarrowKernel, bodiesThreadGroupsNum, 1, 1, assignBuffers);

                assignBuffers = false;
            }
        }

        public void Reset()
        {
            wraper.DispatchKernel(resetKernel, bodiesThreadGroupsNum, 1, 1, assignBuffers);
        }

        private void SetParams()
        {
            shader.SetVector("gravity", data.Gravity);
            shader.SetVector("wind", data.Wind);
            shader.SetFloat("drag", data.Drag);
            shader.SetFloat("dt", 1f/data.Iterations);
        }

        private void UpdateDistanceJoints()
        {
            for (var i = 0; i < data.DistanceJoints.GroupsData.Count; i++)
            {
                var groupData = data.DistanceJoints.GroupsData[i];

                shader.SetInt("startDistanceJointGroup", groupData.Start);
                shader.SetInt("sizeDistanceJointGroup", groupData.Num);

                var threatsNum = Mathf.CeilToInt(groupData.Num/1024f);
                wraper.DispatchKernel(distanceJointsKernel, threatsNum, 1, 1, assignBuffers);
            }
        }

        public void Dispose()
        {
            wraper.Dispose();
        }

        public void DebugDraw()
        {
            if (!Application.isPlaying || !data.DebugDraw)
                return;
            
            debug.Draw();
        }

        public ComputeWrapper Wraper
        {
            get { return wraper; }
        }
    }
}
