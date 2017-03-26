using System.Collections.Generic;

namespace Assets.GPUTools.Common.Editor.Engine
{
    public class Processor : EditorItemBase
    {
        private List<EditorItemBase> items = new List<EditorItemBase>(); 

        public void Add(EditorItemBase item)
        {
            items.Add(item);
        }

        public override void DrawInspector()
        {
            items.ForEach(i => i.DrawInspector());
        }

        public override void DrawScene()
        {
            items.ForEach(i => i.DrawScene());
        }
    }
}
