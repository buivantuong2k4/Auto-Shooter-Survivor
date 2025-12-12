using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class Leadboard : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText;  // Gắn TextMeshPro component từ Inspector
    private List<int> highScores = new List<int>();
    private static Leadboard instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Tìm TextMeshProUGUI component nếu không assign
        if (leaderboardText == null)
        {
            leaderboardText = GetComponent<TextMeshProUGUI>();
        }

        if (leaderboardText == null)
        {
            leaderboardText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    // Tải và hiển thị leaderboard từ PlayerPrefs
    public void DisplayLeaderboard()
    {
        LoadHighScores();
        UpdateLeaderboardUI();
    }

    // Static method để gọi từ bất kỳ đâu
    public static void Show()
    {
        if (instance != null)
        {
            instance.DisplayLeaderboard();
        }
        else
        {
            Debug.LogError("Leadboard instance not found!");
        }
    }

    // Tải điểm cao từ PlayerPrefs
    private void LoadHighScores()
    {
        highScores.Clear();
        string highScoresString = PlayerPrefs.GetString("HighScores", "");

        if (string.IsNullOrEmpty(highScoresString))
        {
            return;
        }

        string[] scores = highScoresString.Split(',');
        foreach (string score in scores)
        {
            if (int.TryParse(score, out int s))
            {
                highScores.Add(s);
            }
        }
    }

    // Cập nhật giao diện leaderboard
    private void UpdateLeaderboardUI()
    {
        if (leaderboardText == null)
        {
            return;
        }

        if (highScores.Count == 0)
        {
            leaderboardText.text = "=== LEADERBOARD ===\n\nChưa có điểm cao nào!";
            return;
        }

        string displayText = "=== LEADERBOARD ===\n\n";
        for (int i = 0; i < highScores.Count; i++)
        {
            displayText += $"{i + 1}. {highScores[i]}\n";
        }

        leaderboardText.text = displayText;
    }

    // Xóa tất cả high scores (tùy chọn)
    public void ClearLeaderboard()
    {
        PlayerPrefs.DeleteKey("HighScores");
        PlayerPrefs.Save();
        highScores.Clear();
        UpdateLeaderboardUI();
    }
}
