using UnityEngine;

public class Slash : MonoBehaviour
{
    public float speed = 15f;
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
        Vector2 dir = transform.right;

        rb.linearVelocity = dir * speed;

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))   // kiểm tra tag
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}

