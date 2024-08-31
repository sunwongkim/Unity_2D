using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int Life;
    public GameObject[] stageArray;
    public Image[] UILifeArray;
    public Text UIPoint;
    public Text UIStage;
    public GameObject RestartBtn;

    void Start()
    {
        UIStage.text = "STAGE " + stageIndex.ToString();
    }

    void Update()
    {
        UIPoint.text = "SCORE: " + (totalPoint + stagePoint).ToString();
    }
    public void OnPlayerDied()
    {
        // Debug.Log("Player Die...");
    }

    public void UILifeDamaged()
    {
        if (Life > 1) {
            Life--;
            UILifeArray[Life].color = new Color(1, 0, 0, 0.4f);
        } else {
            RestartBtn.SetActive(true);
            Debug.Log("DEAD");
        }
    }

    public void NextStage()
    {
        // 모든 스테이지 비활성화
        foreach (GameObject stage in stageArray)
            stage.SetActive(false);
        // 현재 스테이지만 활성화
        if (stageIndex < stageArray.Length) {
            stageIndex++;
        } else {
            RestartBtn.SetActive(true);
            // stageIndex = 1;
        }
        stageArray[stageIndex - 1].SetActive(true);  // stageIndex는 1부터 시작하므로 -1 해줌
        
        UIStage.text = "STAGE " + stageIndex.ToString();
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(0); // 현재 씬
    }
}
