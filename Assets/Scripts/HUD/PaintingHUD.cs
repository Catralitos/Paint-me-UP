using CustomUI;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class PaintingHUD : MonoBehaviour
    {
        [SerializeField] private Image currentSelectedColor;
        [SerializeField] private Image aimingReticle;
        [Space]
        [SerializeField] private HoldButton paintButton;
        [SerializeField] private Button confirmPaintedButton;

        private void Start()
        {
            // Add function to the button 
            confirmPaintedButton.onClick.AddListener(SwitchToColorPicker);
        }

        private void OnEnable()
        {
            // Let the player know the color they are painting with
            currentSelectedColor.color = PaintingSceneManager.Instance != null ? PaintingSceneManager.Instance.currentColor : new Color(1,1,1,0);
            aimingReticle.color = currentSelectedColor.color;
        }

        private static void SwitchToColorPicker()
        {
            // Switch to next piece or game end
            PaintingSceneManager.Instance.IncreaseRound();
        }

        private void Update()
        {
            // We already have events handled for the shoot ink button
            // So all that's left is to activate the particles accordingly
            if (paintButton.buttonPressed) PaintingSceneManager.Instance.EnableParticles();
            else PaintingSceneManager.Instance.DisableParticles();
        }
    }
}