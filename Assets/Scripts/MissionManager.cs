using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private int missionCount = 2;
    [SerializeField] private MissionBox missionBox;
    [SerializeField] private Transform missionPanel;
    
    private List<MissionData> _allMissionDatas = new();
    private Dictionary<string, Sprite> _iconCache = new();
    private int _currentIndex = 0; 

    private void Start()
    {
        ItemManager.Instance.OnItemsLoaded -= HandleItemsLoaded;
        ItemManager.Instance.OnItemsLoaded += HandleItemsLoaded;
    }
    
    private void HandleItemsLoaded()
    {
        _ = LoadMissionDataAsync();
    }
    
    private async Task LoadMissionDataAsync()
    {
        try
        {
            _allMissionDatas = await TsvLoader.LoadTableAsync<MissionData>("Missions");
            CreateMissions();
        }
        catch (Exception ex)
        {
            Debug.LogError("[MissionManager] 미션 로딩 실패");
        }
    }


    private void CreateMissions()
    {
        for (int i = 0; i < missionCount; i++)
        {
            if (_currentIndex >= _allMissionDatas.Count)
                break;
            
            MissionData data = _allMissionDatas[_currentIndex];
            _currentIndex++;
            
            MissionBox box = Instantiate(missionBox, missionPanel);
            box.SetData(data);
        }
    }
    
    
}
