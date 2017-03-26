using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Core
{
    public class CurveTest : MonoBehaviour
    {
        [SerializeField] private float amplitude;
        [SerializeField] private float frequency;
        [SerializeField] private Vector3 dir;

        private float an = 0;

        private Vector3 Rotate(Vector3 e, Vector3 r, float angle)
        {
            var a = e*Mathf.Sin(angle*0.5f);
            var b = Vector3.Cross(a, r);
            var q0 = Mathf.Cos(angle/2);

            return r + 2*Vector3.Cross(a, b) + 2*q0*b;
        }

        private void OnDrawGizmos()
        {
          Gizmos.color = Color.green;


          Gizmos.DrawLine(Vector3.zero, dir);

            an += 0.01f;
            var r1 = Rotate(dir.normalized, Vector3.right, an);

          Gizmos.DrawLine(Vector3.zero, dir);
          Gizmos.DrawLine(Vector3.zero, r1);
        }
    }
}
