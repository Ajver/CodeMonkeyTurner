using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionFailedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    [SerializeField] private string mainMenuSceneName;
    
    private void Start()
    {
        MissionSystem.Instance.OnMissionFailed += MissionSystem_OnMissionFailed;
        
        gameObject.SetActive(false);
    }

    public void OnRetryBtnClicked()
    {
        SceneFader.Instance.FadeToScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToMenuBtnClicked()
    {
        SceneFader.Instance.FadeToScene(mainMenuSceneName);
    }

    private void MissionSystem_OnMissionFailed(object sender, MissionSystem.MissionFailReason reason)
    {
        switch (reason)
        {
            case MissionSystem.MissionFailReason.AllUnitsDied:
                missionDescriptionText.text = "All allies died";
                break;
            case MissionSystem.MissionFailReason.TreasureDestroyed:
                missionDescriptionText.text = "The Suitcase you were supposed to collect got destroyed";
                break;
            case MissionSystem.MissionFailReason.TestFailReason:
                missionDescriptionText.text = "Mission failed";
                break;
        }

        gameObject.SetActive(true);
    }
}
