using UnityEngine;
using System.Collections;

public class GoToStore : MonoBehaviour {

	public void goToURL(string url) {
        if (url == "")
            url = "https://www.assetstore.unity3d.com/en/#!/content/13867";

        Application.OpenURL(url);
    }
}
