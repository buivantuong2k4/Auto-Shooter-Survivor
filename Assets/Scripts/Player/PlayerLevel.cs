using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 9;
    public int xpToNextLevel = 10;

    public delegate void LevelUpEvent(int newLevel);
    public event LevelUpEvent OnLevelUp;

    void Start()
    {
        // đảm bảo xpToNextLevel đúng theo công thức
        xpToNextLevel = CalculateXPForLevel(level);
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        // Lặp để xử lý trường hợp dư XP lên nhiều cấp
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        LevelUpManager.Instance.ShowLevelUp();
        xpToNextLevel = CalculateXPForLevel(level);

        Debug.Log("LEVEL UP! New Level: " + level + "  Next XP: " + xpToNextLevel);



        if (OnLevelUp != null)
            OnLevelUp(level);
    }

    int CalculateXPForLevel(int lvl)
    {
        float exponent = 2.18f;
        float baseXP = 10f;

        float xp = baseXP * Mathf.Pow(lvl, exponent);
        return Mathf.RoundToInt(xp);
    }
}
