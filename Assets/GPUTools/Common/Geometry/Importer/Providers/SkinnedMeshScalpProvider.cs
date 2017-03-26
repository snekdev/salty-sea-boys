using System;
using GPUTools.Common.Geometry.Importer.Abstract;
using GPUTools.Common.Geometry.Importer.Skinning;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace GPUTools.Common.Geometry.Importer.Providers
{
    [Serializable]
    public class SkinnedMeshScalpProvider : IScalpProvider
    {
        [SerializeField] public SkinnedMeshRenderer SkinnedMeshRenderer;
        [SerializeField] public ComputeShader Shader;

        private Matrix4x4[] toWorldMatrices;
        private ScalpGPUSkinner gpuSkinner;

        private void UpdateToWorldMatricesCPU()
        {
            if (toWorldMatrices == null || toWorldMatrices.Length != SkinnedMeshRenderer.sharedMesh.vertexCount)
            {
                toWorldMatrices = new Matrix4x4[SkinnedMeshRenderer.sharedMesh.vertexCount];
            }

            ScalpSkinUtils.CreateToWorldMatrices(SkinnedMeshRenderer, toWorldMatrices);
        }

        private void UpdateToWorldMatricesGPU()
        {
            if(gpuSkinner == null)
                gpuSkinner = new ScalpGPUSkinner(SkinnedMeshRenderer, Object.Instantiate(Shader));

            gpuSkinner.Update();
            toWorldMatrices = gpuSkinner.SkinMatrices;
        }

        private void UpdateToWorldMatricesBufferGPU()
        {
            if (gpuSkinner == null)
                gpuSkinner = new ScalpGPUSkinner(SkinnedMeshRenderer, Object.Instantiate(Shader));

            gpuSkinner.Dispatch();
        }

        public Matrix4x4[] ToWorldMatrices
        {
            get
            {
                if (Application.isPlaying)
                {
                    UpdateToWorldMatricesGPU();
                }
                else
                {
                    UpdateToWorldMatricesCPU();
                }


                return toWorldMatrices;
            }
        }

        public Matrix4x4 ToWorldMatrix
        {
            get { return ScalpSkinUtils.CreateToWorldMatrix(SkinnedMeshRenderer); }
        }

        public ComputeBuffer ToWorldMatricesBuffer
        {
            get
            {
                UpdateToWorldMatricesBufferGPU();
                return gpuSkinner.SkinMatricesBuffer;
            }
        }

        public Mesh Mesh
        {
            get { return SkinnedMeshRenderer.sharedMesh; }
        }

        public bool Validate()
        {
            Assert.IsNotNull(SkinnedMeshRenderer, "Add Skinned Mesh Renderer");
            Assert.IsNotNull(Shader, "Add Skinning Compute Shader");

            return SkinnedMeshRenderer != null && Shader != null;
        }

        public void Dispose()
        {
            if (gpuSkinner != null)
            {
                gpuSkinner.Dispose();
            }
        }
    }
}
