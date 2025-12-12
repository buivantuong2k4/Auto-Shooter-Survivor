using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private int damage;

    public void Init(int dmg)
    {
        damage = dmg;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                AudioManager.Instance.PlaySFX("Attack_suriken");
                enemy.TakeDamage(damage);
            }
        }
        else if (other.CompareTag("EnemyBoss"))   // kiểm tra tag
        {
            EnemyBoss enemyboss = other.GetComponent<EnemyBoss>();
            if (enemyboss != null)
            {
                AudioManager.Instance.PlaySFX("Attack_suriken");
                enemyboss.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
