using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyAnimationController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyBoss2 : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 3f;
    public int maxHP = 100; // Boss thì máu trâu hơn tí

    [Header("Combat")]
    public float attackCooldown = 1.5f; // Tốc độ bắn
    public float attackRange = 5f;      // Tầm bắn
    public GameObject bulletPrefab;     // Kéo prefab BossBullet vào đây
    public Transform firePoint;         // Vị trí đạn bay ra (đặt ở tay hoặc miệng boss)

    [Header("Multi-Shot Attack")]
    public int burstCount = 5;          // Số lần bắn liên tiếp
    public float delayBetweenBursts = 0.15f;  // Delay giữa các loạt
    public int bulletPerBurst = 3;      // Số đạn mỗi loạt
    public float spreadAngle = 40f;     // Góc spread

    [Header("Summon Attack")]
    public GameObject[] enemyPrefabs;   // 4 loại quái để triệu hồi
    public int summonCount = 5;         // Số quái triệu hồi
    public float summonRadius = 3f;     // Bán kính triệu hồi xung quanh boss

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
    private Coroutine shootCoroutine;
    private int attackPhase = 0;  // 0 = summon, 1-3 = shoot

    void Awake()
    {
        animController = GetComponent<EnemyAnimationController>();
        rb = GetComponent<Rigidbody2D>();

        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Nếu quên gán firePoint thì lấy vị trí của boss luôn
        if (firePoint == null) firePoint = transform;

        // Cập nhật CircleCollider2D để match với attackRange
        CircleCollider2D attackRangeCollider = GetComponentInChildren<CircleCollider2D>();
        if (attackRangeCollider != null)
        {
            attackRangeCollider.radius = attackRange;
        }
    }

    void Update()
    {
        if (isDead) return;

        // Nếu chưa có player thì thử tìm lại
        if (player == null)
        {
            TryFindPlayer();
        }

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
            FacePlayer();

            // Nếu cooldown xong thì bắn, nếu chưa xong thì vẫn di chuyển tới player
            if (attackTimer <= 0f)
            {
                rb.linearVelocity = Vector2.zero;
                animController.SetRunning(false);
                TryShoot();
            }
            else
            {
                // Đang cooldown, vẫn di chuyển tới player
                MoveTowardPlayer();
            }
        }
        else
        {
            // --- NGOÀI VÙNG ATTACK ---
            // Di chuyển tới player
            MoveTowardPlayer();
        }
    }

    void TryFindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
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

        rb.linearVelocity = Vector2.zero;
        animController.SetRunning(false);

        // Stop coroutine cũ nếu có
        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);

        // Phân chia thứ tự: Triệu hồi -> Bắn -> Bắn -> Bắn
        if (attackPhase == 0)
        {
            // Triệu hồi quái
            animController.PlayAttack();
            shootCoroutine = StartCoroutine(SummonEnemiesCoroutine());
        }
        else
        {
            // Bắn multi-shot
            animController.PlayAttack2();
            shootCoroutine = StartCoroutine(MultiShotAttack());
        }

        // Tăng phase và reset khi vượt 3
        attackPhase++;
        if (attackPhase > 3)
            attackPhase = 0;

        attackTimer = attackCooldown;
    }

    IEnumerator SummonEnemiesCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        SummonEnemies();
    }

    void SummonEnemies()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("[Boss2] No enemy prefabs assigned for summoning!");
            return;
        }

        // Triệu hồi 5 quái xung quanh boss
        for (int i = 0; i < summonCount; i++)
        {
            // Chọn ngẫu nhiên từ các loại quái
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Tính vị trí spawn xung quanh boss
            float angle = (360f / summonCount) * i;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector3 spawnPos = transform.position + (Vector3)direction * summonRadius;

            // Spawn quái
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    IEnumerator MultiShotAttack()
    {
        // Bắn 5 loạt liên tiếp
        for (int burst = 0; burst < burstCount; burst++)
        {
            AudioManager.Instance.PlaySFX("FireBallBoss");
            // Mỗi loạt bắn 3 tia với góc spread 40 độ
            float startAngle = -spreadAngle * 0.5f;
            float step = bulletPerBurst > 1 ? spreadAngle / (bulletPerBurst - 1) : 0f;

            for (int i = 0; i < bulletPerBurst; i++)
            {
                if (player == null || bulletPrefab == null || firePoint == null) break;

                float angleOffset = startAngle + step * i;

                // Tính hướng bắn từ firePoint tới player, sau đó rotate thêm angleOffset
                Vector2 directionToPlayer = (player.position - firePoint.position).normalized;
                float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

                Quaternion rot = Quaternion.AngleAxis(baseAngle + angleOffset, Vector3.forward);
                Vector2 shootDir = rot * Vector2.right;

                // Spawn bullet
                GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                BossBullet bulletScript = bulletObj.GetComponent<BossBullet>();
                if (bulletScript != null)
                {
                    bulletScript.Initialize(shootDir);
                }
            }

            // Delay giữa các loạt
            if (burst < burstCount - 1)
            {
                yield return new WaitForSeconds(delayBetweenBursts);
            }
        }

        shootCoroutine = null;
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

        // Thông báo cho EnemySpawner boss đã chết
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.OnBossDied();
        }

        rb.linearVelocity = Vector2.zero;
        animController.SetRunning(false);
        animController.PlayDeath();
        EndMenu endMenu = FindFirstObjectByType<EndMenu>();
        if (endMenu != null)
        {
            endMenu.ShowWin();
        }
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