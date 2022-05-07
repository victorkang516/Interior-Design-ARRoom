using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxHandler : MonoBehaviour
{
    CanvasManager canvasManager;
    Text messageText;
    Button closeButton;

    void Start()
    {
        canvasManager = GameObject.Find("/Canvas").GetComponent<CanvasManager>();
        closeButton = transform.Find("MessageBox/CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OK);
    }

    public void DisplayMessage(string message)
    {
        gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = message;
    }

    void OK()
    {
        gameObject.SetActive(false);
        canvasManager.SwitchCanvas(CanvasType.Login);
    }
}
