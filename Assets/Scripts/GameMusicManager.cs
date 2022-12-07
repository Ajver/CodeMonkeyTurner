using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMusicManager : MonoBehaviour
{
    
    [SerializeField] private AudioClip missionCompleteClip;
    [SerializeField] private AudioClip missionFailClip;
    
    [SerializeField] private MusicPlayer musicPlayer;

    void Start()
    {
        MissionSystem.Instance.OnMissionComplete += MissionSystem_OnMissionComplete;
        MissionSystem.Instance.OnMissionFailed += MissionSystem_OnMissionFail;
    }

    private void MissionSystem_OnMissionComplete(object sender, MissionSystem.MissionCompleteReason reason)
    {
        musicPlayer.SetLoop(false);
        musicPlayer.PlayClipNow(missionCompleteClip);
    }
    
    private void MissionSystem_OnMissionFail(object sender, MissionSystem.MissionFailReason reason)
    {
        musicPlayer.SetLoop(false);
        musicPlayer.PlayClipNow(missionFailClip);
    }
    
}
