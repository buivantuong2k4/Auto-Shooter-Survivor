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

    private float healTimer = 0f;
    private const float HEAL_INTERVAL = 1f;  // Hồi phục mỗi 1 giây
    private const float HEAL_PERCENTAGE = 0.01f;  // 1% maxHP

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

    void Update()
    {
        // Tự động hồi HP 1% mỗi 1 giây
        healTimer += Time.deltaTime;
        if (healTimer >= HEAL_INTERVAL)
        {
            healTimer = 0f;
            Heal(maxHP * HEAL_PERCENTAGE);
        }
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

    // Hồi phục HP
    private void Heal(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
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

        // Tìm EndMenu và gọi Show để hiển thị end menu
        EndMenu endMenu = FindFirstObjectByType<EndMenu>();
        if (endMenu != null)
        {
            endMenu.Show();
        }
    }

    public float GetCurrentHP() => currentHP;
    public float GetMaxHP() => maxHP;
}
