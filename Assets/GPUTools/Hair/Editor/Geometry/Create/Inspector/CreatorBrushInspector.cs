using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Inspector
{
    public class CreatorBrushInspector : EditorItemBase
    {
        private HairGeometryCreator creator;
        private const int Width = 330;
        private const int Height = 215;

        public CreatorBrushInspector(HairGeometryCreator creator)
        {
            this.creator = creator;
        }

        public override void DrawInspector()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Brush Settings", EditorStyles.boldLabel);
            creator.Brush.Color = EditorGUILayout.ColorField("Brush Color", creator.Brush.Color);
            GUILayout.EndVertical();
    }

        public override void DrawScene()
        {
            if (creator.Geomery.Selected == null)
                return;

            Handles.BeginGUI();

            GUILayout.BeginArea(GetWindowRect(), EditorStyles.helpBox);
            var rect = EditorGUILayout.BeginVertical();

            DrawTitle();
            DrawSeparator();
            DrawSettings();
            DrawSeparator();
            DrawBrushBehaviour();

            EditorGUILayout.EndVertical();
            
            GUI.backgroundColor = Color.clear;
            if (GUI.Button(rect, "", EditorStyles.helpBox)){ }

            GUILayout.EndArea();
            Handles.EndGUI();
        }

        private void DrawSettings()
        {
            var brush = creator.Brush;
            brush.Radius = EditorGUILayout.Slider("Radius", brush.Radius, 0, 1);
            brush.Strength = EditorGUILayout.Slider("Strength", brush.Strength, 0, 1);
            brush.CollisionDistance = EditorGUILayout.Slider("Collision Distance", brush.CollisionDistance, 0, 1);
            brush.Lenght1 = EditorGUILayout.Slider("Lenght Front", brush.Lenght1, 0, 1);
            brush.Lenght2 = EditorGUILayout.Slider("Lenght Back", brush.Lenght2, 0, 1);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Color");
            GUI.backgroundColor = brush.Color;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
        }

        private void DrawTitle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Brush Settings", EditorStyles.boldLabel);

            if (creator.Geomery.Selected.IsRedo && GUILayout.Button("Redo", GUILayout.Width(50)))
                creator.Geomery.Selected.Redo();

            if (creator.Geomery.Selected.IsUndo && GUILayout.Button("Undo", GUILayout.Width(50)))
                creator.Geomery.Selected.Undo();

            GUILayout.EndHorizontal();
        }

        private void DrawBrushBehaviour()
        {
            if (IsBrushEnabled())
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Press and hold key to use the brush");

                GUILayout.BeginHorizontal();
                GUILayout.Label("'M' Move");
                GUILayout.Label("'R' Remove");
                GUILayout.Label("'S' Shrink");
                GUILayout.Label("'G' Grow");
                GUILayout.Label("'C' Color");
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            else if (GUILayout.Button("Enable Brush"))
            {
                EnableBrush();
            }
        }

        private void DrawSeparator()
        {
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        }

        private void EnableBrush()
        {
            if (SceneView.sceneViews.Count > 0)
            {
                var sceneView = (SceneView)SceneView.sceneViews[0];
                sceneView.Focus();
                sceneView.orthographic = true;
            }
        }

        public bool IsBrushEnabled()
        {
            if (!Application.isPlaying && SceneView.sceneViews.Count > 0 && EditorWindow.focusedWindow != null)
            {
                var sceneView = (SceneView)SceneView.sceneViews[0];
                return sceneView.orthographic && sceneView.ToString() == EditorWindow.focusedWindow.ToString();
            }

            return false;
        }

        public Rect GetWindowRect()
        {
            return new Rect(Screen.width - Width - 10, Screen.height - Height - 45, Width, Height);
        }
    }
}
