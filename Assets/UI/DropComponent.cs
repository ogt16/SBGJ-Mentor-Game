using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropComponent : MonoBehaviour, IDropHandler
{
    public GameObject SceneManager;
    public int SlotID;

    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.transform.position = transform.position;
        SceneManager.GetComponent<CultController>().AddFollowerToRitual(eventData.pointerDrag, SlotID);


        if(eventData.pointerDrag.GetComponent<DragComponent>().InSlot != -1 && eventData.pointerDrag.GetComponent<DragComponent>().InSlot != SlotID)
        {
            // Debug.Log("Slot to slot race condition");
            SceneManager.GetComponent<CultController>().RemoveFollowerFromRitual(eventData.pointerDrag.GetComponent<DragComponent>().InSlot);
        }
        eventData.pointerDrag.GetComponent<DragComponent>().InSlot       = SlotID;
        eventData.pointerDrag.GetComponent<DragComponent>().PosCache     = transform.position;
    }

    public void RemoveFollowerFromSlot()
    {
        SceneManager.GetComponent<CultController>().RemoveFollowerFromRitual(SlotID);
    }
}
