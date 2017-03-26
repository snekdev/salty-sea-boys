using Assets.GPUTools.Common.Editor;
using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Settings;
using GPUTools.Hair.Scripts.Settings.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Settings.Inspector
{
    public class HairRenderInspector : EditorItemBase
    {
        private HairSettings settings;

        public HairRenderInspector(HairSettings settings)
        {
            this.settings = settings;
        }

        public override void DrawInspector()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Render Settings", EditorStyles.boldLabel);
            Render.HairMaterial = (Material)EditorGUILayout.ObjectField("Hair Material", Render.HairMaterial, typeof(Material), true);

            //color
            GUILayout.BeginVertical(EditorStyles.helpBox);
            Render.ColorProviderType = (ColorProviderType) EditorGUILayout.EnumPopup("Color Provider Type", Render.ColorProviderType);
            
            if(Render.ColorProviderType == ColorProviderType.RootTip)
                DrawRootTipColorProvider();
            else if(Render.ColorProviderType == ColorProviderType.List)
                DrawListColorProvider();
            else
                DrawGeometryrColorProvider();

            GUILayout.EndVertical();

            //specular
            GUILayout.BeginVertical(EditorStyles.helpBox);
            Render.PrimarySpecular = EditorGUILayout.FloatField("Primary Specular", Render.PrimarySpecular);
            Render.SecondarySpecular = EditorGUILayout.FloatField("Secondary Specular", Render.SecondarySpecular);
            Render.SpecularColor = EditorGUILayout.ColorField("Specular Color", Render.SpecularColor);
            GUILayout.EndVertical();


            //interpolation
            GUILayout.BeginVertical(EditorStyles.helpBox);
            Render.InterpolationCurve = EditorGUILayout.CurveField("Root-Tip Interpolation", Render.InterpolationCurve);
            GUILayout.EndVertical();

            //waviness
            GUILayout.BeginVertical(EditorStyles.helpBox);
            Render.WavinessScale = EditorGUILayout.FloatField("Waviness Scale", Render.WavinessScale);
            Render.WavinessScaleCurve = EditorGUILayout.CurveField("Root-Tip Scale", Render.WavinessScaleCurve);
            Render.WavinessFrequency = EditorGUILayout.FloatField("Waviness Frequency", Render.WavinessFrequency);
            Render.WavinessFrequencyCurve = EditorGUILayout.CurveField("Root-Tip Frequency", Render.WavinessFrequencyCurve);
            Render.WavinessAxis = EditorGUILayout.Vector3Field("Waviness Axis", Render.WavinessAxis);
            GUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                settings.UpdateSettings();
            }


            //lenght
            GUILayout.BeginVertical(EditorStyles.helpBox);
            Render.Length1 = Mathf.Clamp(EditorGUILayout.FloatField("Length 1", Render.Length1), 0, 1);
            Render.Length2 = Mathf.Clamp(EditorGUILayout.FloatField("Length 2", Render.Length2), 0, 1);
            Render.Length3 = Mathf.Clamp(EditorGUILayout.FloatField("Length 3", Render.Length3), 0, 1);
            GUILayout.EndVertical();

            GUILayout.EndVertical();


        }

        public void DrawRootTipColorProvider()
        {
            var provider = Render.RootTipColorProvider;
            provider.RootColor = EditorGUILayout.ColorField("Root Color", provider.RootColor);
            provider.TipColor = EditorGUILayout.ColorField("Tip Color", provider.TipColor);
            provider.Blend = EditorGUILayout.CurveField("Color Blend", provider.Blend);
        }

        public void DrawListColorProvider()
        {
            EditorDrawUtils.ListColorGUI("Color", Render.ListColorProvider.Colors);
        }

        public void DrawGeometryrColorProvider()
        {

        }

        public HairRenderSettings Render
        {
            get { return settings.RenderSettings; }
        }
    }
}
