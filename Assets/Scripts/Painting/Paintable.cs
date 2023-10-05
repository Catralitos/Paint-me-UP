// Code adapted from https://github.com/mixandjam/Splatoon-Ink
// Comments mine to help me understand/modify code

using UnityEngine;

namespace Painting
{
    public class Paintable : MonoBehaviour {
        
        private const int TextureSize = 1024;
        
        private RenderTexture _maskRenderTexture;
        private RenderTexture _supportTexture;

        private Renderer _rend;

        private readonly int _maskTextureID = Shader.PropertyToID("_MaskTexture");

        public RenderTexture GetMask() => _maskRenderTexture;
        public RenderTexture GetSupport() => _supportTexture;
        public Renderer GetRenderer() => _rend;

        private void Start() {
            //Create a mask render texture
            _maskRenderTexture = new RenderTexture(TextureSize, TextureSize, 0)
            {
                filterMode = FilterMode.Bilinear
            };
            
            //Create a support render texture
            _supportTexture = new RenderTexture(TextureSize, TextureSize, 0)
            {
                filterMode = FilterMode.Bilinear
            };

            _rend = GetComponent<Renderer>();
            //Set the texture on the material to be the mask render texture we created
            _rend.material.SetTexture(_maskTextureID, _maskRenderTexture);

            //Initialize the textures
            PaintManager.Instance.InitTextures(this);
        }

        private void OnDisable(){
            _maskRenderTexture.Release();
            _supportTexture.Release();
        }
    }
}