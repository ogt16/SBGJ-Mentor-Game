using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragComponent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int InSlot = -1;
    public Vector2 PosCache = new Vector2(0,0);
    private bool InDrag = false;
    public CultController ControllerReference;
    public Vector2 CarouselPosition;


    public void OnBeginDrag(PointerEventData eventData)
    {
        ChangeImageVisual(0.5f, false);
        InDrag = true;

        FMODUnity.RuntimeManager.PlayOneShot("event:/sounds/pick up");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ChangeImageVisual(1f, true);
        InDrag = false;

        FMODUnity.RuntimeManager.PlayOneShot("event:/sounds/drop");


        // if the card is not in a slot then return it to the carousel. --> if it WAS in a slot remove the slot data from the ritual
        // check if the card overlaps with the slot
        if(PosCache != null)
        {
            BoxCollider2D _collider = this.transform.gameObject.GetComponent<BoxCollider2D>();
            if(!_collider.OverlapPoint(PosCache))
            {
                if(InSlot != -1){ControllerReference.RemoveFollowerFromRitual(InSlot);}
                InSlot = - 1;
                PosCache = new Vector2(0,0);

                // NOT IN A SLOT
                transform.localPosition = CarouselPosition;            
                ControllerReference.UpdateInformationPane();
            }
        }
        else
        {
            // NOT IN A SLOT
            transform.localPosition = CarouselPosition;
            ControllerReference.UpdateInformationPane();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // update the information pane to the character data of this follower card
        ControllerReference.UpdateInformationPane(transform.gameObject.GetComponent<Character>().data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!InDrag)
        {
            ControllerReference.UpdateInformationPane();
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