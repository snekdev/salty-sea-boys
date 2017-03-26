using System.Collections.Generic;
using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Inspector
{
    public class CreatorGroupInspector : EditorItemBase
    {
        private HairGeometryCreator creator;

        public CreatorGroupInspector(HairGeometryCreator creator)
        {
            this.creator = creator;
        }

        public override void DrawInspector()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Geometry Settings", EditorStyles.boldLabel);
            DrawGroupsList();
            GUILayout.EndVertical();
        }

        private void DrawGroupsList()
        {
            var geomery = creator.Geomery;
            if (geomery != null)
            {
                for (int i = 0; i < geomery.List.Count; i++)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    if (creator.Geomery.SelectedIndex == i)
                    {
                        DrawSelectedGroup(geomery.List, i);
                    }
                    else
                    {
                        DrawGroupButton(geomery.List, i);
                    }
                    GUILayout.EndVertical();
                }
            }

            GUILayout.BeginHorizontal();

            GUILayout.Label(geomery == null || geomery.List.Count == 0 ? "Add Group" : "");

            if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
            {
                geomery.List.Add(new GeometryGroupData());
                geomery.SelectedIndex = geomery.List.Count - 1;
            }

            GUILayout.EndHorizontal();

        }

        private void DrawSelectedGroup(List<GeometryGroupData> list, int i)
        {
            var data = list[i];

            GUILayout.BeginHorizontal();

            GUILayout.Label("< Geometry Group " + i);
            if (GUILayout.Button("edit", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxWidth(40)))
            {}

            if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
            {
                list.RemoveAt(i);
                creator.Geomery.SelectedIndex = 0;
            }
            GUILayout.EndHorizontal();

            if (data.Vertices == null)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);

                data.Length = Mathf.Clamp(EditorGUILayout.FloatField("Length", data.Length), 0, 100);

                if (GUILayout.Button(data.Vertices == null ? "GenerateControl" : "Reset"))
                {
                    if (data.Vertices == null)
                    {
                        data.Generate(creator.ScalpProvider.Mesh,  creator.Segments);
                    }
                    else
                    {
                        data.Reset();
                    }
                }

                GUILayout.EndVertical();
            }
        }

        private void DrawGroupButton(List<GeometryGroupData> list, int i)
        {
            GUILayout.BeginHorizontal();


            GUILayout.Label("Geometry Group " + i);

            if (GUILayout.Button("edit", EditorStyles.miniButton, GUILayout.MaxWidth(40)))
                creator.Geomery.SelectedIndex = i;

            if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
                list.RemoveAt(i);

            GUILayout.EndHorizontal();
        }
    }
}
