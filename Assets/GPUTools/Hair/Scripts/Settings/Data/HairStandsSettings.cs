using System;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Settings.Data.Abstract;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Hair.Scripts.Settings.Data
{
    /// <summary>
    /// Editor settings for hair geometry 
    /// </summary>
    [Serializable]
    public class HairStandsSettings : HairSettingsBase
    {   
        /// <summary>
        /// Provide geometry gameobject, it should have 2 children
        /// 1) Hair
        /// 2) Scalp
        /// Each child should have mesh filter with mesh on it
        /// </summary>
        public GeometryProviderBase Provider;
        public Vector3 HeadCenter;
        public Vector3 BoundsSize = Vector3.one*2;

        public int Segments
        {
            get { return Provider.GetSegments(); }
        }

        public Vector3 HeadCenterWorld
        {
            get
            {
                return Provider.transform.TransformPoint(HeadCenter);
            }
        }

        public override void Validate()
        {
            Assert.IsNotNull(Provider);
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(HeadCenterWorld, 0.025f);
            Gizmos.DrawWireCube(HeadCenterWorld, BoundsSize);
        }
    }
}
