using System;
using System.Collections.Generic;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
    [Serializable]
    public class CreatorGeometry//todo make it generic tool
    {
        public List<GeometryGroupData> List = new List<GeometryGroupData>(); 
        public int SelectedIndex;

        public GeometryGroupData Selected
        {
            get { return SelectedIndex >= 0 && SelectedIndex < List.Count
                    ? List[SelectedIndex] 
                    : null;
            }
        }
    }
}
