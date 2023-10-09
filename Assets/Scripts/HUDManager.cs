using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    
    #region SingleTon
    
    public static HUDManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
    
    [SerializeField] private GameObject colorPickingCanvas;
    [SerializeField] private GameObject paintingCanvas;
    
    [Space]
    
    [HideInInspector] public WebCamTexture webcamTexture;
    [SerializeField] private RawImage rawImage;

    [SerializeField] private TextMeshProUGUI debugText;
    private void Start()
    {
        // Render the camera output to a rendertexture
        webcamTexture = new WebCamTexture
        {
            deviceName = WebCamTexture.devices[^1].name
        };
        rawImage.texture = webcamTexture;
        webcamTexture.Play();
        
        //Set both canvas to inactive
        DisableHUD();
    }

    private void Update()
    {
        debugText.text = webcamTexture.deviceName + " - " + webcamTexture.isPlaying + " - " +
                         webcamTexture.didUpdateThisFrame + webcamTexture;
    }

    public void EnableColorDetectionHUD()
    {
        paintingCanvas.SetActive(false);
        colorPickingCanvas.SetActive(true);
    }
    
    public void EnablePaintingHUD()
    {
        colorPickingCanvas.SetActive(false);
        paintingCanvas.SetActive(true);
    }

    public void DisableHUD()
    {
        colorPickingCanvas.SetActive(false);
        paintingCanvas.SetActive(false);
    }
}
