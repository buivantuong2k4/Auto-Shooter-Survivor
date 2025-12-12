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
                AudioManager.Instance.PlaySFX("Shuriken");
                enemy.TakeDamage(damage);
            }
        }
        else if (other.CompareTag("EnemyBoss"))   // kiểm tra tag
        {
            EnemyBoss enemyboss = other.GetComponent<EnemyBoss>();
            if (enemyboss != null)
            {
                AudioManager.Instance.PlaySFX("Shuriken");
                enemyboss.TakeDamage(damage);

            }
            else
            {
                EnemyBoss2 enemyboss2 = other.GetComponent<EnemyBoss2>();
                if (enemyboss2 != null)
                {
                    AudioManager.Instance.PlaySFX("Shuriken");
                    enemyboss2.TakeDamage(damage);

                }
            }
        }
    }
}
