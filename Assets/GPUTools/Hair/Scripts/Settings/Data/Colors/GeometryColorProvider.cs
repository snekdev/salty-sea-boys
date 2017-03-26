using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Data.Colors
{
    [Serializable]
    public class GeometryColorProvider : IColorProvider
    {
        public Color GetColor(HairSettings settings, int x, int y, int sizeY)
        {
            var colors = settings.StandsSettings.Provider.GetColors();
            var i = x*sizeY + y;

            return colors[i];
        }
    }
}
