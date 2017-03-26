using System.Collections.Generic;
using GPUTools.Physics.Scripts.Collisions;
using GPUTools.Physics.Scripts.Dynamics;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.GPBuilders
{
    public class GPKinematicBuilder
    {
       private readonly HairSettings settings;

        private GPBody[] bodies;
        private GPSphereCollider[] sphereColliders;

        public GPKinematicBuilder(HairSettings settings)
        {
            this.settings = settings;
            UpdateBodies(settings.PhysicsSettings.Colliders);
        }

        private void UpdateBodies(List<SphereCollider> colliders)
        {
            if (bodies == null)
                bodies = new GPBody[colliders.Count];

            for (var i = 0; i < colliders.Count; i++)
            {
                var sphereCollider = colliders[i];
                var transformedPoint = sphereCollider.transform.TransformPoint(sphereCollider.center);

                bodies[i] = new GPBody(transformedPoint);
            }
        }

        private void UpdateSphereColliders(List<SphereCollider> colliders)
        {
            if (sphereColliders == null)
                sphereColliders = new GPSphereCollider[colliders.Count];

            for (var i = 0; i < colliders.Count; i++)
            {
                var sphereCollider = colliders[i];
                var transformedRadius = sphereCollider.transform.lossyScale.x*sphereCollider.radius;

                sphereColliders[i] = new GPSphereCollider(i, transformedRadius);
            }
        }

        public GPBody[] Bodies
        {
            get
            {
                UpdateBodies(settings.PhysicsSettings.Colliders);
                return bodies;
            }
        }

        public GPSphereCollider[] SphereColliders
        {
            get
            {
                UpdateSphereColliders(settings.PhysicsSettings.Colliders);
                return sphereColliders;
            }
        }
    }
}
