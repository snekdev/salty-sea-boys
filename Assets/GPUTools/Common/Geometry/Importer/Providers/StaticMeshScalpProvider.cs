using System;
using GPUTools.Common.Geometry.Importer.Abstract;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Common.Geometry.Importer.Providers
{
    [Serializable]
    public class StaticMeshScalpProvider : IScalpProvider
    {
        [SerializeField] public MeshFilter MeshFilter;

        private Matrix4x4[] toWorldMatrices;

        private void UpdateToWorldMatrices()
        {
            if(toWorldMatrices == null || toWorldMatrices.Length != MeshFilter.sharedMesh.vertexCount)
                toWorldMatrices = new Matrix4x4[MeshFilter.sharedMesh.vertexCount];

            for (var i = 0; i < toWorldMatrices.Length; i++)
                toWorldMatrices[i] = MeshFilter.transform.localToWorldMatrix;
        }

        public Matrix4x4[] ToWorldMatrices
        {
            get
            {
                UpdateToWorldMatrices();
                return toWorldMatrices;
            }
        }

        public Matrix4x4 ToWorldMatrix
        {
            get { return MeshFilter.transform.localToWorldMatrix; }
        }

        public ComputeBuffer ToWorldMatricesBuffer
        {
            get { return null; }
        }

        public Mesh Mesh
        {
            get
            {
                return MeshFilter.sharedMesh;
            }
        }

        public bool Validate()
        {
            Assert.IsNotNull(MeshFilter, "Add Scalp MeshFilter");
            return MeshFilter != null;
        }

        public void Dispose()
        {
            //here is nothing to dispose
        }
    }
}