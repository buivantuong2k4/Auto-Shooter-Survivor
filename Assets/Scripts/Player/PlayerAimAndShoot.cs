using UnityEngine;

public class PlayerAimAndShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireCooldown = 0.2f;

    private float fireTimer = 0f;
    private Camera cam;

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

        if (Input.GetButton("Fire1") && fireTimer <= 0f) // Fire1 = chuột trái
        {
            Shoot();
            fireTimer = fireCooldown;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // lấy PlayerStats để tăng damage
        PlayerStats stats = GetComponent<PlayerStats>();
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (stats != null && bullet != null)
        {
            bullet.damage = Mathf.RoundToInt(bullet.damage * stats.mainWeaponDamageMultiplier);
        }
    }

}
