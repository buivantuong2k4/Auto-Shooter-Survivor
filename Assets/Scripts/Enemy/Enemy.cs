using System.Collections;

using UnityEngine;

[RequireComponent(typeof(EnemyAnimationController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public int maxHP = 20;
    public int attackDamage = 10;
    public float attackCooldown = 2.0f;  // thời gian giữa 2 lần đánh

    [Header("Drop")]
    public GameObject xpOrbPrefab;
    public float deathDestroyDelay = 0.8f; // thời gian chờ sau khi chơi anim chết

    [Header("XP Drop Settings")]
    [Range(0f, 1f)]
    public float dropRate = 1f;   // tỉ lệ rơi xp

    private int currentHP;
    private Transform player;
    private PlayerHealth targetPlayerHealth;   // player đang trong vùng attack

    private EnemyAnimationController animController;
    private Rigidbody2D rb;

    private bool isDead = false;
    private float attackTimer = 0f;

    void Awake()
    {
        animController = GetComponent<EnemyAnimationController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (isDead) return;

        attackTimer -= Time.deltaTime;

        // không có player thì đứng yên
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            animController.SetRunning(false);
            return;
        }

        // nếu player đang trong vùng attack -> ưu tiên đánh
        if (targetPlayerHealth != null)
        {
            rb.linearVelocity = Vector2.zero;
            animController.SetRunning(false);

            TryAttack();
        }
        else
        {
            MoveTowardPlayer();
        }
    }

    void MoveTowardPlayer()
    {
        if (player == null) return;

        // hướng di chuyển
        Vector2 dir = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        // nếu còn xa thì chạy tới, nếu đủ gần thì giảm tốc cho đỡ giật
        if (distance > 0.5f)
        {
            rb.linearVelocity = dir * moveSpeed;
            animController.SetRunning(true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animController.SetRunning(false);
        }

        // flip hướng nhìn (nếu là game side-view)
        if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    void TryAttack()
    {
        if (attackTimer > 0f || targetPlayerHealth == null) return;

        animController.PlayAttack();
        targetPlayerHealth.TakeDamage(attackDamage);

        attackTimer = attackCooldown;
    }

    public void TakeDamage(int dmg)
    {
        Debug.Log("Hit enemy, dmg = " + dmg);
        if (isDead) return;

        currentHP -= dmg;

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // dừng di chuyển
        rb.linearVelocity = Vector2.zero;
        animController.SetRunning(false);

        // play anim chết
        animController.PlayDeath();

        // tắt collider body + vùng attack
        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        // chạy quy trình death (delay → spawn orb → destroy)
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        // 🔥 chờ animation chạy được 0.03s
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        // 🔵 spawn XP orb sau delay
        if (xpOrbPrefab != null)
        {
            Instantiate(xpOrbPrefab, transform.position, Quaternion.identity);
        }




    }

    // Hàm này được gọi từ EnemyAttackRange (child)
    public void SetPlayerInRange(PlayerHealth playerHealth)
    {
        targetPlayerHealth = playerHealth;
    }

    public void ClearPlayerInRange(PlayerHealth playerHealth)
    {
        if (targetPlayerHealth == playerHealth)
        {
            targetPlayerHealth = null;
        }
    }
}
