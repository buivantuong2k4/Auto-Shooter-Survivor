using UnityEngine;
using UnityEngine.InputSystem;

public class FireBallWeapon : MonoBehaviour
{
    [Header("Tham chiếu")]
    public PlayerStats playerStats;
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Base stats RIÊNG của vũ khí này")]
    public int baseDamage = 5;
    public int baseProjectile = 1;
    public float baseFireRate = 1f;   // phát / giây

    [Header("Scale theo PLAYER STATS (global level)")]
    public float damagePerGlobalLevel = 2f;
    public float projectilePerGlobalLevel = 1f;
    public float fireRatePerGlobalLevel = 0.1f;

    [Header("Tăng theo TỪNG CẤP (nhập trong Inspector)")]
    public int level2_DamageBonus = 5;        // cấp 2: +dmg
    public float level3_FireRateBonus = 0.5f; // cấp 3: +fire rate
    public int level4_ProjectileBonus = 1;    // cấp 4: +số đạn
    public int level5_DamageBonus = 10;       // cấp 5: +dmg thêm lần nữa

    [Header("Trạng thái vũ khí")]
    public bool unlocked = false;
    public int weaponLevel = 0;   // 0 = chưa có, 1–5 = cấp vũ khí

    private float fireTimer = 0f;
    private bool isFiring = false;
    private bool _fireContinuously;
    private PlayerAnimationController animController;

    void Awake()
    {
        animController = GetComponent<PlayerAnimationController>();
        if (playerStats == null)
            playerStats = GetComponentInParent<PlayerStats>();
    }

    void Update()
    {
        if (!unlocked)
            return;

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
    }

    public void UpgradeWeapon()
    {
        // Tăng tối đa tới cấp 5
        if (weaponLevel < 5)
            weaponLevel++;
        UnlockWeapon();
    }

    // ====== TÍNH CHỈ SỐ HIỆN TẠI ======
    int GetCurrentDamage()
    {
        float dmg =
            baseDamage * (100f + playerStats.GetDamageLevel() * damagePerGlobalLevel) / 100f;

        // Cấp 2: thêm dmg
        if (weaponLevel >= 2)
            dmg += level2_DamageBonus;

        // Cấp 5: thêm dmg lần nữa
        if (weaponLevel >= 5)
            dmg += level5_DamageBonus;

        return Mathf.Max(1, Mathf.RoundToInt(dmg));
    }

    int GetCurrentProjectileCount()
    {
        float count =
            baseProjectile +
            playerStats.GetProjectileLevel() * projectilePerGlobalLevel;

        // Cấp 4: tăng số lượng đạn
        if (weaponLevel >= 4)
            count += level4_ProjectileBonus;

        return Mathf.Max(1, Mathf.RoundToInt(count));
    }

    float GetCurrentFireRate()
    {
        float rate =
            baseFireRate +
            playerStats.GetFireRateLevel() * fireRatePerGlobalLevel;

        // Cấp 3: tăng fire rate
        if (weaponLevel >= 3)
            rate += level3_FireRateBonus;

        return Mathf.Max(0.1f, rate);
    }

    float GetCurrentCooldown()
    {
        return 1f / GetCurrentFireRate();
    }

    // ====== BẮN ĐẠN ======
    void Fire()
    {
        int currentChar = CharacterSelectionData.SelectedCharacterIndex;

        // Nếu là nhân vật số 1 → chơi animation Shoot
        if (currentChar == 1)
        {
            AudioManager.Instance.PlaySFX("fireball");
            animController.PlayShoot();
        }

        int dmg = GetCurrentDamage();
        int projCount = GetCurrentProjectileCount();

        if (projCount <= 1)
        {
            SpawnBullet(firePoint.position, firePoint.rotation, dmg);
            return;
        }

        float spreadAngle = 60f;
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
        FireBall bullet = obj.GetComponent<FireBall>();
        if (bullet != null)
            bullet.Init(dmg);
    }
}
