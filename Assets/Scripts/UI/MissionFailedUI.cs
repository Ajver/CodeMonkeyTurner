using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionFailedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    private void Start()
    {
        MissionSystem.Instance.OnMissionFailed += MissionSystem_OnMissionFailed;
        
        gameObject.SetActive(false);
    }

    public void OnRetryBtnClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        }

        gameObject.SetActive(true);
    }
}
