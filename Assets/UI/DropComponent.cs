using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropComponent : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.transform.position = transform.position;
    }
}
