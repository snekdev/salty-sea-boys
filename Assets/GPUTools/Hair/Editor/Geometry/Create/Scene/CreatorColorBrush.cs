using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Scene
{
    public class CreatorColorBrush : CreatorBaseBrush
    {
        public CreatorColorBrush(HairGeometryCreator creator) : base(creator)
        {

        }

        public override void DrawScene()
        {
            var toWorld = Creator.ScalpProvider.ToWorldMatrix;
            var vertices = Creator.Geomery.Selected.Vertices;
            var colors = Creator.Geomery.Selected.Colors;

            for (var i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                var wordVertex = toWorld.MultiplyPoint3x4(vertex);

                if (Creator.Brush.Contains(wordVertex))
                {
                    colors[i] = Color.Lerp(colors[i], Creator.Brush.Color, Creator.Brush.Strength*0.1f);
                }
            }

        }

    }
}
