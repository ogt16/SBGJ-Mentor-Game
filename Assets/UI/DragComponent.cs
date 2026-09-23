using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragComponent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int InSlot = -1;
    private bool InDrag = false;
    public CultController ControllerReference;

    public void OnBeginDrag(PointerEventData eventData)
    {
        ChangeImageVisual(0.5f, false);
        InDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ChangeImageVisual(1f, true);
        InDrag = false;

        // if the card is not in a slot then return it to the carousel. --> if it WAS in a slot remove the slot data from the ritual

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("On Hover");
        // update the information pane to the character data of this follower card
        ControllerReference.UpdateInformationPane(transform.gameObject.GetComponent<CharacterData>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!InDrag)
        {
            Debug.Log("Hover end");
        }
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