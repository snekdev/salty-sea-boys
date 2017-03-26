using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

using UnityEngine.Networking;
using SoftRare.Net;

namespace SoftRare.Editor {

    [CustomEditor(typeof(NetObject))]
    public class NetObjectEditor : UnityEditor.Editor {

        private SerializedProperty objectType;

        private SerializedProperty inputCollectionMethod;
        private SerializedProperty inputSendingMethod;
        private SerializedProperty inputReadingMethod;
        private SerializedProperty clearInputStates;

        private SerializedProperty inputFromAxesToSendToServer;
        private SerializedProperty inputFromKeysToSendToServer;
        private SerializedProperty inputFromMouseButtonsToSendToServer;

        private SerializedProperty callbackOptions;
        private SerializedProperty messengerPlugins;
        private SerializedProperty mecanimVarsToSync;

        private SerializedProperty transformSyncingMethod;
        private SerializedProperty animationSyncingMethod;
        private SerializedProperty pluginShouldSyncPosition;
        private SerializedProperty pluginShouldSyncRotation;
        private SerializedProperty pluginShouldSyncAnimations;

        private SerializedProperty handelScriptsBasedOnSpawnCase;
        private SerializedProperty clientLocalPlayerScripts;
        private SerializedProperty clientRemotePlayerScripts;
        private SerializedProperty serverPlayerScripts;
        private SerializedProperty serverNPOScripts;
        private SerializedProperty clientNPOScripts;

        private SerializedProperty handelScriptsBasedOnSpawnCaseAtRuntime;
        private SerializedProperty clientLocalPlayerRuntimeScripts;
        private SerializedProperty clientRemotePlayerRuntimeScripts;
        private SerializedProperty serverPlayerRuntimeScripts;
        private SerializedProperty serverNPORuntimeScripts;
        private SerializedProperty clientNPORuntimeScripts;

        NetObject myTarget;

        private string[] dropdown_options;
        private string[] checkbox_options;

        private void OnEnable() {
            objectType = this.serializedObject.FindProperty("objectType");

            inputCollectionMethod = this.serializedObject.FindProperty("inputCollectionMethod");
            inputSendingMethod = this.serializedObject.FindProperty("inputSendingMethod");
            inputReadingMethod = this.serializedObject.FindProperty("inputReadingMethod");
            clearInputStates = this.serializedObject.FindProperty("clearInputStates");

            handelScriptsBasedOnSpawnCase = this.serializedObject.FindProperty("handelScriptsBasedOnSpawnCase");
            clientLocalPlayerScripts = this.serializedObject.FindProperty("clientLocalPlayerScripts");
            clientRemotePlayerScripts = this.serializedObject.FindProperty("clientRemotePlayerScripts");
            serverPlayerScripts = this.serializedObject.FindProperty("serverPlayerScripts");
            serverNPOScripts = this.serializedObject.FindProperty("serverNPOScripts");
            clientNPOScripts = this.serializedObject.FindProperty("clientNPOScripts");

            handelScriptsBasedOnSpawnCaseAtRuntime = this.serializedObject.FindProperty("handelScriptsBasedOnSpawnCaseAtRuntime");
            clientLocalPlayerRuntimeScripts = this.serializedObject.FindProperty("clientLocalPlayerRuntimeScripts");
            clientRemotePlayerRuntimeScripts = this.serializedObject.FindProperty("clientRemotePlayerRuntimeScripts");
            serverPlayerRuntimeScripts = this.serializedObject.FindProperty("serverPlayerRuntimeScripts");
            serverNPORuntimeScripts = this.serializedObject.FindProperty("serverNPORuntimeScripts");
            clientNPORuntimeScripts = this.serializedObject.FindProperty("clientNPORuntimeScripts");

            dropdown_options = InputAxes.mergedValues();
            inputFromAxesToSendToServer = this.serializedObject.FindProperty("inputFromAxesToSendToServer");
            inputFromKeysToSendToServer = this.serializedObject.FindProperty("inputFromKeysToSendToServer");
            inputFromMouseButtonsToSendToServer = this.serializedObject.FindProperty("inputFromMouseButtonsToSendToServer");

            checkbox_options = Net.NetObject.callbackValues();
            callbackOptions = this.serializedObject.FindProperty("activeCallbacks");
            mecanimVarsToSync = this.serializedObject.FindProperty("mecanimVarsToSync");
            messengerPlugins = this.serializedObject.FindProperty("messengerPlugins");
            transformSyncingMethod = this.serializedObject.FindProperty("transformSyncingMethod");
            animationSyncingMethod = this.serializedObject.FindProperty("animationSyncingMethod");
            pluginShouldSyncPosition = this.serializedObject.FindProperty("pluginShouldSyncPosition");
            pluginShouldSyncRotation = this.serializedObject.FindProperty("pluginShouldSyncRotation");
            pluginShouldSyncAnimations = this.serializedObject.FindProperty("pluginShouldSyncAnimations");

        }

