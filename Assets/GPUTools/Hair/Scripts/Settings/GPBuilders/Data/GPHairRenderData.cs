using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.GPBuilders.Data
{
    public class GPHairRenderData
    {
        private readonly HairSettings settings;
        private readonly ComputeBuffer bodiesBuffer;
        private readonly GPBarycentricBuilder barycentric;
        private readonly GPHairBodyDataBuilder bodiesDataBuilder;

        public GPHairRenderData(HairSettings settings, ComputeBuffer bodiesBuffer)
        {
            this.settings = settings;
            this.bodiesBuffer = bodiesBuffer;
            barycentric = new GPBarycentricBuilder();
            bodiesDataBuilder = new GPHairBodyDataBuilder(settings);
        }

        public Material Material { get { return settings.RenderSettings.HairMaterial; }}
        
        #region Geometry

        public BodyData[] BodiesData { get { return bodiesDataBuilder.BodiesData; } }

        public ComputeBuffer BodiesBuffer { get { return bodiesBuffer; }}

        public Vector3[] BarycentricWeights { get { return barycentric.Weights; }}

        public int[] Indices { get { return settings.StandsSettings.Provider.GetIndices(); }}

        public Vector4 Size
        {
            get
            {
                var sizeY = settings.StandsSettings.Provider.GetSegments();
                var sizeX = settings.StandsSettings.Provider.GetVertices().Count/
                            settings.StandsSettings.Provider.GetSegments();

                return new Vector4(sizeX, sizeY);
            }
        }

        public Bounds Bounds { get { return new Bounds(settings.StandsSettings.HeadCenterWorld, settings.StandsSettings.BoundsSize); }}

        public float Volume { get { return settings.RenderSettings.Volume; }}

        #endregion

        #region Interpolation

        public Vector3 Length { get { return new Vector4(settings.RenderSettings.Length1, settings.RenderSettings.Length2, settings.RenderSettings.Length3); }}

        #endregion

        #region LOD

        public Vector3 LightCenter { get { return settings.StandsSettings.HeadCenterWorld; }}

        public Vector3 TessFactor
        {
            get
            {
                var x = settings.LODSettings.GetDetail(LightCenter);
                var y = settings.LODSettings.GetDencity(LightCenter);
                return new Vector4(x, y, 0.99f/x, 0.99f/y);
            }
        }

        public float StandWidth { get { return settings.LODSettings.GetWidth(LightCenter); }}

        #endregion

        #region Specular

        public float SpecularShift { get { return 0.01f; }}

        public float PrimarySpecular { get { return settings.RenderSettings.PrimarySpecular; }}

        public float SecondarySpecular { get { return settings.RenderSettings.SecondarySpecular; }}

        public Color SpecularColor { get { return settings.RenderSettings.SpecularColor; }}

        #endregion

        #region Waviness

        public Vector3 WavinessAxis { get { return settings.RenderSettings.WavinessAxis; }}

        #endregion

        #region Shadows

        public bool CastShadows { get { return settings.ShadowSettings.CastShadows; } }
        public bool ReseiveShadows { get { return settings.ShadowSettings.ReseiveShadows; } }

        #endregion
    }
}
