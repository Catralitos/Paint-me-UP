using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace HUD
{
    public class HUDManager : MonoBehaviour
    {
    
        #region SingleTon
    
        public static HUDManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // Render the camera output to a render texture
                if (Camera.main == null) return;
                _cameraBackground = Camera.main.gameObject.GetComponent<ARCameraBackground>();
                _cameraManager = Camera.main.gameObject.GetComponent<ARCameraManager>();
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
    
        public RenderTexture renderTexture;

        private ARCameraBackground _cameraBackground;
        private ARCameraManager _cameraManager;
    
        private void Start()
        {
            //Set both canvas to inactive
            DisableHUD();
        }

        private void OnEnable()
        {
            _cameraManager.frameReceived += OnCameraFrameReceived;
        }
    
        private void OnDisable()
        {
            _cameraManager.frameReceived -= OnCameraFrameReceived;
        }
    
 
        private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
        {
            foreach (Texture2D t in eventArgs.textures)
            {
                Graphics.Blit(t, renderTexture, _cameraBackground.material);
            }
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
}
