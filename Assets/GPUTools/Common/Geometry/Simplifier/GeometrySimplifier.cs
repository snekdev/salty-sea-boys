using UnityEngine;

namespace GPUTools.Common.Geometry.Simplifier
{
    public class GeometrySimplifier : MonoBehaviour
    {
        [SerializeField] public MeshFilter Filter;
        public ComputeBuffer VertexBuffer { private set; get; }
        private Vector3[] vertices;

        private void Start()
        {
            vertices = Filter.mesh.vertices;

            VertexBuffer = new ComputeBuffer(vertices.Length, sizeof(float)*3);
            VertexBuffer.SetData(vertices);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            foreach (var vertex in vertices)
            {
                Gizmos.DrawSphere(vertex, 0.01f);
                //Gizmos.DrawWireSphere(vertex, 0.01f);
            }
        }
    }
}
