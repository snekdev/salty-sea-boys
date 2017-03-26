using GPUTools.Hair.Scripts.Settings.GPBuilders.Data;
using GPUTools.Hair.Scripts.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPUTools.Hair.Scripts.Render
{
    public class HairRender : MonoBehaviour
    {
        private Mesh mesh;
        private new MeshRenderer renderer;
        private GPHairRenderData data;
        private ComputeBuffer barycentric;
        private ComputeBuffer bodiesData;

        private void Awake()
        {
            mesh = new Mesh();
            renderer = gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>().mesh = mesh;
        }

        public void Initialize(GPHairRenderData data)
        {
            this.data = data;

            Initializebuffers();
            InitializeMaterial();
            InitializeMesh();
        }

        private void InitializeMesh()
        {
            mesh.vertices = new Vector3[(int)data.Size.x];
            mesh.SetIndices(data.Indices, MeshTopology.Triangles, 0);
        }

        private void Initializebuffers()
        {
            barycentric = new ComputeBuffer(data.BarycentricWeights.Length, sizeof (float)*3);
            barycentric.SetData(data.BarycentricWeights);

            bodiesData = new ComputeBuffer(data.BodiesData.Length, BodyData.Size());
            bodiesData.SetData(data.BodiesData);
        }

        private void InitializeMaterial()
        {
            renderer.material = data.Material;
            renderer.material.SetBuffer("_BarycentricBuffer", barycentric);
            renderer.material.SetBuffer("_BodiesDataBuffer", bodiesData);
            renderer.material.SetBuffer("_BodiesBuffer", data.BodiesBuffer);
            renderer.material.SetVector("_Size", data.Size);
        }

        public void UpdateSettings()
        {
            bodiesData.SetData(data.BodiesData);
        }

        private void OnDestroy()
        {
            bodiesData.Dispose();
            barycentric.Dispose();
        }

        private void LateUpdate()
        {
            UpdateBounds();
            UpdateMaterial();
            UpdateRenderer();
        }

        private void UpdateBounds()
        {
            mesh.bounds = transform.InverseTransformBounds(data.Bounds);
        }

        private void UpdateMaterial()
        {
            renderer.material.SetVector("_LightCenter", data.LightCenter);
            renderer.material.SetVector("_TessFactor", data.TessFactor);
            renderer.material.SetFloat("_StandWidth", data.StandWidth);

            renderer.material.SetFloat("_SpecularShift", data.SpecularShift);
            renderer.material.SetFloat("_PrimarySpecular", data.PrimarySpecular);
            renderer.material.SetFloat("_SecondarySpecular", data.SecondarySpecular);
            renderer.material.SetColor("_SpecularColor", data.SpecularColor);

            renderer.material.SetVector("_WavinessAxis", data.WavinessAxis);

            renderer.material.SetVector("_Length", data.Length);
            renderer.material.SetFloat("_Volume", data.Volume);
        }

        private void UpdateRenderer()
        {
            renderer.shadowCastingMode = data.CastShadows ? ShadowCastingMode.On : ShadowCastingMode.Off;
            renderer.receiveShadows = data.ReseiveShadows;
        }
    }
}
