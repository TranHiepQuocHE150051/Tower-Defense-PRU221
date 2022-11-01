using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AnimatedButton : UIBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    public bool interactable = true;

    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
    [SerializeField]
    private bool isPopupOpen;
    [SerializeField]
    private Transform tf;

    private bool isCLicked;
    public bool isBtn;

    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || !interactable)
            return;

        if (tf != null)
        {
            tf.localScale *= 0.9f;
        }
        isCLicked = true;
    }

    private void Press()
    {
        if (!IsActive())
            return;
        OnClickAction();
    }

    private void OnClickAction()
    {
        AudioController.instance.PlaySound("button");
        m_OnClick.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable)
        {
            return;
        }
        if (isCLicked)
        {
            Press();
        }

        if (tf != null && isCLicked)
        {
            tf.localScale /= 0.9f;
        }
        isCLicked = false;
    }
}