using System.Collections.Generic;
using Assets.GPUTools.Common.Editor.Engine;
using Assets.GPUTools.Hair.Editor.Geometry.Create.Inspector;
using Assets.GPUTools.Hair.Editor.Geometry.Create.Scene;
using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create
{
    [CustomEditor(typeof(HairGeometryCreator))]
    public class HairGeometryCreatorEditor : UnityEditor.Editor
    {
        private Dictionary<KeyCode, CreatorBaseBrush> brushes = new Dictionary<KeyCode, CreatorBaseBrush>();
        private EditorInput input = new EditorInput();
        private Processor processor = new Processor();
        private HairGeometryCreator creator;

        private void OnEnable()
        {
            creator = target as HairGeometryCreator;

            processor.Add(new CreatorInputInspector(creator));
            processor.Add(new CreatorGroupInspector(creator));
            processor.Add(new CreatorBrushInspector(creator));
            processor.Add(new CreatorBrushView(creator));

            brushes.Add(KeyCode.M, new CreatorMoveBrush(creator));
            brushes.Add(KeyCode.R, new CreatorRemoveBrush(creator));
            brushes.Add(KeyCode.G, new CreatorShrinkBrush(creator, 0.1f));
            brushes.Add(KeyCode.S, new CreatorShrinkBrush(creator, -0.1f));
            brushes.Add(KeyCode.C, new CreatorColorBrush(creator));
        }

        public override void OnInspectorGUI()
        {
            processor.DrawInspector();
        }

        private void OnSceneGUI()
        {
            input.Update();
            processor.DrawScene();

            if (creator.Geomery.Selected == null)
                return;

            foreach (var pair in brushes)
            {
                if (input.GetKeyDown(pair.Key))
                {
                    pair.Value.StartDrawScene();
                }

                if (input.GetKey(pair.Key))
                {
                    pair.Value.DrawScene();
                }

                if (input.GetKeyUp(pair.Key))
                {
                    creator.Geomery.Selected.Record();
                }
            }
        }
    }
}
