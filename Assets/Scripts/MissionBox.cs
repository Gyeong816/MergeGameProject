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
  [SerializeField] private GameObject claimImage;

  private MissionManager missionManager;
  private bool required1Done = false;
  private bool required2Done = false;
  private bool hasTowItems;
  

  public void SetData(MissionData data, MissionManager manager)
  {
    claimButton.onClick.AddListener(OnButtonClicked);
    missionManager = manager;
    MissionData = data;
    missionText.text = "+" + data.RewardGold.ToString();
    
    var icon1 = Instantiate(missionIconPrefab, iconPanel);
    icon1.sprite = ItemManager.Instance.GetIcon(data.RequiredItem1);
    required1Done = false;

    if (data.RequiredItem2 != "null")
    {
      var icon2 = Instantiate(missionIconPrefab, iconPanel);
      icon2.sprite = ItemManager.Instance.GetIcon(data.RequiredItem2);
      required2Done = false;
      hasTowItems = true;
    }
    else
    {
      required2Done = true;
      hasTowItems = false;
    }
  }
  
  public void CheckProgress()
  {
    List<string> currentItemsNames = ItemManager.Instance.GetActiveItemsNames();
    
    required1Done = false;
    required2Done = !hasTowItems; 

    foreach (var name in currentItemsNames)
    {
      if (MissionData.RequiredItem1 == name)
        required1Done = true;

      if (hasTowItems && MissionData.RequiredItem2 == name)
        required2Done = true;
    }

    claimButton.gameObject.SetActive(required1Done && required2Done);
    claimImage.SetActive(required1Done && required2Done);
  }

  private void OnButtonClicked()
  {
    ItemManager.Instance.ConsumeItem(MissionData.RequiredItem1);
    if (hasTowItems)
      ItemManager.Instance.ConsumeItem(MissionData.RequiredItem2);
    missionManager.RemoveMission(this);
    Destroy(gameObject);
  }
}
