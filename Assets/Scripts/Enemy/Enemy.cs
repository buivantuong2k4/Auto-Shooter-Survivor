using UnityEngine;



public class Enemy : MonoBehaviour
{
    public GameObject xpOrbPrefab;
    public float moveSpeed = 2f;
    public int maxHP = 20;

    private int currentHP;
    private Transform player;

    void Start()
    {
        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            Die();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10); // mỗi lần chạm trừ 10 máu
        }
    }


    void Die()
    {
        if (xpOrbPrefab != null)
        {
            Instantiate(xpOrbPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
