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
        // float nextThinkTime = Random.Range(2f, 5f);
        InvokeRepeating("Think", 0, intervalTime);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(nextMove, rb.velocity.y);

        if (nextMove != 0){ // 바라보는 방향
            sr.flipX = nextMove > 0;
            ani.SetBool("isRun", true);
        } else
            ani.SetBool("isRun", false);
            
        // Platform Cheak - Ray를 객체보다 앞에 두어 바닥 체크
        Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y);
        if(rb.velocity.y < 0){
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
            if(rayHit.collider == null){
                nextMove = -nextMove;
                CancelInvoke("Think");
                InvokeRepeating("Think", 0, intervalTime);
                Debug.Log("cliff");
                // if(rayHit.distance < 0.5f)
                //     ani.SetBool("isJump", false);
            }
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2); // -1,0,1
        Debug.Log("nextMove: " + nextMove);
    }
}
