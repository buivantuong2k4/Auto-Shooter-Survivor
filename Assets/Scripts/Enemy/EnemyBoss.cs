using UnityEngine;

[RequireComponent(typeof(EnemyAnimationController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyBoss : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 3f;
    public int maxHP = 100; // Boss thì máu trâu hơn tí

    [Header("Combat")]
    public float attackCooldown = 1.5f; // Tốc độ bắn
    public GameObject bulletPrefab;     // Kéo prefab BossBullet vào đây
    public Transform firePoint;         // Vị trí đạn bay ra (đặt ở tay hoặc miệng boss)

    [Header("Drop")]
    public GameObject xpOrbPrefab;
    public float deathDestroyDelay = 0.8f;

    private int currentHP;
    private Transform player;
    private PlayerHealth targetPlayerHealth; // Player đang trong vùng attack

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

        // Nếu quên gán firePoint thì lấy vị trí của boss luôn
        if (firePoint == null) firePoint = transform;
    }

    void Update()
    {
        if (isDead) return;

        attackTimer -= Time.deltaTime;

        // Không có player thì đứng yên
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            animController.SetRunning(false);
            return;
        }

        // Logic cũ: Dựa vào EnemyAttackRange (child object) gọi hàm SetPlayerInRange
        if (targetPlayerHealth != null)
        {
            // --- TRONG VÙNG ATTACK ---
            // 1. Dừng di chuyển
            rb.linearVelocity = Vector2.zero;
            animController.SetRunning(false);

            // 2. Quay mặt về phía player kể cả khi đứng yên bắn
            FacePlayer();

            // 3. Bắn
            TryShoot();
        }
        else
        {
            // --- NGOÀI VÙNG ATTACK ---
            // Di chuyển tới player
            MoveTowardPlayer();
        }
    }

    void MoveTowardPlayer()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        // Logic giữ khoảng cách tối thiểu để không chồng lên player (0.5f)
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

        FacePlayer();
    }

    // Hàm riêng để quay mặt (Flip)
    void FacePlayer()
    {
        if (player == null) return;

        // So sánh vị trí x để lật hình
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1); // Quay trái
        else
            transform.localScale = new Vector3(1, 1, 1);  // Quay phải
    }

    // Trong script EnemyBoss.cs

    void TryShoot()
    {
        if (attackTimer > 0f) return;

        // Kiểm tra an toàn: Nếu không có player thì không bắn
        if (player == null) return;

        animController.PlayAttack();

        if (bulletPrefab != null && firePoint != null)
        {
            // Tính hướng bắn trực tiếp từ FirePoint tới vị trí hiện tại của player
            Vector2 shootDir = (player.position - firePoint.position).normalized;

            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            BossBullet bulletScript = bulletObj.GetComponent<BossBullet>();

            if (bulletScript != null)
            {
                bulletScript.Initialize(shootDir);
            }
        }

        attackTimer = attackCooldown;
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        animController.SetRunning(false);
        animController.PlayDeath();

        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        if (xpOrbPrefab != null)
        {
            Instantiate(xpOrbPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, deathDestroyDelay);
    }

    // Các hàm này vẫn giữ nguyên để Child Object (AttackRange) gọi vào
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