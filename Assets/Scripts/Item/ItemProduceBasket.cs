using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemProduceBasket: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Transform currentSlot;
    
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private BasketEnergy _basketEnergy;
    
    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        currentSlot = transform.parent;
    }

    public void Init(BasketEnergy basketEnergy)
    {
        _basketEnergy = basketEnergy;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Slot parentSlot = GetComponentInParent<Slot>();
        
        if(ItemManager.Instance.hasEmptySlot(parentSlot)&&_basketEnergy.CanUseEnergy())
        {
           // _basketEnergy.UseEnergy();
           
            if(GameManager.Instance != null)
               GameManager.Instance.UseStamina(5);
           
            ItemManager.Instance.CreateRandomItem(parentSlot);
        }
        else
        {
            Debug.Log("슬롯 없음 or 에너지 부족");
        }
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
            currentSlot = targetSlot.transform;
        }
        else if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Item>())
        {
            Item targetItem = eventData.pointerEnter.GetComponent<Item>();
           
            if (targetItem.State == ItemState.Active)
            {
                transform.SetParent(targetItem.currentSlot);
                ItemManager.Instance.SwapItems(currentSlot, targetItem);
                currentSlot = transform.parent;
                _rectTransform.anchoredPosition = Vector2.zero;
            }
            else
            {
                ReturnToSlot();
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
        transform.SetParent(currentSlot);
        _rectTransform.anchoredPosition = Vector2.zero;
    }
    
}
