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
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    
    public GameObject webcamTextureCanvas;
    public GameObject colorPickingCanvas;
    
    private WebCamTexture _webCamTexture;
    public RawImage rawImage;
    
    // Start is called before the first frame update
    private void Start()
    {
        _webCamTexture = new WebCamTexture();
        rawImage.texture = _webCamTexture;
    }

    public void EnableWebcam()
    {
        webcamTextureCanvas.SetActive(true);
        _webCamTexture.Play();
    }

    public void DisableWebcam()
    {
        webcamTextureCanvas.SetActive(false);
        _webCamTexture.Stop();
    }
    
}
