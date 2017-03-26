using GPUTools.Hair.Scripts.Settings.Data;
using GPUTools.Hair.Scripts.Settings.Data.Abstract;
using GPUTools.Hair.Scripts.Settings.GPBuilders;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings
{
    /// <summary>
    /// This class is access point to all hair settings. 
    /// Hair settings consist of one or multiple hair group settings
    /// On start it creates game object (to render hair) for each group settings
    /// </summary>
    public class HairSettings : MonoBehaviour
    {
        public HairStandsSettings StandsSettings;
        public HairPhysicsSettings PhysicsSettings;
        public HairRenderSettings RenderSettings;
        public HairLODSettings LODSettings;
        public HairShadowSettings ShadowSettings;
        
        public GPHairBuilder GPBuilder;
     
        private void Start()
        {
            ValidateImpl();
            GPBuilder = new GPHairBuilder(this);
        }

        public void Reset()
        {
            GPBuilder.Dispose();
            GPBuilder = new GPHairBuilder(this);
        }

        public void UpdateSettings()
        {
            if(Application.isPlaying)
                GPBuilder.UpdateSettings();
        }

        private void ValidateImpl()
        {
            StandsSettings.Validate();
            PhysicsSettings.Validate();
            RenderSettings.Validate();
            LODSettings.Validate();
            ShadowSettings.Validate();
        }

        private void OnDrawGizmos()
        {
            StandsSettings.DrawGizmos();
            PhysicsSettings.DrawGizmos();
            RenderSettings.DrawGizmos();
            LODSettings.DrawGizmos();
            ShadowSettings.DrawGizmos();
        }
        
    }
}
