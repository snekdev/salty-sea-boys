using System;
using GPUTools.Hair.Scripts.Settings.Data.Abstract;
using GPUTools.Hair.Scripts.Settings.Data.Colors;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Hair.Scripts.Settings.Data
{
    public enum ColorProviderType { RootTip, List, Geometry }

    [Serializable]
    public class HairRenderSettings : HairSettingsBase
    {
        public Material HairMaterial;

        //color
        public ColorProviderType ColorProviderType = ColorProviderType.RootTip;
        public RootTipColorProvider RootTipColorProvider;
        public ListColorProvider ListColorProvider;
        public GeometryColorProvider GeometryColorProvider;

        //specular
        public float PrimarySpecular = 50;
        public float SecondarySpecular = 50;
        public Color SpecularColor = new Color(0.15f, 0.15f, 0.15f);

        //lenght
        public float Length1 = 1;
        public float Length2 = 1;
        public float Length3 = 1;

        //waviness
        public float WavinessScale = 0;
        public AnimationCurve WavinessScaleCurve = AnimationCurve.EaseInOut(0,0,1,1);
        public float WavinessFrequency = 0;
        public AnimationCurve WavinessFrequencyCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public Vector3 WavinessAxis = new Vector3(1,0,0);

        //interpoation
        public AnimationCurve InterpolationCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);

        //volume 
        public float Volume = 0;

        public override void Validate()
        {
            Assert.IsNotNull(HairMaterial, "Add material to render settings");
        }

        public IColorProvider ColorProvider
        {
            get
            {
                if (ColorProviderType == ColorProviderType.RootTip)
                    return RootTipColorProvider;
                if (ColorProviderType == ColorProviderType.List)
                    return ListColorProvider;

                return GeometryColorProvider;
            }
        }

    }
}
