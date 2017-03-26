using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Data.Colors
{
    [Serializable]
    public class RootTipColorProvider: IColorProvider
    {
        public Color RootColor = new Color(0.35f, 0.15f, 0.15f);
        public Color TipColor = new Color(0.15f, 0.05f, 0.05f);
        public AnimationCurve Blend = AnimationCurve.EaseInOut(0,0,1,1);

        public Color GetColor(HairSettings settings, int x, int y, int sizeY)
        {
            return GetStandColor((float)y / sizeY);
        }


        private Color GetStandColor(float t)
        {
            var blend = Blend.Evaluate(t);
            return Color.Lerp(RootColor, TipColor, blend);
        }
    }
}
