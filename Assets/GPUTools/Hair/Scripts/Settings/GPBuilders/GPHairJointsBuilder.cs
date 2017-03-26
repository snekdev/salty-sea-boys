using System.Collections.Generic;
using GPUTools.Physics.Scripts.Joints;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.GPBuilders
{
    public class GPHairJointsBuilder
    {
        private readonly HairSettings settings;
        public GPPointJoint[] Joints { private set; get; }

        public GPHairJointsBuilder(HairSettings settings)
        {
            this.settings = settings;

            var joints = new List<GPPointJoint>();
            var vertices = settings.StandsSettings.Provider.GetVertices();
            var matrices = settings.StandsSettings.Provider.GetTransforms();

            foreach (var jointArea in settings.PhysicsSettings.JointAreas)
                joints.AddRange(ProcessJointArea(jointArea, vertices, matrices[0]));

            Joints = joints.ToArray();
        }

        private List<GPPointJoint> ProcessJointArea(HairJointArea jointArea, List<Vector3> vertices, Matrix4x4 matrix)
        {
            var result = new List<int>();
            var usedXs = new List<int>();

            for (var i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                var x = i / settings.StandsSettings.Segments;

                var diff = vertex - jointArea.transform.localPosition;//todo maybe must local for scalp

                if (diff.sqrMagnitude < Mathf.Pow(jointArea.Radius, 2) && !usedXs.Contains(x))
                {
                    result.Add(i);
                    usedXs.Add(x);
                }
            }

            return CreateJoints(result, vertices);
        }

        private List<GPPointJoint> CreateJoints(List<int> bodiesIds, List<Vector3> vertices)
        {
            var joints = new List<GPPointJoint>();

            for (var i = 0; i < bodiesIds.Count; i++)
            {
                var bodyId = bodiesIds[i];
                var vertex = vertices[bodyId];
                joints.Add(new GPPointJoint(bodyId, 0, vertex, 1));
            }

            return joints;
        }
    }
}
