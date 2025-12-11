using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject xpOrbPrefab;
    public float moveSpeed = 2f;
    public int maxHP = 20;

    [Header("XP Drop Settings")]
    [Range(0f, 1f)]
    public float dropRate = 1f;   // tỉ lệ rơi xp

    private int currentHP;
    private Transform player;
    private SpriteRenderer sprite;   // 👈 thêm sprite renderer

    void Start()
    {
        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        sprite = GetComponent<SpriteRenderer>(); // 👈 tự lấy sprite
    }

    void Update()
    {
        if (player == null) return;

        // hướng di chuyển
        Vector2 dir = (player.position - transform.position).normalized;

        // di chuyển kẻ thù
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

        // === XOAY TRÁI / PHẢI GIỐNG PLAYER ===
        if (dir.x > 0.01f)
            sprite.flipX = false;       // nhìn phải
        else if (dir.x < -0.01f)
            sprite.flipX = true;        // nhìn trái
    }

    public void TakeDamage(int dmg)
    {
        Debug.Log("Hit enemy, dmg = " + dmg);
        currentHP -= dmg;
        if (currentHP <= 0)
            Die();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(10);
    }

    void Die()
    {
        // drop xp theo tỉ lệ
        if (xpOrbPrefab != null && Random.value <= dropRate)
            Instantiate(xpOrbPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
