using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverHandler : MarkBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Action<PointerEventData> OnPointerEnterAction;
    public Action<PointerEventData> OnPointerExitAction;
    public Action<PointerEventData> OnPointerUpAction;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterAction?.Invoke(eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitAction?.Invoke(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpAction?.Invoke(eventData);
    }
}
