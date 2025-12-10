using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        Debug.Log("Player took damage, HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("PLAYER DEAD - GAME OVER");
        // sau này sẽ gọi GameManager.GameOver()
        // tạm thời chỉ log
        Time.timeScale = 0f;
    }
}
