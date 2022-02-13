using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateARRoomUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;
    Button newARRoomButton;
    Button loadARRoomButton;
    Button backButton;

    private void Start()
    {
        canvasManager = gameObject.GetComponentInParent<CanvasManager>();

        newARRoomButton = gameObject.GetComponentsInChildren<Button>()[0];
        newARRoomButton.onClick.AddListener(GoToCreateARRoomPage);

        loadARRoomButton = gameObject.GetComponentsInChildren<Button>()[1];
        loadARRoomButton.onClick.AddListener(GoToJoinARRoomPage);

        backButton = gameObject.GetComponentsInChildren<Button>()[2];
        backButton.onClick.AddListener(BackToMainMenu);
    }

    void GoToCreateARRoomPage()
    {
        canvasManager.SwitchCanvas(CanvasType.NewARRoom);
    }

    void GoToJoinARRoomPage()
    {
        canvasManager.SwitchCanvas(CanvasType.LoadARRoom);
    }

    void BackToMainMenu()
    {
        canvasManager.SwitchCanvas(CanvasType.MainMenu);
    }
}
