using UnityEngine;

namespace GPUTools.Physics.Scripts.Collisions
{
    public class CapsuleTests : MonoBehaviour
    {
        [SerializeField] private Transform ca;
        [SerializeField] private Transform cb;
        [SerializeField] private Transform s;

        [SerializeField] private float rA;
        [SerializeField] private float rB;
        [SerializeField] private float rC;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Gizmos.DrawLine(a, b);
            Gizmos.DrawWireSphere(a, rA);
            Gizmos.DrawWireSphere(b, rB);


            var v = a - b;
            var w = a - c;
            var projWv = Vector3.Dot(v, w)/v.sqrMagnitude;
            var p = Vector3.Lerp(a, b, projWv);
            var rP = Mathf.Lerp(rA, rB, projWv);
            var isCollision = (c - p).magnitude > rC + rP;

            Gizmos.DrawWireSphere(p, rP);
            Gizmos.DrawLine(p, c);

            Gizmos.color = isCollision ? Color.green : Color.red;
            Gizmos.DrawWireSphere(c, rC);
        }

        private Vector3 a
        {
            get { return ca.position; }
        }
        private Vector3 b
        {
            get { return cb.position; }
        }
        private Vector3 c
        {
            get { return s.position; }
        }


    }
}
