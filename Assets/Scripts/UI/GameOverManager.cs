using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void RestartGame()
    {
        // Load lại màn chơi chính
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Thoát game");
    }

    public void BackToMenu()
    {
        // Nếu bạn có màn hình Menu chính
        SceneManager.LoadScene("MainMenu");
    }
}