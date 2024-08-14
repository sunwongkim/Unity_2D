using UnityEngine;

public class Player : MonoBehaviour
{

    public float stopSpeed;
    public float moveSpeed;
    public float maxSpeed;
    Rigidbody2D rb;
    SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Player 위치 강제 이동
            transform.position = new Vector2(-1, 1);
        // Z: 회전 고정, X: 해제
        if (Input.GetKeyDown(KeyCode.Z))
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (Input.GetKeyDown(KeyCode.X))
            rb.constraints = RigidbodyConstraints2D.None;

        if (Input.GetButtonUp("Horizontal")) // Stop Speed
            rb.velocity = new Vector2(rb.velocity.normalized.x * stopSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Horizontal")) // 바라보는 방향
            sr.flipX = Input.GetAxis("Horizontal") < 0;
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Move Speed
        rb.AddForce(Vector2.right * moveInput * moveSpeed, ForceMode2D.Impulse);

        if (Mathf.Abs(rb.velocity.x) > maxSpeed) // Max Speed
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
    }
}
