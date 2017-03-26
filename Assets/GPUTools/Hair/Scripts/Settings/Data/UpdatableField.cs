using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Data
{
    [Serializable]
    public class UpdatableField<T> where T : struct
    {
        [SerializeField]
        public T Value;
    }
}
