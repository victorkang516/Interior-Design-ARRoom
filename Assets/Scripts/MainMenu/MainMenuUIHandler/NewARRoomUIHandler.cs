using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewARRoomUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;
    Button backButton;

    private void Start()
    {
        canvasManager = gameObject.GetComponentInParent<CanvasManager>();

        backButton = gameObject.GetComponentsInChildren<Button>()[1];
        backButton.onClick.AddListener(BackToCreateARRoom);
    }

    void BackToCreateARRoom()
    {
        canvasManager.SwitchCanvas(CanvasType.CreateARRoom);
    }
}
