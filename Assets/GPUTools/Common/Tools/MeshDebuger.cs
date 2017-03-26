using UnityEngine;

namespace GPUTools.Common.Tools
{
    public class MeshDebuger : MonoBehaviour
    {
        [SerializeField] private MeshFilter filter;

        private void Start()
        {
            var vertices = filter.mesh.vertices;

            Debug.Log("VerticesNum" + vertices.Length);
        }

        private void OnDrawGizmos()
        {
            
        }
    }
}
