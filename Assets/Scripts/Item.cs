using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData ItemData;
    public Transform currentSlot;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        currentSlot = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_canvas.transform);
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
       _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Slot>())
        {
            Slot targetSlot = eventData.pointerEnter.GetComponent<Slot>();
            transform.SetParent(targetSlot.transform);
            _rectTransform.anchoredPosition = Vector2.zero; 
        }
        else if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Item>())
        {
            Item targetItem = eventData.pointerEnter.GetComponent<Item>();
            int maxTier = ItemManager.Instance.GetMaxTierNum(targetItem.ItemData.ItemType);
            
            if (targetItem.ItemData.Id == ItemData.Id && targetItem.ItemData.Tier < maxTier)
            {
                Transform parentSlot = targetItem.currentSlot;
                var nextItemData = ItemManager.Instance.TryMerge(ItemData);
                ItemManager.Instance.CreateItem(nextItemData, parentSlot); 
                
                targetItem.gameObject.SetActive(false);
                gameObject.SetActive(false);
                Destroy(targetItem.gameObject);
                Destroy(gameObject);
            }
            else
            { 
                transform.SetParent(targetItem.currentSlot);
                ItemManager.Instance.SwapItems(currentSlot, targetItem);
                currentSlot = transform.parent;
               _rectTransform.anchoredPosition = Vector2.zero;
            }
            
        }
        else
        {
            transform.SetParent(currentSlot);
            _rectTransform.anchoredPosition = Vector2.zero;
        }
        
        _canvasGroup.blocksRaycasts = true;
    }
    
}