        protected void generateTitle(string title, string helptext) {
            generateTitle(title, helptext, EditorStyles.objectFieldThumb);
        }

        protected void generateTitle(string title, string helptext, GUIStyle style) {
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField(title, style);

            EditorGUILayout.HelpBox(helptext, MessageType.Info);
            
        }

        public override void OnInspectorGUI() {

            myTarget = (NetObject)target;

            serializedObject.Update();
            generateTitle("uMMO SoftRare.Net.NetObject", "Configure specifics about this networked game object here (being refered to as 'this NetObject'). Move the mouse over a property of this script to read information about its uses and limitations.", EditorStyles.toolbar);

            EditorGUILayout.PropertyField(objectType);
            generateTitle("Synchronization Methods", "Define here how you want transforms and animations synchronized");

            EditorGUILayout.PropertyField(transformSyncingMethod);
            if (myTarget.transformSyncingMethod == SyncingMethod.SyncUsing_DEFAULT_NATIVE_COMPONENT) { 
                if (myTarget.GetComponent<NetworkTransform>() == null) {
                    myTarget.gameObject.AddComponent<NetworkTransform>();
                } else {
                    myTarget.gameObject.GetComponent<NetworkTransform>().enabled = true;
                }
            } else if (myTarget.transformSyncingMethod != SyncingMethod.SyncUsing_DEFAULT_NATIVE_COMPONENT) {
                if (myTarget.GetComponent<NetworkTransform>() != null) {
                    myTarget.gameObject.GetComponent<NetworkTransform>().enabled = false;
                }
            }
            if (myTarget.transformSyncingMethod == SyncingMethod.SyncUsing_DEFAULT_uMMO_PLUGIN) {

                EditorGUILayout.PropertyField(pluginShouldSyncPosition);
                EditorGUILayout.PropertyField(pluginShouldSyncRotation);

            }

            EditorGUILayout.PropertyField(animationSyncingMethod);
            bool usesMecanim = false;
            Animator animator = myTarget.gameObject.GetComponentInChildren<Animator>();
            if (animator != null)
                usesMecanim = true;

            if (myTarget.animationSyncingMethod == SyncingMethod.SyncUsing_DEFAULT_NATIVE_COMPONENT) {

                if (usesMecanim) {
                    if (myTarget.gameObject.GetComponent<NetworkAnimator>() == null) {
                        myTarget.gameObject.AddComponent<NetworkAnimator>();

                        myTarget.gameObject.GetComponent<NetworkAnimator>().enabled = true;
                    }

                    //myTarget.gameObject.GetComponent<NetworkAnimator>().animator = animator; //throws error
                }

            } else if (myTarget.animationSyncingMethod != SyncingMethod.SyncUsing_DEFAULT_NATIVE_COMPONENT) {
                if (myTarget.GetComponent<NetworkAnimator>() != null) {
                    myTarget.gameObject.GetComponent<NetworkAnimator>().enabled = false;
                }
            }
            EditorGUI.indentLevel += 1;
            if (myTarget.animationSyncingMethod == SyncingMethod.SyncUsing_DEFAULT_uMMO_PLUGIN) {
                EditorGUILayout.PropertyField(pluginShouldSyncAnimations);
            }
            if (myTarget.animationSyncingMethod == SyncingMethod.SyncUsing_DEFAULT_uMMO_PLUGIN) {
                if (usesMecanim && myTarget.pluginShouldSyncAnimations) {

                    var controller = animator.runtimeAnimatorController as AnimatorController;
                    if (controller != null) {
                        EditorGUI.indentLevel += 1;
                        for(int i=0;i<controller.parameters.Length;i++) { // (var p in controller.parameters) {
                            AnimatorControllerParameter acp = controller.parameters[i];
                            
                            bool oldExists = GetArrayElementAtIndex(mecanimVarsToSync, i) >= 0 ? true: false;
                            bool newExists = EditorGUILayout.Toggle(acp.name, oldExists);
                            if (newExists != oldExists) {
                                if (newExists) {
                                    addToStringArr(mecanimVarsToSync, i);
                                } else {
                                    removeFromStringArr(mecanimVarsToSync, i);
                                }
                                EditorUtility.SetDirty(target);
                            }
                        }
                        EditorGUI.indentLevel -= 1;
                    }
                }
            }

            EditorGUI.indentLevel -= 1;
            generateTitle("Spawn Case Settings","Drag and drop scripts from this gameobject to handle different spawn cases per script");
            EditorGUILayout.PropertyField(handelScriptsBasedOnSpawnCase,true);
            EditorGUI.indentLevel += 1;
            if (myTarget.handelScriptsBasedOnSpawnCase) {
                if (myTarget.objectType == ObjectType.Player) {
                    EditorGUILayout.PropertyField(clientLocalPlayerScripts, true);
                    EditorGUILayout.PropertyField(clientRemotePlayerScripts, true);
                    EditorGUILayout.PropertyField(serverPlayerScripts, true);
                } else if (myTarget.objectType == ObjectType.NonPlayerObject) {
                    EditorGUILayout.PropertyField(serverNPOScripts, true);
                    EditorGUILayout.PropertyField(clientNPOScripts, true);
                }
            }
            EditorGUI.indentLevel -= 1;

            EditorGUILayout.PropertyField(handelScriptsBasedOnSpawnCaseAtRuntime,true);
            EditorGUI.indentLevel += 1;
            if (myTarget.handelScriptsBasedOnSpawnCaseAtRuntime) {
                if (myTarget.objectType == ObjectType.Player) {
                    EditorGUILayout.PropertyField(clientLocalPlayerRuntimeScripts, true);
                    EditorGUILayout.PropertyField(clientRemotePlayerRuntimeScripts, true);
                    EditorGUILayout.PropertyField(serverPlayerRuntimeScripts, true);
                } else if (myTarget.objectType == ObjectType.NonPlayerObject) {
                    EditorGUILayout.PropertyField(serverNPORuntimeScripts, true);
                    EditorGUILayout.PropertyField(clientNPORuntimeScripts, true);
                }
            }
            EditorGUI.indentLevel -= 1;

            generateTitle("Input Settings","Define here how you want input synchonized and processed. Especially important for cases of server-authority");
            if (myTarget.GetComponent<NetworkIdentity>() != null && !myTarget.GetComponent<NetworkIdentity>().localPlayerAuthority && myTarget.objectType == ObjectType.Player) {                
                    //settings for authoritative server:
                EditorGUILayout.PropertyField(inputCollectionMethod, true);

                if (myTarget.inputCollectionMethod == InputCollectionMethod.CollectEveryFrame) {
                    EditorGUI.indentLevel += 1;
                    strings2enumView(inputFromAxesToSendToServer, "Input Axes");
                    EditorGUILayout.PropertyField(inputFromKeysToSendToServer, true);
                    EditorGUILayout.PropertyField(inputFromMouseButtonsToSendToServer, true);
                    EditorGUI.indentLevel -= 1;
                }

                EditorGUILayout.PropertyField(inputSendingMethod, true);
            }
            EditorGUILayout.PropertyField(inputReadingMethod, true);
            EditorGUILayout.PropertyField(clearInputStates, true);

            generateTitle("Plugins","Drag and drop \"Messenger\" plugins here for them to be added at runtime.");
            EditorGUILayout.PropertyField(messengerPlugins, true);

            generateTitle("Callbacks", "Implement these as functions in any script on this NetObject.\nIn example:\n\nvoid __uMMO_localPlayer_init() {\n    print(\"local player initialized\")\n}");
            strings2checkboxView(callbackOptions, "Callbacks");

            serializedObject.ApplyModifiedProperties();

        }

