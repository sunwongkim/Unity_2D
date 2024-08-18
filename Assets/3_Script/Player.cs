using UnityEngine;

public class Player : MonoBehaviour
{

    public float stopSpeed;
    public float moveSpeed;
    public float maxSpeed;
    public float jumpPower;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator ani;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
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

        if (Input.GetButtonDown("Jump") && !ani.GetBool("isJump")){
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            ani.SetBool("isJump", true);
        }

        // if (Input.GetButtonUp("Horizontal")) // Stop Speed <-이 값을 반대 방향으로 세게 주면 멈추는 효과 구현 가능
            // rb.velocity = new Vector2(rb.velocity.normalized.x * stopSpeed, rb.velocity.y);
            // rb.velocity = new Vector2(0, rb.velocity.y);
        // Debug.Log("rb.velocity: "+rb.velocity);
        // Debug.Log("rb.velocity.normalized.x: "+rb.velocity.normalized.x);

        if (Input.GetButton("Horizontal")) // 바라보는 방향
            sr.flipX = Input.GetAxis("Horizontal") < 0;
    
        if (Mathf.Abs(rb.velocity.x) < 2)
            ani.SetBool("isRun", false);
        else
            ani.SetBool("isRun", true);
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Move Speed
        rb.AddForce(Vector2.right * moveInput * moveSpeed, ForceMode2D.Impulse);

        if (Mathf.Abs(rb.velocity.x) > maxSpeed) // Max Speed
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        
        // Platform Cheak
        if(rb.velocity.y < 0){
            Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayHit.collider != null){
                if(rayHit.distance < 0.5f)
                    ani.SetBool("isJump", false);
            }
        }
    }
}
