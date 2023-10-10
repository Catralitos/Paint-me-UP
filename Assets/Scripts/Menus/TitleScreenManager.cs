using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menus
{
    public class TitleScreenManager : MonoBehaviour
    {
        
        [SerializeField] private Button startButton;
        
        private void Start()
        {
           startButton.onClick.AddListener(LoadLevelSelect);
        }

        private static void LoadLevelSelect()
        {
            SceneManager.LoadScene(1);
        }
    }
}
