using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;
    Button createARRoomButton;
    Button joinARRoomButton;

    private void Start()
    {
        canvasManager = gameObject.GetComponentInParent<CanvasManager>();

        createARRoomButton = transform.Find("CreateARRoomButton").GetComponent<Button>();
        createARRoomButton.onClick.AddListener(GoToCreateARRoomPage);

        joinARRoomButton = transform.Find("JoinARRoomButton").GetComponent<Button>();
        joinARRoomButton.onClick.AddListener(GoToJoinARRoomPage);
    }

    void GoToCreateARRoomPage()
    {
        canvasManager.SwitchCanvas(CanvasType.CreateARRoom);
    }

    void GoToJoinARRoomPage()
    {
        canvasManager.SwitchCanvas(CanvasType.JoinARRoom);
    }
}
