using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    public float stopSpeed;
    public float moveSpeed;
    public float maxSpeed;
    public float jumpPower;
    public float reboundPower;
    public float stompDistance;
    public float safeTime;
    private int playerLayer;
    private int enemyLayer;
    public AudioClip audioJump;
    public AudioClip audioItem;
    public AudioClip audioStomp;
    public AudioClip audioDamaged;
    public AudioClip audioDeath;
    public AudioClip audioNextStage;
    public AudioClip audioClear;
    public GameManager gameManager;
    public enum AudioAction {JUMP, ITEM, STOMP, DAMAGED, DEATH, NEXTSTAGE, CLEAR}
    Dictionary<AudioAction, AudioClip> audioCollection;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator ani; 
    AudioSource ad;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        ad = GetComponent<AudioSource>();
                // 딕셔너리 초기화
        audioCollection = new Dictionary<AudioAction, AudioClip>{
            {AudioAction.JUMP, audioJump},
            {AudioAction.ITEM, audioItem},
            {AudioAction.STOMP, audioStomp},
            {AudioAction.DAMAGED, audioDamaged},
            {AudioAction.DEATH, audioDeath},
            {AudioAction.NEXTSTAGE, audioNextStage},
            {AudioAction.CLEAR, audioClear}
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // R: Player 위치 초기화
            ResetPosition();
            
        if (Input.GetButtonDown("Jump") && !ani.GetBool("isJump")){
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            ani.SetBool("isJump", true);
            PlaySound(AudioAction.JUMP);
        }
        
        if (Input.GetButton("Horizontal")) // 바라보는 방향
            sr.flipX = Input.GetAxis("Horizontal") < 0;
        
        ani.SetBool("isRun", Mathf.Abs(rb.velocity.x) >= 2);
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Move Speed
        rb.AddForce(Vector2.right * moveInput * moveSpeed, ForceMode2D.Impulse);
        
        if (Mathf.Abs(rb.velocity.x) > maxSpeed) // Max Speed
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        
        // 바닥 확인 (by Ray)
        if (rb.velocity.y < 0){ // 떨어지는 중
            Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector3.down, 2, LayerMask.GetMask("Platform"));
            // 바닥 확인
            if (rayHit.collider != null && rayHit.distance < 1)
                ani.SetBool("isJump", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin"){
            other.gameObject.SetActive(false);
            gameManager.stagePoint += 50;
        } else if (other.gameObject.tag == "Finish"){
            gameManager.NextStage();
            ResetPosition();
        } else if (other.gameObject.tag == "GameManager"){ // 추락
            OnDamaged(other.transform.position);
            ResetPosition();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy"){
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            // Stomp
            if (transform.position.y - other.transform.position.y > stompDistance){ // 위에서 밟음
                rb.AddForce(Vector2.up * reboundPower, ForceMode2D.Impulse);
                gameManager.stagePoint += 100;
                enemy.OnStomped();
            } else { // 실패 시 피해
                OnDamaged(other.transform.position);
            }
        } else if (other.gameObject.tag == "Trap"){
            OnDamaged(other.transform.position);
        }
    }

    void OnDamaged(Vector2 targetPosition)
    {
        // Enemy와 충돌 무시(투명 효과)
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        // Graphics
        sr.color = new Color(1, 0, 1, 0.5f);
        ani.SetTrigger("doDamaged");
        // Physics
        int direction = (transform.position.x - targetPosition.x > 0) ? 1 : -1;
        rb.AddForce(new Vector2(direction, 1) * reboundPower, ForceMode2D.Impulse);
        // Logics
        gameManager.UILifeDamaged();
        Invoke(nameof(OffDamaged), safeTime);
    }

    void OffDamaged()
    {
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false); // 투명 해제
        sr.color = new Color(1, 1, 1, 1);
    }

    void ResetPosition()
    {
        transform.position = new Vector2(0, 1);
    }
    public void PlaySound(AudioAction action)
    {
        if (audioCollection.TryGetValue(action, out AudioClip clip)){
            ad.clip = clip;
            ad.Play();
        }
    }
}
