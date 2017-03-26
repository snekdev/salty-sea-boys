using System;
using GPUTools.Common.Geometry.Importer.Abstract;
using GPUTools.Common.Geometry.Importer.Providers;
using UnityEngine;

namespace GPUTools.Common.Geometry.Importer
{
    public enum ScalpMeshType
    {
        Static, Skinned
    }

    [Serializable]
    public class ScalpMeshProvider : IScalpProvider
    {
        public ScalpMeshType Type = ScalpMeshType.Static;

        [SerializeField] public SkinnedMeshScalpProvider SkinnedProvider;
        [SerializeField] public StaticMeshScalpProvider StaticProvider;

        public bool Validate()
        {
            return GetCurrentProvider().Validate();
        }

        public void Dispose()
        {
            GetCurrentProvider().Dispose();
        }

        private IScalpProvider GetCurrentProvider()
        {
            if (Type == ScalpMeshType.Static)
                return StaticProvider;

            return SkinnedProvider;
        }

        public Matrix4x4[] ToWorldMatrices
        {
            get { return GetCurrentProvider().ToWorldMatrices; }
        }

        public Matrix4x4 ToWorldMatrix
        {
            get { return GetCurrentProvider().ToWorldMatrix; }
        }

        public ComputeBuffer ToWorldMatricesBuffer
        {
            get { return GetCurrentProvider().ToWorldMatricesBuffer; }
        }

        public Mesh Mesh
        {
            get { return GetCurrentProvider().Mesh; }
        }
    }
}
