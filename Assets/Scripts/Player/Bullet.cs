using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float lifeTime = 2f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu trúng Enemy thì gây damage
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {

            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
