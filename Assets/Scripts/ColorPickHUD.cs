using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ColorPickHUD : MonoBehaviour
{
    
    [SerializeField] private Image colorCheckCrosshair;
    [SerializeField] private int numColorChecks;
    [Space] 
    [SerializeField] private Image currentSelectedColor;
    [Space] 
    [SerializeField] private Button checkColorButton;
    [SerializeField] private Button confirmColorButton;
    
    private void Start()
    {
       // Add function to the buttons 
       checkColorButton.onClick.AddListener(CheckColorAheadWebcam);
       confirmColorButton.onClick.AddListener(SwitchToPaint);
    }

    private void OnEnable()
    {
        // Set the picked color to be transparent when the HUD reappears
        currentSelectedColor.color = new Color(1, 1, 1, 0);
        SceneManager.Instance.currentColor = currentSelectedColor.color;
    }

    private static void SwitchToPaint()
    {
        // Switch the HUDs when the player has finished selecting the color
        HUDManager.Instance.EnablePaintingHUD();
    }

    private void CheckColorAheadWebcam()
    {
        // Get the texture from the webcam texture
        // It's always active, but not rendered by the camera
        WebCamTexture webcamTexture = HUDManager.Instance.webcamTexture;
        
        // Convert to a Texture2D
        Texture2D texture2D = new Texture2D(webcamTexture.width, webcamTexture.height);
        texture2D.SetPixels32(webcamTexture.GetPixels32());
        
        // Get the colors
        Color c = GetColorFromTexture(texture2D);

        // Set the color as the new paint color and the selected color in the hud
        SceneManager.Instance.currentColor = c;
        currentSelectedColor.color = c;
    }

    private Color GetColorFromTexture(Texture2D camTexture)
    {
        // Get the center of the texture
        int x = camTexture.width / 2;
        int y = camTexture.height / 2;
        Vector2Int screenCenter = new Vector2Int(x, y);
        
        //return camTexture.GetPixel(screenCenter.x, screenCenter.y);

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
