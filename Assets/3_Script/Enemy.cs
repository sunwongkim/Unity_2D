using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float nextMove;
    public float intervalTime;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator ani;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        // float nextMoveTime = Random.Range(2f, 5f);
        InvokeRepeating(nameof(EnemyAI), 0, intervalTime);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(nextMove, rb.velocity.y);

        if (nextMove != 0){ // 바라보는 방향
            sr.flipX = nextMove > 0;
            ani.SetBool("isRun", true);
        } else
            ani.SetBool("isRun", false);
            
        // Platform Cheak by Ray
        Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y);
        if(rb.velocity.y < 0){
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
            if(rayHit.collider == null){
                nextMove = -nextMove;
                Debug.Log("cliff");
            }
        }
    }

    void EnemyAI()
    {
        nextMove = Random.Range(-1, 2); // -1,0,1
        // Debug.Log("nextMove: " + nextMove);
    }

    public void OnStomped()
    {
        sr.flipY = !sr.flipY;
        // 죽는 모션 후 비활성화
    }
}
