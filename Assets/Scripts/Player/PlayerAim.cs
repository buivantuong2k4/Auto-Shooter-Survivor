using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    private float fireTimer;
    private Camera cam;
    private Vector2 mouseScreenPos;
    private SpriteRenderer sprite;

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
    }

    void Aim()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 dir = mouseWorld - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        sprite.flipX = dir.x < 0;
    }
}
