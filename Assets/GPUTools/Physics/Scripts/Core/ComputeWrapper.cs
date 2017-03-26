using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Physics.Scripts.Core
{
    public class ComputeWrapper
    {
        private readonly ComputeShader shader;
        private readonly Dictionary<string, ComputeBuffer> buffers; 
        private readonly List<string> buffersNames; 

        public ComputeWrapper(ComputeShader shader)
        {
            this.shader = shader;
            buffers = new Dictionary<string, ComputeBuffer>();
            buffersNames = new List<string>();
        }

        public void AddBuffer(string name, ComputeBuffer buffer)
        {
            buffers.Add(name, buffer);
            buffersNames.Add(name);
        }

        public void CreateBuffer<T>(string name, T[] array, int stride)
        {
            Assert.IsFalse(buffers.ContainsKey(name), string.Format("ComputeBuffer {0} already created", name));

            var buffer = new ComputeBuffer(array.Length, stride);
            buffer.SetData(array);

            AddBuffer(name, buffer);
        }

        public void SetBufferData<T>(string name, T[] array)
        {
            Assert.IsTrue(buffers.ContainsKey(name));
            buffers[name].SetData(array);
        }

        public void GetBufferData<T>(string name, T[] array)
        {
            Assert.IsTrue(buffers.ContainsKey(name));
            buffers[name].GetData(array);
        }

        public ComputeBuffer GetBuffer(string name)
        {
            Assert.IsTrue(buffers.ContainsKey(name));
            return buffers[name];
        }

        public void DispatchKernel(int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ, bool assignBuffers = true)
        {
            if (assignBuffers)
            {
                AssignBuffersToKernel(kernelIndex);
            }
            shader.Dispatch(kernelIndex,threadGroupsX, threadGroupsY, threadGroupsZ);
        }

        private void AssignBuffersToKernel(int kernelIndex)
        {
            for (var i = 0; i < buffersNames.Count; i++)
            {
                var key = buffersNames[i];
                var value = buffers[key];
                shader.SetBuffer(kernelIndex, key, value);
            }
        }

        public void Dispose()
        {
            foreach (var pair in buffers)
            {
                pair.Value.Dispose();
            }
        }
    }
}
