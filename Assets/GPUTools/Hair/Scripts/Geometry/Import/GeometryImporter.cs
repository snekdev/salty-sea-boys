using System.Collections.Generic;
using System.Linq;
using GPUTools.Common.Geometry.Importer;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.Tools;
using UnityEngine;

#pragma warning disable 649

namespace GPUTools.Hair.Scripts.Geometry.Import
{
    [ExecuteInEditMode]
    public class GeometryImporter : GeometryProviderBase
    {
        [SerializeField] public bool DebugDraw = true;

        [SerializeField] public int Segments = 5;
        [SerializeField] public HairGroupsProvider HairGroupsProvider = new HairGroupsProvider();
        [SerializeField] public ScalpMeshProvider ScalpProvider = new ScalpMeshProvider();

        [SerializeField] private int[] indices;
        [SerializeField] private int[] hairStandRootToScalpIndexMap;

        private bool Validate()
        {
            if (!ScalpProvider.Validate())
                return false;

            return HairGroupsProvider.Validate();
        }

        public void Process()
        {
            if(!Validate())
                return;
       
            HairGroupsProvider.Process(ScalpProvider.ToWorldMatrices[0].inverse);    

            indices = ProcessIndices();
            hairStandRootToScalpIndexMap = ProcessMap();
        }

        private void OnDestroy()
        {
            ScalpProvider.Dispose();
        }

        private int[] ProcessMap()
        {
            var scalpVertices = ScalpProvider.Mesh.vertices.ToList();
            return ScalpProcessingTools.HairRootToScalpIndices(scalpVertices, HairGroupsProvider.Vertices, GetSegments()).ToArray();
        }

        private int[] ProcessIndices()
        {
            var scalpIndices = ScalpProvider.Mesh.GetIndices(0).ToList();
            var scalpVertices = ScalpProvider.Mesh.vertices.ToList();

            return ScalpProcessingTools.ProcessIndices(scalpIndices, scalpVertices, HairGroupsProvider.VerticesGroups, GetSegments())/*.GetRange(144, 9)*/.ToArray();
        }

        private Matrix4x4[] transforms;

        public override Matrix4x4[] GetTransforms() //todo possible duplicate
        {
            return ScalpProvider.Type == ScalpMeshType.Skinned ? GetTransformsSkinned() : GetTransformsStatic();
        }

        public Matrix4x4[] GetTransformsSkinned()
        {
            if (transforms == null)
                transforms = new Matrix4x4[hairStandRootToScalpIndexMap.Length];

            var scalpToWorldMatices = ScalpProvider.ToWorldMatrices;

            for (var i = 0; i < hairStandRootToScalpIndexMap.Length; i++)
            {
                var hairRootToScalpIndex = hairStandRootToScalpIndexMap[i];
                transforms[i] = scalpToWorldMatices[hairRootToScalpIndex];
            }

            return transforms;
        }

        public Matrix4x4[] GetTransformsStatic()
        {
            if (transforms == null)
                transforms = new Matrix4x4[1];

            transforms[0] =  ScalpProvider.ToWorldMatrix;
            return transforms;
        }

        public override ScalpMeshType GetScalpMeshType()
        {
            return ScalpProvider.Type;
        }

        public override int GetSegments()
        {
            return Segments;
        }

        public override int[] GetIndices()
        {
            return indices;
        }

        public override List<Vector3> GetVertices()
        {
            return HairGroupsProvider.Vertices;
        }

        public override List<Color> GetColors()
        {
            return HairGroupsProvider.Colors;
        }

        #region Draw

        private void OnDrawGizmos()
        {
            if(!DebugDraw || GetVertices() == null || !Validate())
                return;

            var scalpToWorld = ScalpProvider.ToWorldMatrices[0];
            var vertices = GetVertices();

            for (var i = 1; i < vertices.Count; i++)
            {
                if (i % Segments == 0)
                    continue;

                var vertex1 = scalpToWorld.MultiplyPoint3x4(vertices[i - 1]);
                var vertex2 = scalpToWorld.MultiplyPoint3x4(vertices[i]);

                Gizmos.DrawLine(vertex1, vertex2);
            }
        }

        #endregion
    }
}
