using System; 
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public event Action OnJoystickDown;
    public event Action OnJoystickUp;


    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnJoystickDown?.Invoke();
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        OnJoystickUp?.Invoke();
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    private void OnDestroy()
    {
        OnJoystickDown = null;
        OnJoystickUp = null;
    }
}