        protected int GetArrayElementAtIndex(SerializedProperty arr, int key) {
            int ret = -1;
            for(int i=0;i<arr.arraySize;i++) {
                if (arr.GetArrayElementAtIndex(i).intValue == key) {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        protected void addToStringArr(SerializedProperty arr, int key) {
            if (GetArrayElementAtIndex(arr, key) < 0) {
                arr.InsertArrayElementAtIndex(arr.arraySize);
                arr.GetArrayElementAtIndex(arr.arraySize-1).intValue = key;
            }
        }

        protected void removeFromStringArr(SerializedProperty arr, int key) {
            int index = GetArrayElementAtIndex(arr, key);
            arr.DeleteArrayElementAtIndex(index);
        }

        protected void strings2checkboxView(SerializedProperty options, string description) {
            //EditorGUILayout.LabelField(options.displayName); //TODO: make bold
            //options.arraySize = EditorGUILayout.IntField("Amount " + description, NetObject.callbackValues().Length);
            EditorGUI.indentLevel += 1;
            options.arraySize = NetObject.callbackValues().Length;

            if (options.arraySize > NetObject.callbackValues().Length) {
                options.arraySize = NetObject.callbackValues().Length;
            }

            for (int i = 0; i < options.arraySize; i++) {
                options.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.Toggle(checkbox_options[i], options.GetArrayElementAtIndex(i).boolValue);
            }
            EditorGUI.indentLevel -= 1;
        }

        protected void strings2enumView(SerializedProperty options, string description) {
            EditorGUILayout.LabelField(options.displayName); //TODO: make bold
            EditorGUI.indentLevel += 1;
            options.arraySize = EditorGUILayout.IntField("Amount " + description, options.arraySize);
            for (int i = 0; i < options.arraySize; i++) {
                options.GetArrayElementAtIndex(i).intValue = EditorGUILayout.Popup(options.GetArrayElementAtIndex(i).intValue, dropdown_options);
            }
            EditorGUI.indentLevel -= 1;
        }

    }
}