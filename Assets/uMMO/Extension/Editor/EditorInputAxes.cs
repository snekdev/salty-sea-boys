using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

namespace SoftRare.Editor {
    [CustomEditor(typeof(InputAxes))]
    public class EditorInputAxes : UnityEditor.Editor {
        static InputAxes m_InputAxes;
        static string path = "Assets/uMMO/Extension/Resources/InputAxes.asset";

        static void Create() {
            string[] InputAxes = AssetDatabase.FindAssets("t:InputAxes");

            if (InputAxes.Length > 0) {
                m_InputAxes = (InputAxes)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(InputAxes[0]), typeof(InputAxes));
                //Debug.Log (AssetDatabase.GUIDToAssetPath (InputAxes [0]));
            } else {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<InputAxes>(), path);
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = m_InputAxes;
            }
        }

        public override void OnInspectorGUI() {
            this.DrawDefaultInspector();
        }

    }
}