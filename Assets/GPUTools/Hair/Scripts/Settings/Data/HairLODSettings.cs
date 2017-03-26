using System;
using GPUTools.Hair.Scripts.Settings.Data.Abstract;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Data
{
    /// <summary>
    /// Level of detail settings
    /// </summary>
    [Serializable]
    public class HairLODSettings : HairSettingsBase
    {
        public float DensityMin = 4;
        public float DensityMax = 8;
        public float DetailMin = 4;
        public float DetailMax = 16;
        public float WidthMin = 0.0004f;
        public float WidthMax = 0.002f;
        public float StartDistance = 2;
        public float EndDistance = 6;

        public float GetWidth(Vector3 position)
        {
            return Mathf.Lerp(WidthMax, WidthMin, 1 - GetDistanceK(position));
        }

        public int GetDencity(Vector3 position)
        {
            return (int)Mathf.Lerp(DensityMax, DensityMin, GetDistanceK(position));
        }

        public int GetDetail(Vector3 position)
        {
            return (int)Mathf.Lerp(DetailMax, DetailMin, GetDistanceK(position));
        }

        public float GetDistanceK(Vector3 position)
        {
            var k = (GetDistanceToCamera(position) - StartDistance) /(EndDistance - StartDistance);

            return Mathf.Clamp(k, 0, 1);
        }

        public float GetDistanceToCamera(Vector3 position)
        {
            return (position - Camera.main.transform.position).magnitude;
        }
    }
}
