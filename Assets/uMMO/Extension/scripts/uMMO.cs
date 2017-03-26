using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;

namespace SoftRare.Net {
    public
#if IS_UNLICENSED
        sealed
#endif
        class uMMO : Singleton<uMMO> {

        [Tooltip("uMMO uses the UNET HLAPI NetworkManager to configure and run the network. To make it more integrated we defined the NetworkManager as a native uMMO plugin. It should be set here and be added as a child of the uMMO prefab instantiated in the scene. Set the network manager plugin here you want to use. In most cases you should use the one which is given by default with the package which can be found in 'Assets/uMMO/Extension/Prefabs/Plugins/uMMO'.")]
        public Plugin.PL_NetworkManager networkManager;
        [Tooltip("This plugin can be used to decide whether you want to run a server or client. To make that choice this plugin can also contain a GUI to e.g. fill in IP and port of the server. To activate this plugin choose \"Use Custom Connector\" on the \"Architecture To Compile\" setting on this game object.")]
        public Plugin.PL_Connector connector;
        [Tooltip("This setting should be filled with the standard resource it comes with. It is an .asset file which is used to store the input axes configured in the Input Settings of the Unity Editor. If this is empty or not set to the correct resource you will be unable to choose input fields to automatically send to the server in server authority cases.")]
        public InputAxes inputAxes;
        [Tooltip("Decide here what kind of architecture you want to compile/start with the scene. If you choose Server or Client the appropriate architecture will be compiled immediately after starting the scene. If you compile a server in example for upload to a linux machine you should choose Server, otherwise nothing will happen once the server instance is started, the port will not be opened. Choose Client here to make the instance immediately and automatically behave like a client. Make sure in that case that you have specified the standard host IP and port of your dedicated server in the UNET network manager plugin.")]
        public CompilationArchitecture architectureToCompile;

        [Tooltip("Drag and drop a plugin here which should be used as 'default plugin' when 'SyncUsing_DEFAULT_uMMO_PLUGIN' has been chosen on NetObject components.")]
        public Plugin.PL_AuthoritySerializer defaultTransformSyncingPlugin;
        [Tooltip("Drag and drop a plugin here which should be used as 'default plugin' when 'SyncUsing_DEFAULT_uMMO_PLUGIN' has been chosen on NetObject components.")]
        public Plugin.PL_AuthoritySerializer defaultLegacyAnimSyncingPlugin;
        [Tooltip("Drag and drop a plugin here which should be used as 'default plugin' when 'SyncUsing_DEFAULT_uMMO_PLUGIN' has been chosen on NetObject components.")]
        public Plugin.PL_AuthoritySerializer defaultMecanimSyncingPlugin;

        public Dictionary<NetworkConnection, NetObject> connections2NetObjects = new Dictionary<NetworkConnection, NetObject>();
        [Tooltip("Activate this to enable a new feature in uMMO: server and client code separation! This means that you can set via preprocessor defines which parts of your code should e.g. be included in the Assembly only on the server. This way you can hide from the client important parts of your business logic while still having the luxury of designing server and client in the very same scene. E.g. if you activate 'Server' on the property 'Architecture To Compile' you will have the following preprocessor define enabled: SR_uMMO_SERVER . With this conside the following example code:\n\n#if SR_uMMO_SERVER\nstring secretKey=\"theClientWillNeverKnow\";\n#end if\n\n The line in the middle will be hidden from the client, it will entirely not be included in any assembly which you make available to your customers.\n\n(ATTENTION: Make sure you understand that hard coded setting of any keys/passwords is not safe and flexible. The above example only represents exactly that: an example)")]
        public bool dynPreprocessorDefines;

        public bool showDebugHints;

        [SerializeField]
        private List<string> currentDynamicDefs = new List<string>(); 

        //the single instance of this class
        private static uMMO instance;

        protected void Awake() {
            //GlobalConfig config = new GlobalConfig();
            //config.MaxPacketSize = 20000;

            //plugin initialization
            networkManager = Utils.Library.initPlugin(networkManager, gameObject);
            connector = Utils.Library.initPlugin(connector, gameObject);

            if (inputAxes == null) {
                inputAxes = (InputAxes)Resources.Load("InputAxes");
            }

            if (architectureToCompile == CompilationArchitecture.UseCustomConnector) {
                uMMO.get.connector.showGUI = true;
                
            } else {
                uMMO.get.connector.connectAutomatically = true;
            }

            if (uMMO.get.connector.connectAutomatically) {
                StartCoroutine(uMMO.get.connector.connectDelayed());
            }
        }

        public bool isServer {
            get { return NetworkServer.active; }
        }

        public bool isClient { 
            get { return NetworkClient.active; }
        }

        protected void OnValidate() {

            if (!Application.isPlaying) {

                if (networkManager != null) {
                    networkManager.dontDestroyOnLoad = dontDestroyOnLoad;
                    if (uMMO.get != null) {
                        networkManager.scriptCRCCheck = !uMMO.get.dynPreprocessorDefines;
                    }
                }
                
                if (dynPreprocessorDefines) {

                    if (architectureToCompile == CompilationArchitecture.Server) {
                        if (!currentDynamicDefs.Contains("SR_uMMO_SERVER")) {
                            Utils.Library.addDefineIfNotExists("SR_uMMO_SERVER");
                            currentDynamicDefs.Add("SR_uMMO_SERVER");
                        }
                    } else {
                        if (currentDynamicDefs.Contains("SR_uMMO_SERVER")) {
                            Utils.Library.removeDefineIfExists("SR_uMMO_SERVER");
                            currentDynamicDefs.Remove("SR_uMMO_SERVER");
                        }
                    }

                    if (architectureToCompile == CompilationArchitecture.UseCustomConnector) {
                        if (!currentDynamicDefs.Contains("SR_uMMO_TEST")) {
                            Utils.Library.addDefineIfNotExists("SR_uMMO_TEST");
                            currentDynamicDefs.Add("SR_uMMO_TEST");
                        }
                    } else {
                        if (currentDynamicDefs.Contains("SR_uMMO_SERVER")) {
                            Utils.Library.removeDefineIfExists("SR_uMMO_TEST");
                            currentDynamicDefs.Remove("SR_uMMO_TEST");
                        }
                    }
                } else {
                    
                    if (currentDynamicDefs.Contains("SR_uMMO_SERVER") || currentDynamicDefs.Contains("SR_uMMO_TEST")) {
                        Utils.Library.removeDefineIfExists("SR_uMMO_SERVER");
                        Utils.Library.removeDefineIfExists("SR_uMMO_TEST");

                        currentDynamicDefs.Remove("SR_uMMO_SERVER");
                        currentDynamicDefs.Remove("SR_uMMO_TEST");
                    }
                }

            }
        }

        protected void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        // public getter to be able to call the singleton instance from the outside
        public static uMMO get {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<uMMO>();
                }
                return instance;
            }
        }


    }
}
