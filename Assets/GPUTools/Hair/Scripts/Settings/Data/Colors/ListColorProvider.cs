using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Data.Colors
{
    [Serializable]
    public class ListColorProvider : IColorProvider
    {
        public List<Color> Colors = new List<Color>();

        public Color GetColor(HairSettings settings, int x, int y, int sizeY)
        {
            return GetStandColor((float) y/sizeY);
        }

        private Color GetStandColor(float t)
        {
            var i = Colors.Count*t;
            var iClamped = (int) Mathf.Clamp(i, 0, Colors.Count - 1);
            return Colors[iClamped];
        }
    }
}
