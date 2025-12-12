using UnityEngine;

public class BossAttackRange : MonoBehaviour
{
    // Sửa kiểu dữ liệu từ Enemy thành EnemyBoss
    private EnemyBoss boss;

    void Awake()
    {
        // Tìm component EnemyBoss ở object cha thay vì Enemy
        boss = GetComponentInParent<EnemyBoss>();

        if (boss == null)
        {
            Debug.LogError("BossAttackRange: Không tìm thấy EnemyBoss trên parent! Hãy chắc chắn script EnemyBoss đã được gắn vào.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Nếu không tìm thấy boss thì thôi, tránh lỗi
        if (boss == null) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Gọi hàm bên EnemyBoss
            boss.SetPlayerInRange(playerHealth);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (boss == null) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Gọi hàm bên EnemyBoss
            boss.ClearPlayerInRange(playerHealth);
        }
    }
}