using UnityEngine;

namespace GPUTools.Physics.Scripts.Core
{
    public class ComputeBufferWrapper
    {
        private readonly UpdatableArray array;
        private ComputeBuffer buffer;

        public ComputeBufferWrapper(UpdatableArray array, int stride)
        {
            this.array = array;

            buffer = new ComputeBuffer(array.Data.Length, stride);
            buffer.SetData(array.Data);

            array.ChangedEvent += OnArrayChanged;
        }

        public void Dispose()
        {
            array.ChangedEvent -= OnArrayChanged;
            buffer.Dispose();
        }

        private void OnArrayChanged()
        {
            buffer.SetData(array.Data);
        }

        public void Flush()
        {
            buffer.GetData(array.Data);
        }

        public ComputeBuffer ComputeBuffer
        {
            get { return buffer; }
        }
    }
}
