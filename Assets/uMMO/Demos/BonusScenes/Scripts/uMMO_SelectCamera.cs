using UnityEngine;
using System.Collections;

public class uMMO_SelectCamera : MonoBehaviour {

    void __uMMO_localPlayer_init() {
        Camera.main.gameObject.GetComponent<SoftRare.Div.ThirdPersonCamera>().follow = this.transform;
    }
}
