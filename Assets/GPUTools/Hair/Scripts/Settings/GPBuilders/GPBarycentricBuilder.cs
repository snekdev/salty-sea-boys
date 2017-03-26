using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.GPBuilders
{
    public class GPBarycentricBuilder
    {
        private readonly Vector3[] weights;

        public GPBarycentricBuilder()
        {
            var weightsList = new List<Vector3>();

            while (weightsList.Count <= 64)
            {
                var k = GetRandomK();
                if (!weightsList.Contains(k))
                    weightsList.Add(GetRandomK());
            }

            weights = weightsList.ToArray();
        }

        private Vector3 GetRandomK()
        {
            var ka = Random.Range(0f, 1f);
            var kb = Random.Range(0f, 1f);

            if (ka + kb > 1)
            {
                ka = 1 - ka;
                kb = 1 - kb;
            }

            var kc = 1 - (ka + kb);
            return new Vector3(ka, kb, kc);
        }

        public Vector3[] Weights
        {
            get { return weights; }
        }
    }
}
