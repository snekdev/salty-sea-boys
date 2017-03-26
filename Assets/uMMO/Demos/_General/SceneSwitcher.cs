using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GUISwitcher : MonoBehaviour {


    protected void switchScene_to(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

	public void switchTo_LegacyLocalAuth() {
        switchScene_to("uMMO/Demos/LegacyAnim_LocalAuthority");
    }

    public void switchTo_LegacyServerAuth() {
        switchScene_to("uMMO/Demos/LegacyAnim_ServerAuthority");
    }

    public void switchTo_MecanimLocalAuth() {
        switchScene_to("uMMO/Demos/PointAndClickMecanim_LocalAuthority");
    }

    public void switchTo_MecanimServerAuth() {
        switchScene_to("uMMO/Demos/PointAndClickMecanim_ServerAuthority");
    }

    public void switchTo_SpaceShooterLocalAuth() {
        switchScene_to("uMMO/Demos/SpaceShooterCoop_LocalAuthority");
    }

    public void switchTo_SpaceShooterServerAuth() {
        switchScene_to("uMMO/Demos/SpaceShooterCoop_ServerAuthority");
    }
}
