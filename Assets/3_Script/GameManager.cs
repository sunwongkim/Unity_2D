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
        if (Input.GetKeyDown(KeyCode.R)) // R: Player 위치 초기화
            NextStage();
        UIPoint.text = "SCORE: " + (totalPoint + stagePoint).ToString();
    }
    
    public void OnPlayerDeath()
    {
        Debug.Log("DEAD");
        AudioManager.Instance.PlayDeathSound();
    }

    public void UILifeDamaged()
    {
        if (Life > 1) {
            Life--;
            UILifeArray[Life].color = new Color(1, 0, 0, 0.4f);
        } else {
            RestartBtn.SetActive(true);
            OnPlayerDeath();
        }
    }

    public void NextStage()
    {
        // 모든 스테이지 비활성화
        foreach (GameObject stage in stageArray)
            stage.SetActive(false);
        
        if (stageIndex < stageArray.Length) { // Next Stage
            stageIndex++;
            AudioManager.Instance.PlayNextStageSound();
        } else { // Game Clear
            RestartBtn.SetActive(true);
            AudioManager.Instance.PlayClearSound();
        }
        // 현재 스테이지만 활성화
        stageArray[stageIndex - 1].SetActive(true);  // stageIndex는 1부터 시작하므로 -1 보정
        
        UIStage.text = "STAGE " + stageIndex.ToString();
    }

    public void OnRestartBtnClick()
    {
        SceneManager.LoadScene(0); // 현재 씬
    }
}
