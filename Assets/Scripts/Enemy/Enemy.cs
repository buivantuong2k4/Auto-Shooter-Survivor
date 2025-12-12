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
    public float dropRate = 1f;   // tỉ lệ rơi xp (1 = 100%)

    private int currentHP;
    private Transform player;
    private PlayerHealth targetPlayerHealth;   // player đang trong vùng attack

    private EnemyAnimationController animController;
    private Rigidbody2D rb;

    private bool isDead = false;
    private float attackTimer = 0f;
    private Coroutine attackCoroutine;

    void Awake()
    {
        animController = GetComponent<EnemyAnimationController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentHP = maxHP;
        TryFindPlayer();
    }

    void Update()
    {
        if (isDead) return;

        // nếu player chưa có (vì spawn trễ) thì thử tìm lại
        if (player == null)
        {
            TryFindPlayer();
            if (player == null)
            {
                rb.linearVelocity = Vector2.zero;
                animController.SetRunning(false);
                return;
            }
        }

        attackTimer -= Time.deltaTime;

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

    void TryFindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void MoveTowardPlayer()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

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

        // flip trái/phải (nếu là side-view)
        if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    void TryAttack()
    {
        if (attackTimer > 0f || targetPlayerHealth == null) return;

        // nếu đã có coroutine attack đang chạy thì bỏ qua
        if (attackCoroutine != null) return;

        animController.PlayAttack();
        attackTimer = attackCooldown;

        // chạy coroutine để delay damage 0.3s
        attackCoroutine = StartCoroutine(AttackDelayRoutine());
    }

    IEnumerator AttackDelayRoutine()
    {
        yield return new WaitForSeconds(0.8f);

        // kiểm tra nếu vẫn còn chạm player thì mới deal damage
        if (targetPlayerHealth != null)
        {
            targetPlayerHealth.TakeDamage(attackDamage);
        }

        attackCoroutine = null;
    }

    public void TakeDamage(int dmg)
    {
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

        // chạy quy trình death (delay → drop → destroy)
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        // ⏱ chờ đúng thời gian deathDestroyDelay (ví dụ 0.8s)
        yield return new WaitForSeconds(deathDestroyDelay);

        // 🎁 drop orb theo tỉ lệ dropRate
        if (xpOrbPrefab != null && Random.value <= dropRate)
        {
            Instantiate(xpOrbPrefab, transform.position, Quaternion.identity);
        }

        // xoá enemy sau khi anim chết chạy xong + drop xong
        Destroy(gameObject);
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
