using System.Collections.Generic;
using UnityEngine;

public class ShurikenWeapon : MonoBehaviour
{
    [Header("Tham chiếu")]
    public PlayerStats playerStats;
    public Transform firePoint;      // tâm xoay (thường là player)
    public GameObject bulletPrefab;  // prefab Shuriken

    [Header("Base stats RIÊNG của vũ khí này")]
    public int baseDamage = 5;
    public int baseProjectile = 3;   // số shuriken cơ bản
    public float baseFireRate = 1f;  // có thể bỏ, để đây nếu sau này dùng

    [Header("Scale theo PLAYER STATS (global level)")]
    public float damagePerGlobalLevel = 2f;
    public float projectilePerGlobalLevel = 1f;
    public float fireRatePerGlobalLevel = 0.1f;

    [Header("Tăng theo TỪNG CẤP (nhập trong Inspector)")]
    public int level2_DamageBonus = 5;
    public float level3_ProjectileBonus = 0.5f;
    public int level4_ProjectileBonus = 1;
    public int level5_DamageBonus = 10;

    [Header("Trạng thái vũ khí")]
    public bool unlocked = false;
    public int weaponLevel = 0;   // 0 = chưa có, 1–5 = cấp vũ khí

    [Header("Cài đặt VÒNG SHURIKEN")]
    public float orbitRadius = 1.5f;   // bán kính vòng
    public float orbitSpeed = 180f;    // tốc độ xoay (độ / giây)
    public Vector2 orbitOffset;        // offset so với firePoint (nếu muốn lệch lên trên, v.v.)

    // Danh sách shuriken đang xoay
    private readonly List<Shuriken> activeShurikens = new List<Shuriken>();
    private float currentAngleOffset = 0f;

    void Awake()
    {
        if (playerStats == null)
            playerStats = GetComponentInParent<PlayerStats>();
    }

    void Update()
    {
        if (!unlocked)
        {
            // nếu tắt vũ khí, dọn sạch shuriken
            if (activeShurikens.Count > 0)
                ClearRing();
            return;
        }

        if (playerStats == null || firePoint == null || bulletPrefab == null)
            return;

        int projCount = GetCurrentProjectileCount();
        int dmg = GetCurrentDamage();

        // Nếu số lượng shuriken khác với cần thiết -> build lại vòng
        if (activeShurikens.Count != projCount)
        {
            RebuildRing(projCount, dmg);
        }

        // Cập nhật góc xoay
        currentAngleOffset += orbitSpeed * Time.deltaTime;

        // Cập nhật vị trí từng shuriken
        UpdateOrbitPositions();
    }

    // ====== MỞ KHÓA / NÂNG CẤP ======
    public void UnlockWeapon()
    {
        unlocked = true;
    }

    public void UpgradeWeapon()
    {
        if (weaponLevel < 5)
            weaponLevel++;
        UnlockWeapon();
    }

    // ====== TÍNH CHỈ SỐ HIỆN TẠI ======
    int GetCurrentDamage()
    {
        float dmg =
            baseDamage * (100f + playerStats.GetDamageLevel() * damagePerGlobalLevel) / 100f;

        if (weaponLevel >= 2)
            dmg += level2_DamageBonus;

        if (weaponLevel >= 5)
            dmg += level5_DamageBonus;

        return Mathf.Max(1, Mathf.RoundToInt(dmg));
    }

    int GetCurrentProjectileCount()
    {
        float count =
            baseProjectile +
            playerStats.GetProjectileLevel() * projectilePerGlobalLevel;
        if (weaponLevel >= 3)
            count += level3_ProjectileBonus;

        if (weaponLevel >= 4)
            count += level4_ProjectileBonus;

        return Mathf.Max(1, Mathf.RoundToInt(count));
    }

    // ====== XÂY LẠI VÒNG SHURIKEN ======
    void RebuildRing(int projCount, int dmg)
    {
        ClearRing();

        for (int i = 0; i < projCount; i++)
        {
            GameObject obj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Shuriken shuriken = obj.GetComponent<Shuriken>();
            if (shuriken != null)
            {
                shuriken.Init(dmg);
                activeShurikens.Add(shuriken);
            }
        }

        // Đặt lại offset góc để vòng start đẹp
        currentAngleOffset = 0f;
        UpdateOrbitPositions();
    }

    void ClearRing()
    {
        foreach (var s in activeShurikens)
        {
            if (s != null)
                Destroy(s.gameObject);
        }
        activeShurikens.Clear();
    }

    // ====== CẬP NHẬT VỊ TRÍ XOAY ======
    void UpdateOrbitPositions()
    {
        if (activeShurikens.Count == 0)
            return;

        Vector3 center = firePoint.position + (Vector3)orbitOffset;
        int count = activeShurikens.Count;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            var s = activeShurikens[i];
            if (s == null) continue;

            float angle = currentAngleOffset + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 offset =
                new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * orbitRadius;

            Vector3 pos = center + offset;
            s.transform.position = pos;

            // Cho shuriken quay mặt ra ngoài (tùy thích)
            Vector2 dir = (pos - center).normalized;
            float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            s.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }
}
