using GPUTools.Physics.Scripts.Collisions;
using GPUTools.Physics.Scripts.Core;
using GPUTools.Physics.Scripts.Dynamics;
using GPUTools.Physics.Scripts.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.World
{
    public class GPData
    {
        public virtual bool DebugDraw { get; set; }

        public virtual int Iterations { get; set; }
        public virtual float Drag { get; set; }
        public virtual Vector3 Gravity { get; set; }
        public virtual Vector3 Wind { get; set; }
        public virtual ComputeShader Shader { get; set; }

        public virtual Matrix4x4[] Matrices { get; set; }
        public virtual ComputeBuffer MatricesBuffer { set; get; }

        public virtual GPBody[] Bodies { get; set; }
        public virtual GPSphereCollider[] SphereColliders { get; set; }

        public virtual GroupedData<GPDistanceJoint> DistanceJoints { get; set; }
        public virtual GPPointJoint[] PointJoints { get; set; }

        public virtual GPBody[] KinematicsBodies { get; set; }
        public virtual GPSphereCollider[] KinematicsSphereColliders { get; set; }
        public virtual GPLineSphereCollider[] KinematicsLineSphereColliders { get; set; }
        public virtual Vector3[] MeshVertices { get; set; }
    }
}
