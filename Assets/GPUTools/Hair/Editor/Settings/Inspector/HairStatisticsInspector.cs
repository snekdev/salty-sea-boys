using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Settings;
using GPUTools.Hair.Scripts.Settings.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Settings.Inspector
{
    public class HairStatisticsInspector : EditorItemBase
    {
        private HairSettings settings;

        public HairStatisticsInspector(HairSettings settings)
        {
            this.settings = settings;
        }

        public override void DrawInspector()
        {
            if (Application.isPlaying && StandsSettings.Provider != null && StandsSettings.Provider.GetVertices() != null)
            {
                EditorGUILayout.LabelField(string.Format("Total Particles: {0}", StandsSettings.Provider.GetVertices().Count));
                EditorGUILayout.LabelField(string.Format("Total Physics Stands: {0}",
                    StandsSettings.Provider.GetVertices().Count/StandsSettings.Provider.GetSegments()));


                var position = StandsSettings.HeadCenterWorld;

                var totalTrianglesInScalp = StandsSettings.Provider.GetIndices().Length / 3;
                var totalStands = totalTrianglesInScalp*LodSettings.GetDetail(position);
                var totalTrianglesInStand = LodSettings.GetDencity(position)*2;
                var totalTringles = totalTrianglesInStand*totalStands;

                EditorGUILayout.LabelField(string.Format("Total Render Stands: {0}", totalStands));
                EditorGUILayout.LabelField(string.Format("Total Tringles In 1 Stand: {0}", totalTrianglesInStand));
                EditorGUILayout.LabelField(string.Format("Total Tringles: {0}", totalTringles));
            }

            
        }

        public HairStandsSettings StandsSettings
        {
            get { return settings.StandsSettings; }
        }

        public HairLODSettings LodSettings
        {
            get { return settings.LODSettings; }
        }
    }
}
