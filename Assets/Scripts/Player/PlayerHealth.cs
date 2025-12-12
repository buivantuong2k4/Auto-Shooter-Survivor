using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats stats;

    private float currentHP;
    private float maxHP;
    private PlayerAnimationController animController;

    void Awake()
    {
        animController = GetComponent<PlayerAnimationController>();
    }

    void Start()
    {
        stats = GetComponent<PlayerStats>();


        if (stats == null)
        {
            Debug.LogError("PlayerStats not found on Player!");
            return;
        }

        maxHP = stats.GetMaxHP();
        currentHP = maxHP;
    }

    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        Debug.Log("Player took damage, HP: " + currentHP + "/" + maxHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // Khi bạn tăng cấp HealthLevel → gọi hàm này để cập nhật max HP
    public void RefreshMaxHP()
    {
        float oldMax = maxHP;
        maxHP = stats.GetMaxHP();

        // Tăng thêm HP tương ứng (giống Vampire Survivors)
        float gain = maxHP - oldMax;
        currentHP += gain;

        Debug.Log($"Max HP updated: {oldMax} → {maxHP} (+{gain})");
    }

    void Die()
    {

        AudioManager.Instance.PlaySFX("PlayerDie");
        animController.PlayDeath();
        Debug.Log("PLAYER DEAD - GAME OVER");
        Time.timeScale = 0f;
        // SceneManager.LoadScene("EndGameScene");

    }

    public float GetCurrentHP() => currentHP;
    public float GetMaxHP() => maxHP;
}
