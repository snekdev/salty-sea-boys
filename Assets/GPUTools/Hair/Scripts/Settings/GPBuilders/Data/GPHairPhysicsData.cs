using GPUTools.Physics.Scripts.Collisions;
using GPUTools.Physics.Scripts.Core;
using GPUTools.Physics.Scripts.Dynamics;
using GPUTools.Physics.Scripts.Joints;
using GPUTools.Physics.Scripts.Wind;
using GPUTools.Physics.Scripts.World;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.GPBuilders.Data
{
    public class GPHairPhysicsData : GPData
    {
        private readonly HairSettings settings;

        private readonly GPHairStandsBuilder stands;
        private readonly GPKinematicBuilder kinematic;
        private readonly WindReceiver wind;

        public GPHairPhysicsData(HairSettings settings)
        {
            this.settings = settings;

            stands = new GPHairStandsBuilder(settings);
            kinematic = new GPKinematicBuilder(settings);
            wind = new WindReceiver();
        }

        #region Config

        public override bool DebugDraw
        {
            get { return settings.PhysicsSettings.DebugDraw; }
        }

        public override ComputeShader Shader
        {
            get { return settings.PhysicsSettings.Shader; }
        }

        public override int Iterations
        {
            get { return settings.PhysicsSettings.Iterations; }
        }

        public override Vector3 Gravity
        {
            get { return settings.PhysicsSettings.Gravity*0.0005f; }
        }

        public override float Drag
        {
            get { return 1 - Mathf.Clamp01(settings.PhysicsSettings.Drag); }
        }

        public override Vector3 Wind
        {
            get { return wind.GetWind(settings.StandsSettings.HeadCenterWorld)*settings.PhysicsSettings.WindMultiplier*0.001f; }
        }

        #endregion

        #region Transforms

        public override Matrix4x4[] Matrices
        {
            get
            {
                return settings.StandsSettings.Provider.GetTransforms();
            }
        }

        #endregion

        #region Stands

        public override GPBody[] Bodies
        {
            get { return stands.Bodies; }
        }

        public override GPSphereCollider[] SphereColliders
        {
            get { return stands.SphereColliders; }
        }

        public override GroupedData<GPDistanceJoint> DistanceJoints
        {
            get { return stands.DistanceJoints; }
        }

        public override GPPointJoint[] PointJoints
        {
            get { return stands.PointJoints; }
        }

        #endregion

        #region Kinematic

        public override GPBody[] KinematicsBodies
        {
            get { return kinematic.Bodies; }
        }

        public override GPSphereCollider[] KinematicsSphereColliders
        {
            get { return kinematic.SphereColliders; }
        }

        #endregion
    }
}
