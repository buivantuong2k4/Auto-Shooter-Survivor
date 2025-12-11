using UnityEngine;

public class PlayerAimAndShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireCooldown = 0.2f;

    private float fireTimer = 0f;
    private Camera cam;

    // KHÔNG public nữa, chỉ giữ private
    private PlayerAnimationController animController;

    void Awake()
    {
        // TỰ TÌM script animation trên cùng GameObject
        animController = GetComponent<PlayerAnimationController>();

        if (animController == null)
        {
            Debug.LogError("PlayerAimAndShoot: Không tìm thấy PlayerAnimationController trên " + gameObject.name);
        }
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        AimTowardsMouse();
        HandleShooting();
    }

    void AimTowardsMouse()
    {
        if (cam == null) return;

        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 dir = mouseWorldPos - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void HandleShooting()
    {
        fireTimer -= Time.deltaTime;

        if ((Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space)) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireCooldown;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        PlayerStats stats = GetComponent<PlayerStats>();
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (stats != null && bullet != null)
        {
            bullet.damage = Mathf.RoundToInt(bullet.damage * stats.mainWeaponDamageMultiplier);
        }

        // 👉 GỌI THẲNG ANIMATION, KHÔNG GÁN TAY
        if (animController != null)
        {
            animController.PlayShoot();
        }
    }
}
