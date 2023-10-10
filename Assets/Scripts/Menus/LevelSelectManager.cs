using System.Collections.Generic;
using TMPro;
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
                if (!GameManager.Instance.beatenLevels.Contains(i + 1))
                    levelButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "?";

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
