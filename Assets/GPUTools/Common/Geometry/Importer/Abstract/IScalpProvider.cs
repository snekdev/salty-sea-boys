using UnityEngine;

namespace GPUTools.Common.Geometry.Importer.Abstract
{
    public interface IScalpProvider
    {
        Matrix4x4[] ToWorldMatrices { get; }
        Matrix4x4 ToWorldMatrix { get; }
        ComputeBuffer ToWorldMatricesBuffer { get; }
        Mesh Mesh { get; }
        bool Validate();
        void Dispose();
    }
}
