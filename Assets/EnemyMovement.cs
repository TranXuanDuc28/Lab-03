using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;        // tốc độ di chuyển ngang

    [Header("Jump")]
    public float jumpForce = 7f;    // lực nhảy (đặt theo đơn vị velocity)
    public LayerMask groundLayer;   // layer để kiểm tra chạm đất
    public float groundCheckDistance = 0.1f; // khoảng cách raycast kiểm tra đất

    private Rigidbody2D rb;
    private Vector3 startPos;
    private float inputX;
    private bool wantJump;

    void Start()
    {
        startPos = transform.position; // lưu vị trí ban đầu (có thể dùng để reset nếu cần)
        rb = GetComponent<Rigidbody2D>();

        // Nếu người dùng chưa gán groundLayer trong Inspector, cho phép mọi layer làm đất
        if (groundLayer.value == 0)
            groundLayer = ~0;
    }

    void Update()
    {
        // Đọc input (A/D hoặc mũi tên trái/phải)
        inputX = Input.GetAxisRaw("Horizontal");

        // Nhảy (Space hoặc button "Jump") — đánh dấu và xử lý trong FixedUpdate
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
                wantJump = true;
        }
    }

    void FixedUpdate()
    {
        // Áp dụng vận tốc ngang
        Vector2 vel = rb.linearVelocity;
        vel.x = inputX * speed;
        rb.linearVelocity = vel;

        // Xử lý nhảy nếu có yêu cầu
        if (wantJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            wantJump = false;
        }
    }

    // Kiểm tra chạm đất bằng Raycast xuống dưới từ vị trí object
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }
}
