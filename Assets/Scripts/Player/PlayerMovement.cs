using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // dùng làm fallback nếu không có PlayerStats
    public PlayerStats playerStats;   // kéo PlayerStats vào, hoặc để trống cho Auto-find

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private PlayerAnimationController animController;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // nếu chưa gán trong Inspector thì tự tìm trên cùng GameObject
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        // nếu có PlayerStats thì dùng speed từ đó
        if (playerStats != null)
        {
            currentSpeed = playerStats.GetMoveSpeed();
        }

        rb.linearVelocity = moveInput * currentSpeed;
    }
}
