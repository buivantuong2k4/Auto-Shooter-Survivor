using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    private int damage;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(int dmg)
    {
        damage = dmg;
    }

    void Start()
    {
        // 1. Lưu lại hướng bay hiện tại (theo firePoint.rotation)
        Vector2 dir = transform.right;

        // 2. Xoay sprite lại cho đúng (nếu sprite đang nhìn lên trên)
        //    90 hoặc -90 tùy chiều sprite của bạn
        transform.Rotate(0f, 0f, -90f);

        // 3. Giữ nguyên hướng bay cũ
        rb.linearVelocity = dir * speed;

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
