using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menus
{
    public class LevelSelectManager : MonoBehaviour
    {
        [SerializeField] private List<Button> levelButtons;

        [SerializeField] private Button backButton;

        private void Start()
        {
            for (int i = 0; i < levelButtons.Count; i++)
            {
                int closureIndex = i ; 
                levelButtons[closureIndex].onClick.AddListener( () => LoadLevel( closureIndex ) );
            }
            backButton.onClick.AddListener(LoadTitleScreen);
        }

        private static void LoadLevel(int buttonIndex )
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 + buttonIndex);
        }

        private static void LoadTitleScreen()
        {
            SceneManager.LoadScene(0);
        }
    }
}
