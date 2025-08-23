using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionBox : MonoBehaviour
{
  public MissionData MissionData;
  
  [SerializeField] Image missionIconPrefab;
  [SerializeField] private Transform iconPanel;
  [SerializeField] private TextMeshProUGUI missionText;
  [SerializeField] private Button claimButton;

  private MissionManager missionManager;
  private bool required1Done = false;
  private bool required2Done = false;

  private void Awake()
  {
    claimButton.onClick.AddListener(OnButtonClicked);
  }

  public void SetData(MissionData data, MissionManager manager)
  {

    missionManager = manager;
    MissionData = data;
    missionText.text = "+" + data.RewardGold.ToString();
    
    var icon1 = Instantiate(missionIconPrefab, iconPanel);
    icon1.sprite = ItemManager.Instance.GetIcon(data.RequiredItem1);
    required1Done = false;
    required2Done = true;

    if (data.RequiredItem2 != "null")
    {
      var icon2 = Instantiate(missionIconPrefab, iconPanel);
      icon2.sprite = ItemManager.Instance.GetIcon(data.RequiredItem1);
      required2Done = false;
    }
  }
  
  public void CheckProgress()
  {
    List<string> currentItemsNames = ItemManager.Instance.GetActiveItemsNames();

    foreach (var name in currentItemsNames)
    {
      if (MissionData.RequiredItem1 == name)
        required1Done = true;
    
      if(MissionData.RequiredItem2 == name)
        required2Done = true;
    }

    if (required1Done && required2Done)
    {
      
      claimButton.gameObject.SetActive(true);
    }
  }

  private void OnButtonClicked()
  {
    missionManager.RemoveMission(this);
    Destroy(gameObject);
  }
}
