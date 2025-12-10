using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 10;

    public delegate void LevelUpEvent(int newLevel);
    public event LevelUpEvent OnLevelUp;

    public void AddXP(int amount)
    {
        currentXP += amount;
        // Debug.Log("XP: " + currentXP + "/" + xpToNextLevel);

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.3f);

        Debug.Log("LEVEL UP! New Level: " + level);

        if (LevelUpUI.Instance != null)
        {
            LevelUpUI.Instance.Show();
        }

        if (OnLevelUp != null)
            OnLevelUp(level);
    }

}
