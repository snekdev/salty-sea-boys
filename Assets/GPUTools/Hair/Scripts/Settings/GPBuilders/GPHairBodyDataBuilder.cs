using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Settings.GPBuilders.Data;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.GPBuilders
{
    public class GPHairBodyDataBuilder
    {
        private readonly HairSettings settings;
        private readonly GeometryProviderBase provider;
        private BodyData[] bodies;

        public GPHairBodyDataBuilder(HairSettings settings)
        {
            this.settings = settings;
            provider = settings.StandsSettings.Provider;
        }

        private void UpdateBodies()
        {
            var renderSettings = settings.RenderSettings;
            var sizeY = provider.GetSegments();

            if (bodies == null)
                bodies = new BodyData[provider.GetVertices().Count];

            for (var i = 0; i < bodies.Length; i++)
            {
                var x = i / sizeY;
                var y = i % sizeY;
                var t = (float)y/sizeY;

                var body = new BodyData
                {
                    Color = ColorToVector(renderSettings.ColorProvider.GetColor(settings, x, y, sizeY)),
                    Interpolation = Mathf.Clamp01(renderSettings.InterpolationCurve.Evaluate(t)),
                    WavinessScale = Mathf.Clamp01(renderSettings.WavinessScaleCurve.Evaluate(t))*renderSettings.WavinessScale,
                    WavinessFrequency = Mathf.Clamp01(renderSettings.WavinessFrequencyCurve.Evaluate(t))*renderSettings.WavinessFrequency
                };

                bodies[i] = body;
            }
        }

        public Vector3 ColorToVector(Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public BodyData[] BodiesData
        {
            get
            {
                UpdateBodies();
                return bodies;
            }
        }
    }
}
