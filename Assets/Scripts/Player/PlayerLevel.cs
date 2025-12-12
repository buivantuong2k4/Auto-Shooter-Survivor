using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 9;
    public int xpToNextLevel = 10;
    public int totalScore = 0;  // Tổng XP nhận được (Score cuối cùng)

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
        totalScore += amount;  // Tích lũy tổng score

        // Lặp để xử lý trường hợp dư XP lên nhiều cấp
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        // Khóa level tối đa ở 50
        if (level >= 50)
        {
            return;
        }

        level++;
        LevelUpManager.Instance.ShowLevelUp();
        xpToNextLevel = CalculateXPForLevel(level);

        if (OnLevelUp != null)
            OnLevelUp(level);
    }

    int CalculateXPForLevel(int lvl)
    {
        if (lvl >= 1 && lvl < 10)
        {
            return lvl * 10;
        }
        else if (lvl >= 10 && lvl < 30)
        {
            return lvl * 20;
        }
        else if (lvl >= 30 && lvl < 50)
        {
            return lvl * 30;
        }
        return 10;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }
}
