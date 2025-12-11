using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shootFlipDuration = 0.1f;   // thời gian “ưu tiên flip theo bắn”

    private float fireTimer;
    private Camera cam;
    private Vector2 mouseScreenPos;
    private SpriteRenderer sprite;

    // cho Movement biết đang flip theo bắn
    [HideInInspector] public bool isShootingFlip;
    private float shootingFlipTimer;

    void Start()
    {
        cam = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnLook(InputValue value)
    {
        mouseScreenPos = value.Get<Vector2>();
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;
        Aim();

        // Đếm ngược thời gian ưu tiên flip theo bắn
        if (isShootingFlip)
        {
            shootingFlipTimer -= Time.deltaTime;
            if (shootingFlipTimer <= 0f)
            {
                isShootingFlip = false;
            }
        }
    }

    void Aim()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 dir = mouseWorld - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // ❌ KHÔNG flip nhân vật ở đây nữa
        // sprite.flipX = dir.x < 0;
    }

    // Hàm bắn – PlayerInput gọi hàm này
    public void OnShoot(InputValue value)
    {
        if (!value.isPressed) return; // nếu bạn dùng kiểu press

        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 dir = mouseWorld - transform.position;


        // 👉 Chỉ flip nhân vật tại thời điểm bắn
        if (sprite != null)
        {
            sprite.flipX = dir.x < 0;
        }

        isShootingFlip = true;
        shootingFlipTimer = shootFlipDuration;
    }
}
