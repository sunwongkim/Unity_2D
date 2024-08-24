using UnityEngine;

public class Player : MonoBehaviour
{

    public float stopSpeed;
    public float moveSpeed;
    public float maxSpeed;
    public float jumpPower;
    public float reboundPower;
    public float stompDistance;
    public float safeTime;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator ani;
    Enemy enemyScript;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        // R: Player 위치 초기화, Z: 회전 고정, X: 해제
        if (Input.GetKeyDown(KeyCode.R))
            transform.position = new Vector2(-1, 1);
        if (Input.GetKeyDown(KeyCode.Z))
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (Input.GetKeyDown(KeyCode.X))
            rb.constraints = RigidbodyConstraints2D.None;

        if (Input.GetButtonDown("Jump") && !ani.GetBool("isJump")){
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            ani.SetBool("isJump", true);
        }

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
        
        // Player보다 앞에서 바닥으로 Ray 조사하여 바닥 확인
        if (rb.velocity.y < 0){ // 떨어지는 중
            Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null){
                if (rayHit.distance < 0.5f)
                    ani.SetBool("isJump", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy"){
            if (enemyScript == null) // 처음에 한 번 캐싱
                enemyScript = other.gameObject.GetComponent<Enemy>();
            // Stomp
            if (transform.position.y - other.transform.position.y > stompDistance){ // 적보다 위에 위치
                rb.AddForce(Vector2.up * reboundPower, ForceMode2D.Impulse);
                enemyScript.OnStomped();
            } else { // Damaged
                OnDamaged(other.transform.position);
            }
        } else if (other.gameObject.tag == "Trap"){
            OnDamaged(other.transform.position);
        }
    }

    void OnDamaged(Vector2 targetPosition)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDamaged");
        sr.color = new Color(1, 0, 1, 0.5f);
        ani.SetTrigger("doDamaged");

        int direction = (transform.position.x - targetPosition.x > 0) ? 1 : -1;
        rb.AddForce(new Vector2(direction, 1) * reboundPower, ForceMode2D.Impulse);

        Invoke(nameof(OffDamaged), safeTime);
    }

    void OffDamaged()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        sr.color = new Color(1, 1, 1, 1);
    }
}
