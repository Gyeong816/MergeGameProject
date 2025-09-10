using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class FurnitureUnlockUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int furnitureId;  
    [SerializeField] private int unlockCost = 200;
    [SerializeField] private GameObject coinImage;
    private Image _image;
    
    private Color unlockColor = new Color(1f, 1f, 1f, 1f);

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    private void Start()
    {
        if (GameManager.Instance.Player.unlockedFurnitureIds.Contains(furnitureId))
        {
            UnlockFurniture();
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.TryUnlockFurniture(unlockCost))
        {
            GameManager.Instance.Player.unlockedFurnitureIds.Add(furnitureId);
            UnlockFurniture();
        }
        else
        {
            Debug.Log("해금 실패");
        }
    }

    private void UnlockFurniture()
    {
        coinImage.SetActive(false);
        _image.color = unlockColor;
    }
}
