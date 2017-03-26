using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Hair.Scripts.Geometry.Tools
{
    public class ScalpProcessingTools
    {
        public static List<int> HairRootToScalpIndices(List<Vector3> scalpVertices,
            List<Vector3> hairVertices, int segments)
        {
            var resultIndices = new List<int>();

            for (var i = 0; i < hairVertices.Count; i += segments)
            {
                for (var j = 0; j < scalpVertices.Count; j++)
                {
                    if ((hairVertices[i] - scalpVertices[j]).sqrMagnitude < 0.00001f)
                    {
                        resultIndices.Add(j);
                        break;
                    }
                }
            }

            Assert.IsTrue(resultIndices.Count == hairVertices.Count/segments, "Hair geometry is not compatible with scalp");
            return resultIndices;
        }


        public static List<int> ProcessIndices(List<int> scalpIndices, List<Vector3> scalpVertices, List<List<Vector3>> hairVerticesGroups, int segments)
        {
            var hairIndices = new List<int>();

            var grouStartIndex = 0;
            foreach (var hairVertices in hairVerticesGroups)
            {
                var groupIndices = ProcessIndicesForMesh(grouStartIndex, scalpVertices, scalpIndices, hairVertices, segments);
                hairIndices.AddRange(groupIndices);

                grouStartIndex += hairVertices.Count + 1;
            }

            for (var i = 0; i < hairIndices.Count; i++)
            {
                hairIndices[i] = hairIndices[i] / segments;
            }

            return hairIndices;
        }

        private static List<int> ProcessIndicesForMesh(int startIndex, List<Vector3> scalpVertices, List<int> scalpIndices, List<Vector3> hairVertices, int segments)
        {
            var hairIndices = new List<int>();

            for (var i = 0; i < scalpIndices.Count; i++)
            {
                var index = scalpIndices[i];
                var scalpVertex = scalpVertices[index];

                if (i % 3 == 0)
                    FixNotCompletedPolygon(hairIndices);

                for (var j = 0; j < hairVertices.Count; j += segments)
                {
                    var hairVertex = hairVertices[j];

                    if ((hairVertex - scalpVertex).sqrMagnitude < 0.00001f)
                    {
                        hairIndices.Add(startIndex + j);
                        break;
                    }
                }
            }

            FixNotCompletedPolygon(hairIndices);

            return hairIndices;
        }

        private static void FixNotCompletedPolygon(List<int> hairIndices)
        {
            var countToRemove = hairIndices.Count % 3;
            if (countToRemove > 0)
                hairIndices.RemoveRange(hairIndices.Count - countToRemove, countToRemove);
        }
    }
}
