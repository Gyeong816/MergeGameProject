using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData ItemData;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private Transform _originalParent;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = transform.parent;
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
        else
        {
            transform.SetParent(_originalParent);
            _rectTransform.anchoredPosition = Vector2.zero;
        }
        
        _canvasGroup.blocksRaycasts = true;
    }
}
