using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    void Start()
    {
        // Ẩn pause menu khi khởi động
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    // Dừng trò chơi
    public void Pause()
    {
        Time.timeScale = 0f;
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    // Tiếp tục trò chơi
    public void Unpause()
    {
        Time.timeScale = 1f;
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    // Về menu chính
    public void BackToMenu()
    {
        Time.timeScale = 1f;  // Bật lại game trước khi load scene
        SceneManager.LoadScene("MainMenu");
    }

    // Chơi lại (restart game scene)
    public void Restart()
    {
        Time.timeScale = 1f;  // Bật lại game trước khi load scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
