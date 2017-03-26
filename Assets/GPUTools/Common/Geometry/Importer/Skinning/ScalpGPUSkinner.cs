using UnityEngine;

namespace GPUTools.Common.Geometry.Importer.Skinning
{
    public struct Weight
    {
        public int bi0;
        public int bi1;
        public int bi2;
        public int bi3;

        public float w0;
        public float w1;
        public float w2;
        public float w3;
    };

    public class ScalpGPUSkinner
    {
        public readonly ComputeBuffer SkinMatricesBuffer;
        public readonly Matrix4x4[] SkinMatrices;

        private readonly ComputeShader shader;
        private readonly SkinnedMeshRenderer skin;
        private readonly Mesh mesh;

        private readonly ComputeBuffer verticesBuffer;
        private readonly ComputeBuffer bonesBuffer;
        private readonly ComputeBuffer weightsBuffer;

        private readonly Matrix4x4[] boneMatrices;

        public ScalpGPUSkinner(SkinnedMeshRenderer skin, ComputeShader shader)
        {
            this.shader = shader;
            this.skin = skin;
            mesh = skin.sharedMesh;

            SkinMatricesBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float)*16);
            verticesBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float) * 3);
            bonesBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float) * 16);

            weightsBuffer = new ComputeBuffer(mesh.boneWeights.Length, sizeof(int) * 4 + sizeof(float) * 4);
            weightsBuffer.SetData(GetWeightsArray());

            shader.SetBuffer(0, "transformMatrices", SkinMatricesBuffer);
            shader.SetBuffer(0, "weights", weightsBuffer);
            shader.SetBuffer(0, "bones", bonesBuffer);

            boneMatrices = new Matrix4x4[skin.bones.Length];
            SkinMatrices = new Matrix4x4[mesh.vertexCount];
        }


        public void Update()
        {
            Dispatch();
            SkinMatricesBuffer.GetData(SkinMatrices);
        }

        public void Dispatch()
        {
            CalculateBones();

            verticesBuffer.SetData(mesh.vertices);
            bonesBuffer.SetData(boneMatrices);

            shader.Dispatch(0, mesh.vertexCount, 1, 1);
        }

        private void CalculateBones()
        {
            for (var i = 0; i < boneMatrices.Length; i++)
                boneMatrices[i] = skin.bones[i].localToWorldMatrix * mesh.bindposes[i];
        }

        private Weight[] GetWeightsArray()
        {
            var weights = new Weight[mesh.boneWeights.Length];
            var boneWeights = mesh.boneWeights;

            for (var i = 0; i < boneWeights.Length; i++)
            {
                var boneWeight = boneWeights[i];
                var weight = new Weight
                {
                    bi0 = boneWeight.boneIndex0,
                    bi1 = boneWeight.boneIndex1,
                    bi2 = boneWeight.boneIndex2,
                    bi3 = boneWeight.boneIndex3,

                    w0 = boneWeight.weight0,
                    w1 = boneWeight.weight1,
                    w2 = boneWeight.weight2,
                    w3 = boneWeight.weight3
                };
                weights[i] = weight;
            }

            return weights;
        }

        public void Dispose()
        {
            SkinMatricesBuffer.Dispose();
            verticesBuffer.Dispose();
            bonesBuffer.Dispose();
            weightsBuffer.Dispose();
        }
    }
}
