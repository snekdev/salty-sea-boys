using UnityEngine;
using System.Collections;

using UnityEngine.Networking;

namespace SoftRare.Net {
    [ExecuteInEditMode]
    public abstract class PlayerAction : Action {

        public DontPanic.USE_uMMOInput_INSTEAD_OF_Input_IN_YOUR_CODE Input {
            get { return new DontPanic.USE_uMMOInput_INSTEAD_OF_Input_IN_YOUR_CODE(); }
        }

        public NetObject uMMOInput {
            get { return GetComponentInParent<SoftRare.Net.NetObject>(); }
        }

    }
}