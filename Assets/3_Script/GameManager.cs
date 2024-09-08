using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int Life;
    private int playerLayer;
    private int enemyLayer;
    public GameObject[] stageArray;
    public Image[] UILifeArray;
    public Text[] UIPoint;
    public Text UIStage;
    public Text PopupMessage;
    public GameObject Popup;

    void Update()
    {
        // UIPoint.text = (totalPoint + stagePo}int).ToString();
        foreach (Text pointText in UIPoint)
            pointText.text = (totalPoint + stagePoint).ToString();
    }

    public void UILifeDamaged()
    {
        if (Life > 1) {
            Life--;
            UILifeArray[Life].color = new Color(1, 0, 0, 0.4f);
        } else {
            PlayerDeath();
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
            ShowPopup("Clear");
            AudioManager.Instance.PlayClearSound();
        }
        // 현재 스테이지만 활성화
        stageArray[stageIndex - 1].SetActive(true);  // stageIndex는 1부터 시작하므로 -1 보정
        
        UIStage.text = "STAGE " + stageIndex.ToString();
    }

    public void PlayerDeath()
    {
        Debug.Log("DEAD");
        ShowPopup("You Died..");
        AudioManager.Instance.PlayDeathSound();
    }

    public void ShowPopup(string state)
    {
        PopupMessage.text = state.ToString();
        Popup.SetActive(true);
        Time.timeScale= 0f;
    }

    public void OnRestartBtnClick() // OnClick
    {
        Time.timeScale= 1f;
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false); // 투명 해제

        SceneManager.LoadScene(0); // 현재 씬
    }
}
