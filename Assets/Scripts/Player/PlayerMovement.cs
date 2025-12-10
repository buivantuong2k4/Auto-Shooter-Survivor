using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal"); // A/D hoặc ←/→
        float y = Input.GetAxisRaw("Vertical");   // W/S hoặc ↑/↓

        moveInput = new Vector2(x, y).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
