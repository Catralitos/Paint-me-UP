using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace HUD
{
    public class ColorPickHUD : MonoBehaviour
    {
    
        [SerializeField] private Image colorCheckCrosshair;
        [SerializeField] private int numColorChecks;
        [Space] 
        [SerializeField] private Image currentSelectedColor;
        [Space] 
        [SerializeField] private Button checkColorButton;
    
        private void Start()
        {
            // Add function to the button 
            checkColorButton.onClick.AddListener(CheckColorAheadWebcam);
        }

        private void OnEnable()
        {
            // Set the picked color to be transparent when the HUD reappears
            currentSelectedColor.color = new Color(1, 1, 1, 0);
            if (PaintingSceneManager.Instance != null) PaintingSceneManager.Instance.currentColor = currentSelectedColor.color;
        }
    
        private void CheckColorAheadWebcam()
        {
        
            // Get the texture from the webcam texture
            // It's always active, but not rendered by the camera
            RenderTexture imageTexture = HUDManager.Instance.renderTexture;
        
            // Get the colors
            Color c = GetColorFromTexture(ToTexture2D(imageTexture));

            // Set the color as the new paint color and the selected color in the hud
            PaintingSceneManager.Instance.currentColor = c;
            currentSelectedColor.color = c;
            HUDManager.Instance.EnablePaintingHUD();
        }

        private static Texture2D ToTexture2D(RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
            // ReadPixels looks at the active RenderTexture.
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }

        private Color GetColorFromTexture(Texture2D camTexture)
        {
            // Get the center of the texture
            int x = camTexture.width / 2;
            int y = camTexture.height / 2;
            Vector2Int screenCenter = new Vector2Int(x, y);
        
            // Create a list to hold random points where we will also draw the color from to form an average
            List<Vector2Int> randomPositions = new List<Vector2Int> { screenCenter };

            for (int i = 0; i < numColorChecks - 1; i++)
            {
                // Add a random position that is located within the cross hair
                randomPositions.Add(Vector2Int.RoundToInt(screenCenter + Random.insideUnitCircle * colorCheckCrosshair.sprite.rect.width));
            }
        
            float rValue = 0;
            float gValue = 0;
            float bValue = 0;
        
            // Get the sum of the rgb values
            foreach (Vector2Int pos in randomPositions)
            {
                if (camTexture != null)
                {
                    Color c = camTexture.GetPixel(pos.x, pos.y);
                    rValue += c.r;
                    gValue += c.g;
                    bValue += c.b;
                }
            }

            // Return a color with the average of each component
            return new Color(rValue / randomPositions.Count, gValue / randomPositions.Count,
                bValue / randomPositions.Count, 1);
        }
    }
}
