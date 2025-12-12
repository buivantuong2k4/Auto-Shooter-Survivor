using UnityEngine;
using TMPro;

public class Timecount : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isRunning = true;
    public TextMeshProUGUI timeText;  // UI text để hiển thị thời gian

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    // Hiển thị thời gian lên UI
    private void UpdateTimeDisplay()
    {
        if (timeText != null)
        {
            int minutes = (int)(elapsedTime / 60f);
            int seconds = (int)(elapsedTime % 60f);
            timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    // Lấy thời gian hiện tại (seconds)
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // Lấy thời gian dưới dạng string (MM:SS)
    public string GetTimeFormatted()
    {
        int minutes = (int)(elapsedTime / 60f);
        int seconds = (int)(elapsedTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    // Đặt lại thời gian
    public void SetTime(float newTime)
    {
        elapsedTime = newTime;
        UpdateTimeDisplay();
    }

    // Bắt đầu đếm thời gian
    public void Start()
    {
        isRunning = true;
    }

    // Dừng đếm thời gian
    public void Stop()
    {
        isRunning = false;
    }

    // Tiếp tục đếm thời gian
    public void Resume()
    {
        isRunning = true;
    }

    // Reset thời gian về 0
    public void Reset()
    {
        elapsedTime = 0f;
        UpdateTimeDisplay();
    }
}
