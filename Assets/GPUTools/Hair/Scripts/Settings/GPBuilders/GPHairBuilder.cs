using GPUTools.Hair.Scripts.Physics;
using GPUTools.Hair.Scripts.Render;
using GPUTools.Hair.Scripts.Settings.GPBuilders.Data;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.GPBuilders
{
    public class GPHairBuilder
    {
        private GameObject obj;
        private GPHairPhysics physics;
        private HairRender render;

        public GPHairBuilder(HairSettings settings)
        {
            var hairObj = BuildHair(settings);
            hairObj.transform.SetParent(settings.transform.parent, false);
        }

        public void UpdateSettings()
        {
            physics.UpdateSettings();
            render.UpdateSettings();
        }

        private GameObject BuildHair(HairSettings settings)
        {
            obj = new GameObject("Render");
            obj.layer = settings.gameObject.layer;
            
            var physicsData = new GPHairPhysicsData(settings);

            physics = obj.AddComponent<GPHairPhysics>();
            physics.Initialize(physicsData);

            var renderDara = new GPHairRenderData(settings, physics.GetBodiesBuffer());

            render = obj.AddComponent<HairRender>();
            render.Initialize(renderDara);

            return obj;
        }

        public void Dispose()
        {
            Object.Destroy(obj);
        }
    }
}
