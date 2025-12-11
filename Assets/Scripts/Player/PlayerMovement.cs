using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private PlayerAnimationController animController;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animController = GetComponent<PlayerAnimationController>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(x, y).normalized;

        bool isRunning = moveInput.sqrMagnitude > 0.01f;
        animController.SetRunning(isRunning);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
