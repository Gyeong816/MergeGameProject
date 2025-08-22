using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData ItemData;
    public ItemState State => _state;
    
    [SerializeField] private Image hiddenImage;
    [SerializeField] private Image disabledImage; 
    
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private ItemState _state = ItemState.Active; 
    private Transform _currentSlot;
    
    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _currentSlot = transform.parent;
    }
    
    public void SetItemState(ItemState state)
    {
        _state = state;

        switch (_state)
        {
            case ItemState.Hidden:
                hiddenImage.gameObject.SetActive(true);
                disabledImage.gameObject.SetActive(false);
                break;

            case ItemState.Disabled:
                hiddenImage.gameObject.SetActive(false);
                disabledImage.gameObject.SetActive(true);
                disabledImage.raycastTarget = false;
                break;

            case ItemState.Active:
                hiddenImage.gameObject.SetActive(false);
                disabledImage.gameObject.SetActive(false);
                break;
        }
    }
    public void SetSlot(Transform targetSlot)
    {
        transform.SetParent(targetSlot);
        _currentSlot = targetSlot;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_state != ItemState.Active) return;
        
        transform.SetParent(_canvas.transform);
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_state != ItemState.Active) return;
        
       _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_state != ItemState.Active) return;
        
        if (eventData.pointerEnter.GetComponent<Slot>())
        {
            Slot targetSlot = eventData.pointerEnter.GetComponent<Slot>();
            transform.SetParent(targetSlot.transform);
            _rectTransform.anchoredPosition = Vector2.zero; 
        }
        else if (eventData.pointerEnter.GetComponent<Item>())
        {
            Item targetItem = eventData.pointerEnter.GetComponent<Item>();
            int maxTier = ItemManager.Instance.GetMaxTierNum(targetItem.ItemData.ItemType);
            
            
            if (targetItem.ItemData.Id == ItemData.Id && targetItem.ItemData.Tier < maxTier)
            {
                Transform parentSlot = targetItem._currentSlot;
                var nextItemData = ItemManager.Instance.TryMerge(ItemData);
                ItemManager.Instance.CreateItem(nextItemData, parentSlot, ItemState.Active); 
                
                targetItem.gameObject.SetActive(false);
                gameObject.SetActive(false);
                Destroy(targetItem.gameObject);
                Destroy(gameObject);
            }
            else
            {
                if (targetItem.State == ItemState.Active)
                {
                    transform.SetParent(targetItem._currentSlot);
                    ItemManager.Instance.SwapItems(_currentSlot, targetItem);
                    _currentSlot = transform.parent;
                    _rectTransform.anchoredPosition = Vector2.zero;
                }
                else
                {
                    ReturnToSlot();
                }
            }
            
        }
        else
        {
            ReturnToSlot();
        }
        
        _canvasGroup.blocksRaycasts = true;
    }

    private void ReturnToSlot()
    {
        transform.SetParent(_currentSlot);
        _rectTransform.anchoredPosition = Vector2.zero;
    }
    
}
