using UnityEngine;

public class FireBurstWeapon : MonoBehaviour
{
    public bool isUnlocked = false;
    public float cooldown = 5f;
    public float radius = 2.5f;
    public int baseDamage = 20;

    private float timer = 0f;
    private int level = 0;

    void Update()
    {
        if (!isUnlocked) return;

        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            Fire();
            timer = 0f;
        }
    }

    void Fire()
    {
        // tìm tất cả Enemy trong vòng tròn
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(baseDamage + level * 10); // mỗi level cộng thêm damage
            }
        }

        Debug.Log("Fire Burst activated! Level: " + level);
        // TODO: VFX (hiệu ứng cháy)
    }

    // Gọi khi unlock/nâng cấp từ UI
    public void UnlockOrUpgrade()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            level = 1;
        }
        else
        {
            level++;
            // Có thể giảm cooldown, tăng radius, v.v.
            cooldown = Mathf.Max(1f, cooldown - 0.5f);
        }
    }

    void OnDrawGizmosSelected()
    {
        // vẽ vòng tròn trong Editor để dễ chỉnh radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
