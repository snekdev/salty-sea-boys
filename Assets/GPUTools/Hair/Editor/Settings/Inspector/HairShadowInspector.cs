using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Settings;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Settings.Inspector
{
    public class HairShadowInspector : EditorItemBase
    {
        private readonly HairSettings settings;

        public HairShadowInspector(HairSettings settings)
        {
            this.settings = settings;
        }

        public override void DrawInspector()
        {
            var shadow = settings.ShadowSettings;

            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Shadow Settings", EditorStyles.boldLabel);

            shadow.CastShadows = EditorGUILayout.Toggle("Cast Shadows", shadow.CastShadows);
            shadow.ReseiveShadows = EditorGUILayout.Toggle("Reseive Shadows", shadow.ReseiveShadows);

            GUILayout.EndVertical();
        }
    }
}
