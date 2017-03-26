using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Settings;
using GPUTools.Hair.Scripts.Settings.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Settings.Inspector
{
    public class HairLODInspector : EditorItemBase
    {
        private HairSettings settings;

        public HairLODInspector(HairSettings settings)
        {
            this.settings = settings;
        }

        public override void DrawInspector()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("LOD Settings", EditorStyles.boldLabel);

            GUILayout.BeginVertical(EditorStyles.helpBox);
            Lod.StartDistance = EditorGUILayout.FloatField("Start Distance", Lod.StartDistance);
            Lod.EndDistance = EditorGUILayout.FloatField("End Distance", Lod.EndDistance);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            Lod.DensityMin = EditorGUILayout.FloatField("Min Dencity", Lod.DensityMin);
            Lod.DensityMax = EditorGUILayout.FloatField("Max Dencity", Lod.DensityMax);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            Lod.DetailMin = EditorGUILayout.FloatField("Min Detail", Lod.DetailMin);
            Lod.DetailMax = EditorGUILayout.FloatField("Max Detail", Lod.DetailMax);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            Lod.WidthMin = EditorGUILayout.FloatField("Min Width", Lod.WidthMin);
            Lod.WidthMax = EditorGUILayout.FloatField("Max Width", Lod.WidthMax);
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        public HairLODSettings Lod
        {
            get { return settings.LODSettings; }
        }
    }
}
