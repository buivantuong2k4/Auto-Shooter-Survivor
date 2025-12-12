using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BossBullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    public float speed = 10f;
    public int damage = 10;
    public float lifeTime = 5f; // Thời gian tự hủy nếu không trúng gì

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Hàm này được Boss gọi ngay khi sinh ra đạn
    public void Initialize(Vector2 direction)
    {
        // Set vận tốc bay thẳng theo hướng được chỉ định
        rb.linearVelocity = direction.normalized * speed;

        // Xoay viên đạn theo hướng bay
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Tự hủy sau một thời gian để đỡ nặng game
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu đụng Player
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Đạn biến mất
        }
        // Nếu đụng tường (Layer "Ground" hoặc "Wall" tùy bạn đặt)
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}