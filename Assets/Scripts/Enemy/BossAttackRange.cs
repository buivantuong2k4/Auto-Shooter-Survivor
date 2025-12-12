using UnityEngine;

public class BossAttackRange : MonoBehaviour
{
    private EnemyBoss boss;
    private EnemyBoss2 boss2;

    void Awake()
    {
        boss = GetComponentInParent<EnemyBoss>();
        boss2 = GetComponentInParent<EnemyBoss2>();

        if (boss == null && boss2 == null)
        {
            Debug.LogError("BossAttackRange: Không tìm thấy EnemyBoss hoặc EnemyBoss2 trên parent!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            if (boss != null)
                boss.SetPlayerInRange(playerHealth);
            if (boss2 != null)
                boss2.SetPlayerInRange(playerHealth);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            if (boss != null)
                boss.ClearPlayerInRange(playerHealth);
            if (boss2 != null)
                boss2.ClearPlayerInRange(playerHealth);
        }
    }
}