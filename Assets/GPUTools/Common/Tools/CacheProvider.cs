using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPUTools.Common.Tools
{
    public class CacheProvider<T> where T : Component
    {
        private readonly List<GameObject> providers;

        public CacheProvider(List<GameObject> providers)
        {
            this.providers = providers;
        }

        private List<T> items;

        public List<T> GetItems()
        {
            var list = new List<T>();

            foreach (var provider in providers)
                list.AddRange(provider.GetComponents<T>().ToList());

            return list;
        }

        public List<T> Items
        {
            get { return items ?? (items = GetItems()); }
        }
    }
}
