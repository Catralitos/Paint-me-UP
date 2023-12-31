// Code adapted from https://github.com/mixandjam/Splatoon-Ink
// Comments mine to help me understand/modify code

using UnityEngine;
using UnityEngine.Rendering;

namespace Painting
{
    public class PaintManager : MonoBehaviour{
    
        // The shader we're using to paint
        public Shader texturePaint;
        public Shader extendIslands;

        // The properties of the above shaders
        private readonly int _prepareUVid = Shader.PropertyToID("_PrepareUV");
        private readonly int _positionID = Shader.PropertyToID("_PainterPosition");
        private readonly int _hardnessID = Shader.PropertyToID("_Hardness");
        private readonly int _strengthID = Shader.PropertyToID("_Strength");
        private readonly int _radiusID = Shader.PropertyToID("_Radius");
        private readonly int _colorID = Shader.PropertyToID("_PainterColor");
        private readonly int _textureID = Shader.PropertyToID("_MainTex");
        private readonly int _uvOffsetID = Shader.PropertyToID("_OffsetUV");
        private readonly int _uvIslandsID = Shader.PropertyToID("_UVIslands");
        
        // The material we're creating for the paint
        private Material _paintMaterial;
        private Material _extendMaterial;

        // The command buffer used to run a series of rendering commands
        private CommandBuffer _command;

        #region SingleTon
    
        public static PaintManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // We create the material using the shader
                _paintMaterial = new Material(texturePaint);
                _extendMaterial = new Material(extendIslands);
                // And start the command buffer
                _command = new CommandBuffer();
                _command.name = "CommandBuffer - " + gameObject.name;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion
    
        public void InitTextures(Paintable paintable){
            // Get the properties of the object to be prepared for rendering
            RenderTexture mask = paintable.GetMask();
            RenderTexture uvIslands = paintable.GetUVIslands();
            RenderTexture extend = paintable.GetExtend();
            RenderTexture support = paintable.GetSupport();
            Renderer rend = paintable.GetRenderer();

            // Set the render textures as targets
            _command.SetRenderTarget(mask);
            _command.SetRenderTarget(extend);
            _command.SetRenderTarget(support);

            // Set prepareUVid as 1, so it's marked as not painted
            _paintMaterial.SetFloat(_prepareUVid, 1);
            _command.SetRenderTarget(uvIslands);
            // Using the object's renderer, draw the object with the paintable material 
            _command.DrawRenderer(rend, _paintMaterial, 0);

            // Execute the commands and clear the buffer
            Graphics.ExecuteCommandBuffer(_command);
            _command.Clear();
        }


        public void Paint(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = 1f, float strength = 1f, Color? color = null){
            // Get the properties of the object to be painted
            RenderTexture mask = paintable.GetMask();
            RenderTexture uvIslands = paintable.GetUVIslands();
            RenderTexture extend = paintable.GetExtend();
            RenderTexture support = paintable.GetSupport();
            Renderer rend = paintable.GetRenderer();

            // Set prepareUVid as 1, so it's marked as paintable
            _paintMaterial.SetFloat(_prepareUVid, 0);
            // Set all the other properties (we're leaving them as default)
            _paintMaterial.SetVector(_positionID, pos);
            _paintMaterial.SetFloat(_hardnessID, hardness);
            _paintMaterial.SetFloat(_strengthID, strength);
            _paintMaterial.SetFloat(_radiusID, radius);
            _paintMaterial.SetTexture(_textureID, support);
            _paintMaterial.SetColor(_colorID, color ?? Color.red);
            _extendMaterial.SetFloat(_uvOffsetID, paintable.extendsIslandOffset);
            _extendMaterial.SetTexture(_uvIslandsID, uvIslands);

            // Using the object's renderer, draw the object with the paintable material 
            _command.SetRenderTarget(mask);
            _command.DrawRenderer(rend, _paintMaterial, 0);

            // Then we copy what's on the mask render texture to the support render texture
            // The one sampled on the frag shader
            // If we don't do this, the changes don't stick in the render texture.
            // Each paint splat replaces the past one
            _command.SetRenderTarget(support);
            _command.Blit(mask, support);
        
            _command.SetRenderTarget(extend);
            _command.Blit(mask, extend, _extendMaterial);
            
            // Execute the commands and clear the buffer
            Graphics.ExecuteCommandBuffer(_command);
            _command.Clear();
        }

    }
}