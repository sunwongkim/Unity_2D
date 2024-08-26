using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int Life;
    public Player player;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            player.transform.position = new Vector2(-1, 1);
            Life --;
            Debug.Log("log");
        }
    }

    public void NextStage()
    {
        stageIndex ++;
        totalPoint += stagePoint;
        stagePoint = 0;
    }
}
