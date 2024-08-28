using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float nextMove;
    public float intervalTime;
    public float rebound;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator ani;
    CapsuleCollider2D cd;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        cd = GetComponent<CapsuleCollider2D>();
        // float nextMoveTime = Random.Range(2f, 5f);
        InvokeRepeating(nameof(EnemyAI), 0, intervalTime);
        // Enemy 끼리는 충돌하지 않음
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer, true);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(nextMove, rb.velocity.y);
        // 바라보는 방향
        if (nextMove != 0){
            sr.flipX = nextMove > 0;
            ani.SetBool("isRun", true);
        } else
            ani.SetBool("isRun", false);

        // Platform Cheak by Ray
        Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y);
        if(rb.velocity.y < 0){
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
            if(rayHit.collider == null)
                nextMove = -nextMove;
        }
    }

    void EnemyAI()
    {
        nextMove = Random.Range(-1, 2); // -1,0,1
    }

    public void OnStomped()
    {
        rb.AddForce(Vector2.up * rebound, ForceMode2D.Impulse);
        sr.color = new Color(1, 1, 1, 0.5f);
        sr.flipY = true;
        cd.enabled = false;
        Debug.Log("Stomped");
        Invoke(nameof(DeleteEnemy), 3);
    }

    void DeleteEnemy()
    {
        gameObject.SetActive(false);
    }
}
