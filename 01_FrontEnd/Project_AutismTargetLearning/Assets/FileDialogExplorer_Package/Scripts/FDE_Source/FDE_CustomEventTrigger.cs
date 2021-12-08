using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Custom Event Trigger (not documented - for internal use only)
/// </summary>
public class FDE_CustomEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public delegate void EventHandler_();
    public EventHandler_ OnClick;
    public EventHandler_ OnEnter;
    public EventHandler_ OnExit;

    public void OnPointerClick(PointerEventData dat)
    {
        if (dat.button != PointerEventData.InputButton.Right)
            return;
        if (OnClick != null)
            OnClick.Invoke();
    }
    public void OnPointerEnter(PointerEventData dat)
    {
        if (OnEnter != null)
            OnEnter.Invoke();
    }
    public void OnPointerExit(PointerEventData dat)
    {
        if (OnExit != null)
            OnExit.Invoke();
    }
}
