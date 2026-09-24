using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RitualSlotComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool IsLocked;
    private Upgrade RitualReference;
    private CultController Controller;
    
    [SerializeField] private GameObject LockedSlot;
    [SerializeField] private GameObject RitualIcon;

    public void InitSlot(bool _locked, Upgrade _upgrade, CultController _controller)
    {
        IsLocked         = _locked;
        RitualReference  = _upgrade;
        Controller       = _controller;

        DrawSlot();
    }

    private void DrawSlot()
    {
        LockedSlot.SetActive(IsLocked); 
        RitualIcon.GetComponent<Image>().sprite = RitualReference.Icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Controller.UpdateRitualHoverDisplay(RitualReference);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Controller.UpdateRitualHoverDisplay();
    }
}
