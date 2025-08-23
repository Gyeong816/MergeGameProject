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

  
  public void SetData(MissionData data)
  {
    
    MissionData = data;

    missionText.text = "+" + data.RewardGold.ToString();
    
    if (data.RequiredItem1 != "null")
    {
      var sprite = ItemManager.Instance.GetIcon(data.RequiredItem1);
      if (sprite != null)
      {
        var img = Instantiate(missionIconPrefab, iconPanel);
        img.sprite = sprite;
        img.gameObject.SetActive(true);
      }
    }

    if (data.RequiredItem2 != "null")
    {
      var sprite = ItemManager.Instance.GetIcon(data.RequiredItem2);
      if (sprite != null)
      {
        var img = Instantiate(missionIconPrefab, iconPanel);
        img.sprite = sprite;
        img.gameObject.SetActive(true);
      }
    }
  }
}
