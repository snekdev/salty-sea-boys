using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Settings;
using GPUTools.Hair.Scripts.Settings.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Settings.Inspector
{
    public class HairStandsInspector : EditorItemBase
    {
        private HairSettings settings;

        public HairStandsInspector(HairSettings settings)
        {
            this.settings = settings;
        }

        public override void DrawInspector()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Stands Settings", EditorStyles.boldLabel);

            StandsSettings.Provider = (GeometryProviderBase)EditorGUILayout.ObjectField("Geometry Provider", StandsSettings.Provider, typeof(GeometryProviderBase), true);

            //StandsSettings.ScalpAnchor = EditorGUILayout.Vector3Field("Scalp Anchor", StandsSettings.ScalpAnchor);
            StandsSettings.HeadCenter = EditorGUILayout.Vector3Field("Head Center", StandsSettings.HeadCenter);
            StandsSettings.BoundsSize = EditorGUILayout.Vector3Field("Bounds Size", StandsSettings.BoundsSize);
            GUILayout.EndVertical();
        }

        public HairStandsSettings StandsSettings
        {
            get { return settings.StandsSettings; }
        }
    }
}
