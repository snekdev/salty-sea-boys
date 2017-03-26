using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Scene
{
    public class CreatorRemoveBrush : CreatorBaseBrush
    {
        public CreatorRemoveBrush(HairGeometryCreator creator) : base(creator)
        {

        }

        public override void DrawScene()
        {
            var vertices = Creator.Geomery.Selected.Vertices;
            var toWorld = Creator.ScalpProvider.ToWorldMatrix;
            for (var i = 0; i < vertices.Count; i += Creator.Segments)
            {
                if (IsBrushContainsStand(i, toWorld))
                {
                    vertices.RemoveRange(i, Creator.Segments);
                }
            }
        }

        private bool IsBrushContainsStand(int startIndex, Matrix4x4 toWorld)
        {
            var group = Creator.Geomery.Selected;
            var vertices = group.Vertices;

            for (var i = startIndex; i < startIndex + Creator.Segments; i++)
            {
                var vertex = vertices[i];
                var wordVertex = toWorld.MultiplyPoint3x4(vertex);

                if (Creator.Brush.Contains(wordVertex))
                    return true;
            }

            return false;
        }
    }
}
