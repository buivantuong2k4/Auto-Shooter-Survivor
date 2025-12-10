using UnityEngine;
using UnityEngine.InputSystem;

public class BulletWeapon : MonoBehaviour
{
    [Header("Tham chiếu")]
    public PlayerStats playerStats;
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Base stats RIÊNG của vũ khí này")]
    public int baseDamage = 10;
    public int baseProjectile = 1;
    public float baseFireRate = 1f;   // phát / giây

    [Header("Tăng khi NÂNG CẤP vũ khí này (weapon level)")]
    public int damagePerWeaponLevel = 5;
    public float projectilePerWeaponLevel = 0.5f;
    public float fireRatePerWeaponLevel = 0.2f;

    [Header("Scale theo PLAYER STATS (global level)")]
    public float damagePerGlobalLevel = 2f;
    public float projectilePerGlobalLevel = 0.25f;
    public float fireRatePerGlobalLevel = 0.1f;

    [Header("Trạng thái vũ khí")]
    public bool unlocked = true;   // ban đầu chưa mua
    public int weaponLevel = 0;

    private float fireTimer = 0f;
    private bool isFiring = false;
    private bool _fireContinuously;

    void Awake()
    {
        if (playerStats == null)
            playerStats = GetComponentInParent<PlayerStats>();
    }

    void Update()
    {
        if (!unlocked)
        {

            return;
        }

        if (playerStats == null || firePoint == null || bulletPrefab == null)
            return;

        fireTimer -= Time.deltaTime;
        if (fireTimer > 0f) return;
        if (_fireContinuously || isFiring)
        {
            Fire();
            isFiring = false;
            fireTimer = GetCurrentCooldown();
        }
    }

    // ====== INPUT từ PlayerInput (Action name: Shoot) ======
    // PlayerInput (Behavior = Send Messages) sẽ tự gọi hàm này
    public void OnShoot(InputValue value)
    {
        if (!unlocked)
        {
            isFiring = false;
            return;
        }
        _fireContinuously = value.isPressed;
        if (value.isPressed)
        {
            isFiring = true;
        }

    }

    // ====== MỞ KHÓA / NÂNG CẤP ======
    public void UnlockWeapon()
    {
        unlocked = true;
        weaponLevel = 0;
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
    }

    // ====== TÍNH CHỈ SỐ HIỆN TẠI ======
    int GetCurrentDamage()
    {
        float dmg =
            baseDamage
            + weaponLevel * damagePerWeaponLevel
            + playerStats.GetDamageLevel() * damagePerGlobalLevel;

        return Mathf.Max(1, Mathf.RoundToInt(dmg));
    }

    int GetCurrentProjectileCount()
    {
        float count =
            baseProjectile
            + weaponLevel * projectilePerWeaponLevel
            + playerStats.GetProjectileLevel() * projectilePerGlobalLevel;

        return Mathf.Max(1, Mathf.RoundToInt(count));
    }

    float GetCurrentFireRate()
    {
        float rate =
            baseFireRate
            + weaponLevel * fireRatePerWeaponLevel
            + playerStats.GetFireRateLevel() * fireRatePerGlobalLevel;

        return Mathf.Max(0.1f, rate);
    }

    float GetCurrentCooldown()
    {
        return 1f / GetCurrentFireRate();
    }

    // ====== BẮN ĐẠN ======
    void Fire()
    {
        int dmg = GetCurrentDamage();
        int projCount = GetCurrentProjectileCount();

        if (projCount <= 1)
        {
            SpawnBullet(firePoint.position, firePoint.rotation, dmg);
            return;
        }

        float spreadAngle = 15f;
        float startAngle = -spreadAngle * 0.5f;
        float step = spreadAngle / (projCount - 1);

        for (int i = 0; i < projCount; i++)
        {
            float angleOffset = startAngle + step * i;
            Quaternion rot = firePoint.rotation * Quaternion.Euler(0f, 0f, angleOffset);
            SpawnBullet(firePoint.position, rot, dmg);
        }

    }

    void SpawnBullet(Vector3 pos, Quaternion rot, int dmg)
    {
        GameObject obj = Instantiate(bulletPrefab, pos, rot);
        Bullet bullet = obj.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Init(dmg);
    }
}
