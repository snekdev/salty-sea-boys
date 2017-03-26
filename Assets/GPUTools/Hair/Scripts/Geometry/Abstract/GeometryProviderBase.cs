using System.Collections.Generic;
using GPUTools.Common.Geometry.Importer;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Abstract
{
    public abstract class GeometryProviderBase : MonoBehaviour
    {
        public abstract int GetSegments();

        public abstract int[] GetIndices();
        public abstract List<Vector3> GetVertices();
        public abstract List<Color> GetColors();
        public abstract Matrix4x4[] GetTransforms();
        public abstract ScalpMeshType GetScalpMeshType();
    }
}
