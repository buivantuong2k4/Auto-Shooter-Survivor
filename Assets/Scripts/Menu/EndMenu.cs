using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class EndMenu : MonoBehaviour
{
    public GameObject endMenuPanel;
    public TextMeshProUGUI scoreText;  // Text để hiển thị điểm
    public RawImage resultImage;       // RawImage hiển thị win/lose
    public Sprite winSprite;           // Ảnh win
    public Sprite loseSprite;          // Ảnh lose

    void Start()
    {
        // Tìm panel nếu không assign
        if (endMenuPanel == null)
        {
            endMenuPanel = gameObject;
        }

        // Khởi đầu ẩn end menu
        endMenuPanel.SetActive(false);
    }

    // Hiển thị End Menu
    public void Show()
    {
        Time.timeScale = 0f;
        endMenuPanel.SetActive(true);
        SaveHighScore();
    }

    // Hiển thị menu với hình win
    public void ShowWin()
    {
        if (resultImage != null && winSprite != null)
        {
            resultImage.texture = winSprite.texture;
            resultImage.gameObject.SetActive(true);
        }
        Show();
    }

    // Hiển thị menu với hình lose
    public void ShowLose()
    {
        if (resultImage != null && loseSprite != null)
        {
            resultImage.texture = loseSprite.texture;
            resultImage.gameObject.SetActive(true);
        }
        Show();
    }

    // Lưu điểm cao (top 5)
    private void SaveHighScore()
    {
        // Tìm PlayerLevel để lấy totalScore
        PlayerLevel playerLevel = FindFirstObjectByType<PlayerLevel>();
        if (playerLevel == null)
        {
            return;
        }

        int currentScore = playerLevel.GetTotalScore();

        // Hiển thị điểm hiện tại
        if (scoreText != null)
        {
            scoreText.text = $"Your Score: {currentScore}";
        }

        // Lấy danh sách điểm cao từ PlayerPrefs
        string highScoresString = PlayerPrefs.GetString("HighScores", "");
        List<int> highScores = new List<int>();

        // Parse điểm từ string (nếu có)
        if (!string.IsNullOrEmpty(highScoresString))
        {
            string[] scores = highScoresString.Split(',');
            foreach (string score in scores)
            {
                if (int.TryParse(score, out int s))
                {
                    highScores.Add(s);
                }
            }
        }

        // Thêm điểm hiện tại
        highScores.Add(currentScore);

        // Sort giảm dần và chỉ giữ top 5
        highScores = highScores.OrderByDescending(x => x).Take(5).ToList();

        // Lưu lại vào PlayerPrefs
        string result = string.Join(",", highScores);
        PlayerPrefs.SetString("HighScores", result);
        PlayerPrefs.Save();
    }

    // Ẩn End Menu
    public void Hide()
    {
        endMenuPanel.SetActive(false);
    }

    // Nút 1: Trở về Main Menu
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;  // Bật lại game trước khi load scene
        SceneManager.LoadScene("MainMenu");
    }

    // Nút 2: Reload lại Game Scene hiện tại
    public void RestartGame()
    {
        Time.timeScale = 1f;  // Bật lại game trước khi load scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
