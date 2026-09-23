using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragComponent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        ChangeImageVisual(0.5f, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ChangeImageVisual(1f, true);
    }

    private void ChangeImageVisual(float alpha, bool raycast)
    {
        Image image = GetComponent<Image>();
        Color imageColour = image.color;

        if(imageColour != null)
        {
            imageColour.a        = alpha;
            image.color          = imageColour;
            image.raycastTarget  = raycast;
        }
    }
}