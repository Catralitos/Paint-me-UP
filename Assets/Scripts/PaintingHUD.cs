using CustomUI;
using UnityEngine;
using UnityEngine.UI;

public class PaintingHUD : MonoBehaviour
{
    [SerializeField] private Image currentSelectedColor;
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
        currentSelectedColor.color = SceneManager.Instance.currentColor;
    }

    private static void SwitchToColorPicker()
    {
        // Switch to next piece or game end
        SceneManager.Instance.IncreaseRound();
    }

    private void Update()
    {
        // We already have events handled for the shoot ink button
        // So all that's left is to activate the particles accordingly
        if (paintButton.buttonPressed) SceneManager.Instance.EnableParticles();
        else SceneManager.Instance.DisableParticles();
    }
}