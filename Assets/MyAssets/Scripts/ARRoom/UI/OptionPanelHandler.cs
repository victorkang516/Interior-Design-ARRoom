using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelHandler : MonoBehaviour
{
    Button closeButton;

    private void Start()
    {
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        LeanTween.scale(gameObject, Vector2.one, 0.4f).setEaseOutBack();
    }

    public void Hide()
    {
        LeanTween.scale(gameObject, Vector2.zero, 0.4f).setEaseInBack();
    }

    public void ActionDone()
    {
        LeanTween.scale(gameObject, Vector2.zero, 1.0f).setEaseInBack();
    }
}
