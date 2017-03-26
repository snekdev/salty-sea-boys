using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Data.Abstract
{
    [Serializable]
    public class HairShadowSettings : HairSettingsBase
    {
        [SerializeField] public bool CastShadows = true;
        [SerializeField] public bool ReseiveShadows = true;
    }
}
