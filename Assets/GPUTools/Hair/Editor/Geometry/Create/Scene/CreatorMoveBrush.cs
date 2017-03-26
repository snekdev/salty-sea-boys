using GPUTools.Common.Tools;
using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Scene
{
    public class CreatorMoveBrush : CreatorBaseBrush
    {
        private Vector3 oldPosition;

        public CreatorMoveBrush(HairGeometryCreator creator) : base(creator)
        {

        }

        public override void StartDrawScene()
        {
            oldPosition = Creator.Brush.Position;
        }

        public override void DrawScene()
        {
            base.DrawScene();

            //ExecuteTimer.Start();

            var toWorld = Creator.ScalpProvider.ToWorldMatrix;
            var toObject = toWorld.inverse;

            var vertices = Creator.Geomery.Selected.Vertices;
            var guideVertices = Creator.Geomery.Selected.GuideVertices;

            var dir = (Creator.Brush.Position - oldPosition)*Creator.Brush.Strength;

            for (var i = 0; i < vertices.Count; i++)
            {
                if (i % Creator.Segments == 0)
                    continue;

                var vertex = vertices[i];
                var wordVertex = toWorld.MultiplyPoint3x4(vertex);

                if (Creator.Brush.Contains(wordVertex))
                    wordVertex = wordVertex + dir;

                var distance = (guideVertices[i - 1] - guideVertices[i]).magnitude;
                var topVertex = vertices[i - 1];

                wordVertex = FixCollisions(wordVertex, Colliders);
                vertex = toObject.MultiplyPoint3x4(wordVertex);
                vertex = FixDistance(topVertex, vertex, distance);
                vertices[i] = vertex;
            }

            oldPosition = Creator.Brush.Position;

            //ExecuteTimer.Log();
        }
        

        private Vector3 FixDistance(Vector3 upperVertex, Vector3 newVertex, float guideDistance)
        {
            var relPosition = upperVertex - newVertex;
            var actualDistance = relPosition.magnitude;

            var penetration = (guideDistance - actualDistance) / actualDistance;
            var correction = relPosition * penetration;
            
            return newVertex - correction;
        }
    }
}
