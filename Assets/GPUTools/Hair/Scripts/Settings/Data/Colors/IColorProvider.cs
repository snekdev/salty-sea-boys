using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Data.Colors
{
    public interface IColorProvider
    {
        Color GetColor(HairSettings settings, int x, int y, int sizeY);
    }
}
