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

        newARRoomButton = transform.Find("NewARRoomButton").GetComponent<Button>();
        newARRoomButton.onClick.AddListener(GoToCreateARRoomPage);

        loadARRoomButton = transform.Find("LoadARRoomButton").GetComponent<Button>();
        loadARRoomButton.onClick.AddListener(GoToJoinARRoomPage);

        backButton = transform.Find("BackButton").GetComponent<Button>();
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
        canvasManager.SwitchCanvas(CanvasType.Home);
    }
}
