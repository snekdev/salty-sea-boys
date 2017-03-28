using System.Collections.Generic;
using UnityEngine;

namespace SnekShaders {

	[ExecuteInEditMode]
	[AddComponentMenu("Hidden/Snekdev/ImageEffects/LineArt")]
	public class PP_Generic: MonoBehaviour{
        [System.Serializable]
        public class ColorPair
        {
            public Color data;
            public string name;
        }

        [System.Serializable]
        public class FloatPair
        {
            public float data;
            public string name;
        }
        
        public List<FloatPair> floatPairs;
        public List<ColorPair> colorPairs;

        public Shader shader;
        private Material _material;
        
        protected Material material
        {
            get
            {
                if (_material == null)
                {
                    _material = new Material(shader);
                    _material.hideFlags = HideFlags.HideAndDontSave;
                }
                return _material;
            }
        }
        
        //Mono Methods//
        protected void OnEnable()
        {
            //// Disable if we don't support image effects
            //if (!SystemInfo.supportsImageEffects)
            //{
            //    enabled = false;
            //    return;
            //}
            //// Disable the image effect if the shader can't run on the users graphics card
            //if (!edgeShader || !edgeShader.isSupported)
            //{
            //    enabled = false;
            //    return;
            //}
        }

        protected void OnDisable()
        {
            if (_material)
            {
                DestroyImmediate(_material);
            }
        }
        
        //Mono Methods//
        void Awake() {
		}

		void Start() {
        }

       void OnRenderImage(RenderTexture source, RenderTexture destination) {

            foreach (ColorPair pair in colorPairs)
            {
                material.SetColor(pair.name, pair.data);
            }
            foreach (FloatPair pair in floatPairs)
            {
                material.SetFloat(pair.name, pair.data);
            }
            
            RenderTexture buffer = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            Graphics.Blit(source, buffer, material);
            
            Graphics.Blit(buffer, destination);
            
            RenderTexture.ReleaseTemporary(buffer);
        }
	}

}