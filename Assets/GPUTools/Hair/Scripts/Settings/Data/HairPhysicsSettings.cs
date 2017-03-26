using System;
using System.Collections.Generic;
using System.Linq;
using GPUTools.Hair.Scripts.Settings.Data.Abstract;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Hair.Scripts.Settings.Data
{
    /// <summary>
    /// Physics simulation settings 
    /// </summary>
    [Serializable]
    public class HairPhysicsSettings : HairSettingsBase
    {
        public bool DebugDraw = false;

        //engine
        public ComputeShader Shader;

        //quality
        public int Iterations = 2;

        //stands
        public Vector3 Gravity = new Vector3(0,-1, 0);
        public float Drag = 0;
        public float StandRadius = 0.1f;

        //stands elasticy
        public AnimationCurve ElasticityCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        public float WindMultiplier = 0.0001f;

        //colliders
        public List<GameObject> ColliderProviders = new List<GameObject>();

        //Joints
        public List<HairJointArea> JointAreas = new List<HairJointArea>(); 

        public List<SphereCollider> Colliders
        {
            get { return colliders ?? (colliders = GetColliders()); }
        }

        #region compute data

        private List<SphereCollider> colliders;

        public List<SphereCollider> GetColliders()
        {
            var list = new List<SphereCollider>();

            foreach (var provider in ColliderProviders)
                list.AddRange(provider.GetComponents<SphereCollider>().ToList());

            return list;
        }

        #endregion

        public override void Validate()
        {
            Assert.IsNotNull(Shader, "Add compute shader to physics settings");
            foreach (var colliderProvider in ColliderProviders)
                Assert.IsNotNull(colliderProvider, "Setup Colliders Provider in Physics Settings it can't be null.");
        }
    }
}
