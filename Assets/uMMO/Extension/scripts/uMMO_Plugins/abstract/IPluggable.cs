using UnityEngine;
using System.Collections;

namespace SoftRare.Net.Plugin {
    public interface IPluggable {
        void __awake(GameObject pluginOwner);
        void __start();
    }
}
