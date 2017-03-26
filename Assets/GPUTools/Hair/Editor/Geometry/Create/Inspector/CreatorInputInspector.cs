using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Common.Geometry.Importer;
using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Inspector
{
    public class CreatorInputInspector : EditorItemBase
    {
        private HairGeometryCreator creator;

        public CreatorInputInspector(HairGeometryCreator creator)
        {
            this.creator = creator;
        }

        public override void DrawInspector()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Label("Source Settings", EditorStyles.boldLabel);

            creator.DebugDraw = EditorGUILayout.Toggle("Debug", creator.DebugDraw);

            if (creator.Geomery.Selected == null)
            {
                creator.Segments = Mathf.Clamp(EditorGUILayout.IntField("Segments", creator.Segments), 3, 30);
            }
            else
            {
                GUILayout.Label("Segments " + creator.Segments);
            }

            ScalpProviderInspactor();

            CollidersList();

            GUILayout.EndVertical();
        }

        private void ScalpProviderInspactor()
        {
            creator.ScalpProvider.Type = (ScalpMeshType)EditorGUILayout.EnumPopup("Scalp Renderer Type", creator.ScalpProvider.Type);

            if (creator.ScalpProvider.Type == ScalpMeshType.Static)
            {
                creator.ScalpProvider.StaticProvider.MeshFilter = (MeshFilter)EditorGUILayout.ObjectField("Scalp", creator.ScalpProvider.StaticProvider.MeshFilter, typeof(MeshFilter), true);
            }
            else
            {
                creator.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Skinned Mesh Renderer", creator.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer, typeof(SkinnedMeshRenderer), true);
                creator.ScalpProvider.SkinnedProvider.Shader = (ComputeShader)EditorGUILayout.ObjectField("Skinning Shader", creator.ScalpProvider.SkinnedProvider.Shader, typeof(ComputeShader), true);
            }
        }

        private void CollidersList()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            var providers = creator.ColliderProviders;

            for (var i = 0; i < providers.Count; i++)
            {
                GUILayout.BeginHorizontal();

                providers[i] =
                    (GameObject)
                        EditorGUILayout.ObjectField("Colliders Provider", providers[i], typeof(GameObject), true);

                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
                {
                    providers.RemoveAt(i);
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();

            GUILayout.Label(providers.Count == 0 ? "Add Collider" : "");
            if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
            {
                providers.Add(null);
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
}
