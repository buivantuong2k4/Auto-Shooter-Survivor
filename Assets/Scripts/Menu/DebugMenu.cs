using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public GameObject debugMenuPanel;  // Kéo menu debug panel vào đây
    private Timecount timeCounter;

    void Awake()
    {
        timeCounter = FindFirstObjectByType<Timecount>();
    }

    void Start()
    {
        // Kiểm tra giá trị từ MainMenu và bật/tắt menu debug
        bool shouldShowDebugMenu = GetMenuVisible();
        if (debugMenuPanel != null)
        {
            debugMenuPanel.SetActive(shouldShowDebugMenu);
        }
    }

    public void SetMedium()
    {
        // 3m = 180 giây
        if (timeCounter != null)
        {
            timeCounter.SetTime(180f);
        }
    }

    public void SetHard()
    {
        // 6m = 360 giây
        if (timeCounter != null)
        {
            timeCounter.SetTime(360f);
        }
    }

    public void SetBoss1()
    {
        // 5m = 300 giây
        if (timeCounter != null)
        {
            timeCounter.SetTime(300f);
        }
    }

    public void SetBoss2()
    {
        // 10m = 600 giây
        if (timeCounter != null)
        {
            timeCounter.SetTime(600f);
        }
    }

    public void LevelUp()
    {
        PlayerLevel playerLevel = FindFirstObjectByType<PlayerLevel>();
        if (playerLevel != null)
        {
            playerLevel.LevelUp();
            Debug.Log("Player leveled up!");
        }
    }

    // ===== MENU VISIBILITY CONTROL =====
    public void SetMenuVisible(bool isVisible)
    {
        PlayerPrefs.SetInt("MenuVisible", isVisible ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool GetMenuVisible()
    {
        return PlayerPrefs.GetInt("MenuVisible", 0) == 1;
    }
}
