using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using System.Security.Cryptography;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SoftRare.Net;
using System;

namespace SoftRare.Utils {
    public partial class Library {

        public static double NetworkTime
        {
            get { return Network.time; }
        }
		
        //http://codeclimber.net.nz/archive/2007/07/10/convert-a-unix-timestamp-to-a-.net-datetime.aspx
        public static double conv_Date2Timestamp(DateTime date) {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static System.DateTime dt_now() {
            return DateTime.Now;
        }

        public static double ts_now() {
            return conv_Date2Timestamp(dt_now());
        }		

        //source: http://stackoverflow.com/questions/3984138/hash-string-in-c-sharp
        public static byte[] GetHash(string inputString) {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA1.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        public static string GetHashString(string inputString) {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static T initPlugin<T>(T plugin2instantiate, GameObject pluginOwner) where T : MonoBehaviour, Net.Plugin.IPluggable {

            if (plugin2instantiate != null) {
                T[] plugins = (T[])GameObject.FindObjectsOfType(typeof(T));
                //T[] plugins = pluginOwner.GetComponentsInChildren<T>();

                bool alreadyInstantiated = false;

                foreach (T plugin in plugins) {

                    //    if (plugin.GetType() == plugin2instantiate.GetType()) {
                    if (plugin == plugin2instantiate) {
                        alreadyInstantiated = true;
                        plugin2instantiate = plugin;
                        break;
                    }

                }

                if (!alreadyInstantiated) { //exists only as prefab in the project hierarchy, not as instance in scene yet
                    GameObject go = GameObject.Instantiate(plugin2instantiate.gameObject);
                    plugin2instantiate = go.GetComponent<T>();
                }

                if (plugin2instantiate.transform.parent != pluginOwner.transform) {

                    plugin2instantiate.transform.parent = pluginOwner.transform;

                    plugin2instantiate.transform.localPosition = Vector3.zero;
                    plugin2instantiate.transform.localRotation = Quaternion.identity;
                }
            }
            plugin2instantiate.__awake(pluginOwner);
            return plugin2instantiate;

        }

        public static void killScriptsRequiredInclusive(List<Component> componentsToKill) {
            //uses multiple cycles to kill components which are required by others

            if (uMMO.get.showDebugHints) {
                Debug.Log("uMMO hint: if you receive one or multiple errors (\"Can't remove xxx (Script) because yyy (Script) depends on it\") here, its normal (if not, its normal, too ;)");
            }
            int cycles = 0;
            do {
                List<Component> componentsToKillNew = componentsToKill;
                for (int i = 0; i < componentsToKill.Count; i++) {
                    Component comp = componentsToKill[i];

                    try {

                        GameObject.DestroyImmediate(comp);
                    } finally {
                        if (comp == null) {
                            componentsToKillNew.RemoveAt(i);
                        } else { //change order
                            componentsToKillNew.Remove(comp);
                            componentsToKillNew.Add(comp);
                        }
                    }
                }

                componentsToKill = componentsToKillNew;
                cycles++;
            } while (componentsToKill.Count > 0 && cycles <= 10);

        }

        public static string ObjectDebugName2ScriptName(string objectDebugName) {
            Match match = Regex.Match(objectDebugName, @".*\((.*\.)?([a-zA-Z0-9\_]+)\)$", RegexOptions.IgnoreCase);
            string key = "";

            if (match.Success) {
                // Finally, we get the Group value and display it.
                key = match.Groups[2].Value;
            }
            return key;
        }

        public static string ScriptAndNamespace2ScriptName(string objectDebugName) {
            Match match = Regex.Match(objectDebugName, @"(.*\.)?([a-zA-Z0-9\_]+)$", RegexOptions.IgnoreCase);
            string key = "";

            if (match.Success) {
                // Finally, we get the Group value and display it.
                key = match.Groups[2].Value;
            }
            return key;
        }

        public static Component findComponentInChildren(string compName, GameObject parent) {
            Component result = null;
            Component[] components = parent.GetComponentsInChildren<Component>();


            foreach (Component comp in components) {
                if (comp.gameObject.name == compName) {
                    result = comp;
                    break;
                }

                //Debug.Log("ScriptAndNamespace2ScriptName(comp.GetType().ToString()): "+ScriptAndNamespace2ScriptName(comp.GetType().ToString()));
                if (ScriptAndNamespace2ScriptName(comp.GetType().ToString()) == compName) {
                    result = comp;
                    break;
                }
            }

            return result;
        }

        public static void addDefineIfNotExists(string newDefine) {
#if UNITY_EDITOR
            string currDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            string[] split = currDefines.Split(";".ToCharArray());

            bool found = false;
            string newDefinesString = "";
            foreach (string s in split) {

                newDefinesString += s + ";";

                if (newDefine == s) {
                    found = true;
                    break;
                }
            }

            if (!found) {
                newDefinesString += newDefine;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefinesString);
            }
#endif
            //EditorUserBuildSettings.activeScriptCompilationDefines
        }

        public static void removeDefineIfExists(string Define2Remove) { //if exists
#if UNITY_EDITOR
            string currDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            string[] split = currDefines.Split(";".ToCharArray());

            bool found = false;
            string newDefinesString = "";
            foreach (string s in split) {
                if (Define2Remove == s) {
                    found = true;
                } else {
                    newDefinesString += s + ";";
                }
            }

            if (found) {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefinesString);
            }
#endif
        }
    }
}
