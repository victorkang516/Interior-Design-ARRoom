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

        createARRoomButton = gameObject.GetComponentsInChildren<Button>()[0];
        createARRoomButton.onClick.AddListener(GoToCreateARRoomPage);

        joinARRoomButton = gameObject.GetComponentsInChildren<Button>()[1];
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
