using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

public class CheckForColor : MonoBehaviour
{
    //I have seen two methods to do this
    //Once by showing the webcam on a render texture and getting pixels from there
    //Another by accessing the camera via the CPU
    //I don't know which will work better/have better performance so let's do both
    [HideInInspector] public bool usingWebcam = false;
    
    public float colorCheckRadius;
    public int numColorChecks;
    
    public Button checkColorButton;

    private ARCameraManager _arCameraManager;
    private void Start()
    {
        if (Camera.main != null) _arCameraManager = Camera.main.gameObject.GetComponent<ARCameraManager>();

        if (usingWebcam) checkColorButton.onClick.AddListener(CheckColorAheadWebcam);
        else checkColorButton.onClick.AddListener(CheckColorAheadCPU);
    }

    private void CheckColorAheadWebcam()
    {
        HUDManager.Instance.EnableWebcam();

        Color c = GetColorFromTexture(HUDManager.Instance.rawImage.texture as Texture2D);
        
        HUDManager.Instance.DisableWebcam();
    }
    
    private void CheckColorAheadCPU()
    {
        Texture2D camTexture;
        
        unsafe
        {
            //https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/manual/cpu-camera-image.html
            if (!_arCameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
                return;
        
            var conversionParams = new XRCpuImage.ConversionParams
            {
                // Get the entire image.
                inputRect = new RectInt(0, 0, image.width, image.height),

                // Downsample by 2.
                outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

                // Choose RGBA format.
                outputFormat = TextureFormat.RGBA32,

                // Flip across the vertical axis (mirror image).
                transformation = XRCpuImage.Transformation.MirrorY
            };

            // See how many bytes you need to store the final image.
            int size = image.GetConvertedDataSize(conversionParams);

            // Allocate a buffer to store the image.
            var buffer = new NativeArray<byte>(size, Allocator.Temp);

            // Extract the image data
            image.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length);

            // The image was converted to RGBA32 format and written into the provided buffer
            // so you can dispose of the XRCpuImage. You must do this or it will leak resources.
            image.Dispose();

            // At this point, you can process the image, pass it to a computer vision algorithm, etc.
            // In this example, you apply it to a texture to visualize it.

            // You've got the data; let's put it into a texture so you can visualize it.
            camTexture = new Texture2D(
                conversionParams.outputDimensions.x,
                conversionParams.outputDimensions.y,
                conversionParams.outputFormat,
                false);

            camTexture.LoadRawTextureData(buffer);
            camTexture.Apply();

            // Done with your temporary data, so you can dispose it.
            buffer.Dispose();
        }

        if (camTexture != null)
        {
            Color c = GetColorFromTexture(camTexture);
        }
    }

    private Color GetColorFromTexture(Texture2D camTexture)
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        Vector2Int screenCenter = new Vector2Int(x, y);

        List<Vector2Int> randomPositions = new List<Vector2Int> { screenCenter };

        for (int i = 0; i < numColorChecks; i++)
        {
            randomPositions.Add(Vector2Int.RoundToInt(screenCenter + Random.insideUnitCircle * colorCheckRadius));
        }
        
        float rValue = 0;
        float gValue = 0;
        float bValue = 0;
        
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

        //TODO ver como tratar esta cor
        return new Color(rValue / randomPositions.Count, gValue / randomPositions.Count,
            bValue / randomPositions.Count);
    }
}
