using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
    [Serializable]
    public class GeometryBrush
    {
        public Vector3 Position;
        public Vector3 Dirrection;
        public float Radius = 0.01f;
        public float Lenght1 = 1;
        public float Lenght2 = 1;
        public float Strength = 0.25f;
        public Color Color = Color.white;
        
        ///distance to colliders
        public float CollisionDistance = 0.01f;

        [NonSerialized]public bool IsBrushEnabled;

        public Vector3 ToWorld(Matrix4x4 m, Vector3 local)
        {
            return Position + (Vector3) (m*local);
        }

        public bool Contains(Vector3 point)
        {
            //Assert.IsNotNull(SceneView.currentDrawingSceneView, "Use this method only from editor scripts");

            var camera = Camera.current;

            var localPoint = camera.transform.InverseTransformPoint(point);
            var localPosition = camera.transform.InverseTransformPoint(Position);

            var radiusCondition = ((Vector2)localPoint - (Vector2)localPosition).magnitude < Radius;

            var depthCondition1 = (localPosition.z - localPoint.z) > -Lenght1;
            var depthCondition2 = (localPosition.z - localPoint.z) < Lenght2;

            return radiusCondition && depthCondition1 && depthCondition2;
        }
    }
}
