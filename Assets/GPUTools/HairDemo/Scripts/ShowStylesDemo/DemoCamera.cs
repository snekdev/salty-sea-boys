using UnityEngine;

namespace GPUTools.HairDemo.Scripts.ShowStylesDemo
{
    public class DemoCamera : MonoBehaviour
    {
        private float radius;
        private float angle = Mathf.PI*0.5f;

        private void Awake()
        {
            radius = transform.position.z;
        }

        private void Update()
        {
            var x = Mathf.Cos(angle)*radius;
            var y = transform.position.y;
            var z = Mathf.Sin(angle)*radius;

            transform.position = new Vector3(x, y, z);
            transform.LookAt(new Vector3(0, 0.05f,0));

            HandleWheel();
            HandleMove();
        }

        private void HandleWheel()
        {
            radius += Input.GetAxis("Mouse ScrollWheel");
        }

        private void HandleMove()
        {
            if(Input.GetMouseButton(0))
                angle -= Input.GetAxis("Mouse X")*Time.deltaTime;
        }
    }
}
