using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int Life;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnPlayerDied()
    {
        Debug.Log("Player Die...");
    }

    public void NextStage()
    {
        stageIndex ++;
        totalPoint += stagePoint;
        stagePoint = 0;
    }
}
