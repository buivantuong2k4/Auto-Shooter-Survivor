using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private Enemy enemy;
    private EnemyBoss boss;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        boss = GetComponentInParent<EnemyBoss>();

        if (enemy == null && boss == null)
        {
            Debug.LogError("EnemyAttackRange: Không tìm thấy Enemy hoặc EnemyBoss trên parent!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            if (enemy != null)
                enemy.SetPlayerInRange(playerHealth);
            if (boss != null)
                boss.SetPlayerInRange(playerHealth);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            if (enemy != null)
                enemy.ClearPlayerInRange(playerHealth);
            if (boss != null)
                boss.ClearPlayerInRange(playerHealth);
        }
    }
}
