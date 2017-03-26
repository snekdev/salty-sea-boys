using UnityEngine;
using System.Collections;

namespace SoftRare.Net.Plugin {
    public abstract class PL_NetworkManager : UnityEngine.Networking.NetworkManager, IPluggable {
        GameObject pluginOwner;
        public bool initialized = false;
        public bool started = false;

        public virtual void __awake(GameObject pluginOwner) {
            this.pluginOwner = pluginOwner;
            initialized = true;
        }

        public virtual void __start() {
            started = true;
        }
    }
}
