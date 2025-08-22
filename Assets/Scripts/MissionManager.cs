using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private int missionCount = 2;
    [SerializeField] private MissionBox missionBox;
    
    private List<MissionData> _alMissionDatas = new();

    private async void Start()
    {
        _alMissionDatas = await TsvLoader.LoadTableAsync<MissionData>("Missions");
        
    }

    private void CreateMissions()
    {
        for (int i = 0; i < missionCount; i++)
        {
            
        }
    }
    
    
}
