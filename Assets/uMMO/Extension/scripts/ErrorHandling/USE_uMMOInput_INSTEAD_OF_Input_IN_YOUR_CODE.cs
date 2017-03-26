using System;

namespace SoftRare.DontPanic {
    public class USE_uMMOInput_INSTEAD_OF_Input_IN_YOUR_CODE : Exception {

        // Please read the following to resolve the compiler message

        const string message = "If you inherit from PlayerAction instead of MonoBehaviour or NetworkBehaviour: In your code please change 'Input.' to either 'UnityEngine.Input.' or 'uMMO_Input.'. The former uses normal Unity3d input processing. The latter uses advanced uMMO input processing. In case of local player authority nothing changes. In case of server authority the input will be sent to the server automatically, without you having to do anything more. You can also e.g. use 'uMMO_Input.GetButtonSENDTOALL' instead of 'Input.GetButton'. This will then send the input to the server and from the server to all clients.";
        

        public USE_uMMOInput_INSTEAD_OF_Input_IN_YOUR_CODE() :
            base(message) { 

        }

    }
}
