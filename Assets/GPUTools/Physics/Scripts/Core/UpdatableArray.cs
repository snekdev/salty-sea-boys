using System;

namespace GPUTools.Physics.Scripts.Core
{
    public class UpdatableArray
    {
        public event Action ChangedEvent; 

        public Array Data { private set; get; }

        public UpdatableArray(Array data)
        {
            Data = data;
        }

        public void DispatchChanged()
        {
            if (ChangedEvent != null) ChangedEvent.Invoke();
        }
    }
}
