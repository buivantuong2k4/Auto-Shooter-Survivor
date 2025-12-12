using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private Timecount timeCounter;

    void Awake()
    {
        timeCounter = FindFirstObjectByType<Timecount>();
    }

    public void SetMedium()
    {
        // 3m58 = 238 giây
        if (timeCounter != null)
        {
            timeCounter.SetTime(238f);
            Debug.Log("Time set to 3m58s (238s)");
        }
    }

    public void SetHard()
    {
        // 7m58 = 478 giây
        if (timeCounter != null)
        {
            timeCounter.SetTime(478f);
            Debug.Log("Time set to 7m58s (478s)");
        }
    }

    public void SetBoss1()
    {
        // 4m58 = 298 giây
        if (timeCounter != null)
        {
            timeCounter.SetTime(298f);
            Debug.Log("Time set to 4m58s (298s) - Boss 1 spawn");
        }
    }

    public void SetBoss2()
    {
        // 9m58 = 598 giây
        if (timeCounter != null)
        {
            timeCounter.SetTime(598f);
            Debug.Log("Time set to 9m58s (598s) - Boss 2 spawn");
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
}
