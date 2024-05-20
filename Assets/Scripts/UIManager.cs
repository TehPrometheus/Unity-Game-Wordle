using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
namespace Wordle
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] GameObject gameOverMenu;
        [SerializeField] TMP_Text gameOverText;
        [SerializeField] Wordle wordle;
        private void OnEnable()
        {
            wordle.gameOver += EnableGameOverMenu;        
            gameOverMenu.SetActive(false);
        }

        private void OnDisable()
        {
            wordle.gameOver -= EnableGameOverMenu;
        }

        private void EnableGameOverMenu(bool playerWon)
        {
            if (playerWon)
                gameOverText.text = "Correct!\n";
            else 
                gameOverText.text = "Better luck next time!\n";

            gameOverText.text += "The word was: " + wordle.solution;
            gameOverMenu.transform.SetAsLastSibling();
            gameOverMenu.SetActive(true);
        }

        public void OnApplicationQuit()
        {
            Application.Quit();
        }

        public void ReloadGame()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

    }
}