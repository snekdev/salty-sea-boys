using System.Collections.Generic;
using GPUTools.Common.Geometry.Importer;
using GPUTools.Hair.Scripts.Geometry.Import;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Import
{
    [CustomEditor(typeof(GeometryImporter))]
    public class GeometryImporterEditor : UnityEditor.Editor
    {
        private GeometryImporter settings;
        private string sugestedSegments;

        private void OnEnable()
        {
            settings = target as GeometryImporter;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            settings.DebugDraw = EditorGUILayout.Toggle("Debug Draw", settings.DebugDraw);
            settings.Segments = EditorGUILayout.IntSlider("Segments", settings.Segments, 3, 25);
            ScalpProviderInspactor();
            HairMeshFiltersList();

            if (GUILayout.Button("GenerateControl"))
            {
                settings.Process();
                sugestedSegments = "Suggested segments values is:";
                for (int i = 3; i < 2000; i++)
                {
                    var count = settings.HairGroupsProvider.Vertices.Count;
                    if ((count % i == 0) && (count / i < 26) && (count / i < 26))
                        sugestedSegments += count / i + " ";
                }

            }

            GUILayout.Label(sugestedSegments);

            GUILayout.EndVertical();
        }
        
        private void ScalpProviderInspactor()
        {
            //settings.ScalpProvider.Type = (ScalpMeshType)EditorGUILayout.EnumPopup("Scalp Renderer Type", settings.ScalpProvider.Type);

            if (settings.ScalpProvider.Type == ScalpMeshType.Static)
            {
                settings.ScalpProvider.StaticProvider.MeshFilter = (MeshFilter)EditorGUILayout.ObjectField("Scalp Hair Mesh Filter", settings.ScalpProvider.StaticProvider.MeshFilter, typeof(MeshFilter), true);
            }
            else
            {
                settings.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Skinned Mesh Renderer", settings.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer, typeof(SkinnedMeshRenderer), true);
                settings.ScalpProvider.SkinnedProvider.Shader = (ComputeShader)EditorGUILayout.ObjectField("Skinning Shader", settings.ScalpProvider.SkinnedProvider.Shader, typeof(ComputeShader), true);
            }
        }

        private void HairMeshFiltersList()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            if (settings.HairGroupsProvider.HairFilters == null)
                settings.HairGroupsProvider.HairFilters = new List<MeshFilter>();

            var filters = settings.HairGroupsProvider.HairFilters;
            if (filters != null)
            {
                for (int i = 0; i < filters.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    filters[i] =
                        (MeshFilter)
                            EditorGUILayout.ObjectField("Hair Mesh Filter", filters[i], typeof(MeshFilter), true);

                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
                    {
                        filters.RemoveAt(i);
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.BeginHorizontal();

            GUILayout.Label(filters == null || filters.Count == 0 ? "Add Hair Mesh Filter" : "");
            if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
            {
                filters.Add(null);
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void OnSceneGUI()
        {

        }
    }
}
