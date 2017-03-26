using System.Collections.Generic;
using GPUTools.Common.Geometry.Importer;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Physics.Scripts.Collisions;
using GPUTools.Physics.Scripts.Core;
using GPUTools.Physics.Scripts.Dynamics;
using GPUTools.Physics.Scripts.Joints;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.GPBuilders
{
    public class GPHairStandsBuilder
    {
        private readonly HairSettings settings;
        private readonly GeometryProviderBase provider;

        private GPBody[] bodies;
        private GPSphereCollider[] sphereColliders;

        private GPPointJoint[] pointJoints;

        public GPHairStandsBuilder(HairSettings settings)
        {
            this.settings = settings;
            provider = settings.StandsSettings.Provider;

            UpdateBodies();
            CreatePointJoints();
            CreateDistanceJoints();
        }

        private void UpdateBodies()
        {
            var vertices = provider.GetVertices();
            var matrices = provider.GetTransforms();

            if(bodies == null)
                bodies = new GPBody[vertices.Count];

            for (int i = 0; i < vertices.Count; i++)
            {
                var matrix = matrices[0];
                var vertex = matrix.MultiplyPoint3x4(vertices[i]);
                bodies[i] = new GPBody(vertex);//todo maybe should be global transform 
            }
        }

        private void UpdateSphereColliders()
        {
            var radius = settings.PhysicsSettings.StandRadius*provider.transform.lossyScale.x;//todo wrong way to scale
            var vertices = provider.GetVertices();

            if(sphereColliders == null)
                sphereColliders = new GPSphereCollider[vertices.Count];

            for (var i = 0; i < vertices.Count; i++)
            {
                sphereColliders[i] = new GPSphereCollider(i, radius);
            }
        }

        private void CreateDistanceJoints()
        {
            var sizeY = settings.StandsSettings.Segments;

            var distanceJoints = new GroupedData<GPDistanceJoint>();

            var group1 = new List<GPDistanceJoint>();
            var group2 = new List<GPDistanceJoint>();

            for (int i = 0; i < bodies.Length; i++)
            {
                if (i % sizeY == 0)
                    continue;

                var body1 = bodies[i - 1];
                var body2 = bodies[i];
                var distance = Vector3.Distance(body1.Position, body2.Position);//to global

                var list = i % 2 == 0 ? group1 : group2;

                var joint = new GPDistanceJoint(i - 1, i, distance, 0.5f);
                list.Add(joint);
            }

            distanceJoints.AddGroup(group1);
            distanceJoints.AddGroup(group2);

            DistanceJoints = distanceJoints;
        }

        private void CreatePointJoints()
        {
            var vertices = provider.GetVertices();
            var sizeY = settings.StandsSettings.Segments;

            if(pointJoints == null)
                pointJoints = new GPPointJoint[vertices.Count];

            for (var i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                var matrixId = settings.StandsSettings.Provider.GetScalpMeshType() == ScalpMeshType.Skinned
                    ? i/sizeY
                    : 0;

                var t = i%sizeY;

                var elasticity = t == 0 
                    ? 1f 
                    : Mathf.Clamp01(1 - settings.PhysicsSettings.ElasticityCurve.Evaluate((float)t / sizeY));

                elasticity += JointAreaAdd(vertex);

                pointJoints[i] = new GPPointJoint(i, matrixId, vertex, Mathf.Clamp01(elasticity));
            }
        }

        private float JointAreaAdd(Vector3 vertex)
        {
            var result = 0f;

            foreach (var jointArea in settings.PhysicsSettings.JointAreas)
            {
                var diff = vertex - jointArea.transform.localPosition;
                if (diff.sqrMagnitude < jointArea.Radius*jointArea.Radius)
                {
                    result += 1;
                }
            }

            return result;
        }

        public GPBody[] Bodies
        {
            get
            {
                UpdateBodies();
                return bodies;
            }
        }

        public GPSphereCollider[] SphereColliders
        {
            get
            {
                UpdateSphereColliders();
                return sphereColliders;
            }
        }


        public GPPointJoint[] PointJoints
        {
            get
            {
                CreatePointJoints();
                return pointJoints;
            }
        }

        public GroupedData<GPDistanceJoint> DistanceJoints { private set; get; } //todo  this is parallel computing details, so should be with GPU part
    }
}
