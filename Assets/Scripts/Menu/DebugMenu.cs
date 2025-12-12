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


}
