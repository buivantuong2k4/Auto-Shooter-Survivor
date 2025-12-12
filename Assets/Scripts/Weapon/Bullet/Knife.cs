using UnityEngine;

public class Knife : MonoBehaviour
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
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("EnemyBoss"))   // kiểm tra tag
        {
            EnemyBoss enemyboss = other.GetComponent<EnemyBoss>();
            if (enemyboss != null)
            {
                enemyboss.TakeDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                EnemyBoss2 enemyboss2 = other.GetComponent<EnemyBoss2>();
                if (enemyboss2 != null)
                {
                    enemyboss2.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
}

