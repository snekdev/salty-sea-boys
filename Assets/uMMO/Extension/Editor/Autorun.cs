using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

//based on http://answers.unity3d.com/questions/45186/can-i-auto-run-a-script-when-editor-launches-or-a.html
namespace SoftRare.Editor {
    [InitializeOnLoad]
    public class Autorun {
        static Autorun() {
            //Debug.Log("Autorun!");
            Utils.Library.addDefineIfNotExists("SR_uMMO");
            InputManagerAnalyser.ReadAxes();
            EditorApplication.update += Update;
        }
        //based on http://webcache.googleusercontent.com/search?q=cache:blr1FPrDjuAJ:totalmonkery.com/tips-and-tricks-a-handy-unity-trick-for-detecting-when-scene-changes-in-the-editor/&num=1&client=firefox-b&hl=nl&gl=nl&strip=1&vwsrc=0
        static string sSceneName = null;
        static void Update() {
            if (sSceneName != EditorSceneManager.GetActiveScene().name) {
                // New scene has been loaded
                sSceneName = EditorSceneManager.GetActiveScene().name;
                //Debug.Log("new scene load!");
                //EditorSceneManager.
                GameObject textObj = GameObject.Find("uMMO_In_Scene_Text");
                if (textObj != null) {
                    GUIText guiText = textObj.GetComponent<GUIText>();
                    if (guiText != null) {
                        TextAsset txt = (TextAsset)Resources.Load("uMMO_In_Scene_Text", typeof(TextAsset));
                        guiText.text = txt.text;
                    }
                }

            }
        }

    }
}