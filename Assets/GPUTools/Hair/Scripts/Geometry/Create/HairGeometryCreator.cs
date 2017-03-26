using System;
using System.Collections.Generic;
using System.Linq;
using GPUTools.Common.Geometry.Importer;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.Tools;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
    public enum ScalpRendererType { Mesh, SkinnedMesh}

    [Serializable]
    public class HairGeometryCreator : GeometryProviderBase
    {
        [SerializeField]public bool DebugDraw = false;
        [SerializeField]public int Segments = 5;
        [SerializeField]public GeometryBrush Brush = new GeometryBrush();
        [SerializeField]public ScalpMeshProvider ScalpProvider = new ScalpMeshProvider();
        [SerializeField]public List<GameObject> ColliderProviders = new List<GameObject>();
        [SerializeField]public CreatorGeometry Geomery = new CreatorGeometry();

        private int[] indices;
        private List<Vector3> vertices;
        private List<Color> colors;
        private int[] hairRootToScalpIndices;
        private Matrix4x4[] transforms;

        private void Awake()
        {
            var listVerticesGroup = new List<List<Vector3>>();
            var verticesList = new List<Vector3>();
            var colorsList = new List<Color>();

            foreach (var groupData in Geomery.List)
            {
                listVerticesGroup.Add(groupData.Vertices);
                verticesList.AddRange(groupData.Vertices);
                colorsList.AddRange(groupData.Colors);
            }

            vertices = verticesList;
            colors = colorsList;

            var scalpMesh = ScalpProvider.Mesh;
            indices = ScalpProcessingTools.ProcessIndices(scalpMesh.GetIndices(0).ToList(), scalpMesh.vertices.ToList(), listVerticesGroup, Segments).ToArray();
            hairRootToScalpIndices =
                ScalpProcessingTools.HairRootToScalpIndices(scalpMesh.vertices.ToList(), vertices, Segments).ToArray();
        }

        private void OnDestroy()
        {
            ScalpProvider.Dispose();
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
            return vertices;
        }

        public override List<Color> GetColors()
        {
            return colors;
        }

        public override Matrix4x4[] GetTransforms() //todo possible duplicate
        {
            return ScalpProvider.Type == ScalpMeshType.Skinned ? GetTransformsSkinned() : GetTransformsStatic();
        }

        public Matrix4x4[] GetTransformsSkinned()
        {
            if (transforms == null)
                transforms = new Matrix4x4[hairRootToScalpIndices.Length];

            var scalpToWorldMatrices = ScalpProvider.ToWorldMatrices;

            for (var i = 0; i < hairRootToScalpIndices.Length; i++)
            {
                var hairRootToScalpIndex = hairRootToScalpIndices[i];
                transforms[i] = scalpToWorldMatrices[hairRootToScalpIndex];
            }

            return transforms;
        }

        public Matrix4x4[] GetTransformsStatic()
        {
            if (transforms == null)
                transforms = new Matrix4x4[1];

            transforms[0] = ScalpProvider.ToWorldMatrix;
            return transforms;
        }

        public override ScalpMeshType GetScalpMeshType()
        {
            return ScalpProvider.Type;
        }

        #region DebugDraw

        private void OnDrawGizmos()
        {
            if(!DebugDraw || !ScalpProvider.Validate() )
                return;

            foreach (var data in Geomery.List)
            {
                var isSelected = Geomery.Selected == data;

                data.OnDrawGizmos(Segments, isSelected, ScalpProvider.ToWorldMatrix);
            }
        }

        #endregion
    }
}
