using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public PlayerStats playerStats;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private PlayerAnimationController animController;
    private SpriteRenderer sprite;
    private PlayerAim playerAim;   // 👈 tham chiếu tới Aim

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animController = GetComponent<PlayerAnimationController>();
        sprite = GetComponent<SpriteRenderer>();
        playerAim = GetComponent<PlayerAim>();

        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (animController != null)
        {
            animController.SetRunning(isMoving);
        }

        // 👉 Chỉ flip theo hướng chạy nếu KHÔNG bị Aim “chiếm quyền” lúc bắn
        if (sprite != null && (playerAim == null || !playerAim.isShootingFlip))
        {
            if (moveInput.x > 0.1f)
                sprite.flipX = false;    // quay phải
            else if (moveInput.x < -0.1f)
                sprite.flipX = true;     // quay trái
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        if (playerStats != null)
        {
            currentSpeed = playerStats.GetMoveSpeed();
        }

        rb.linearVelocity = moveInput * currentSpeed;
    }
}
