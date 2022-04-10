using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinARRoomUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;
    NetworkManager networkManager;

    Button backButton;

    InputField roomNameInputField;
    Button joinARRoomButton;
    GameObject warningSign;

    private void Start()
    {
        canvasManager = gameObject.GetComponentInParent<CanvasManager>();
        networkManager = GameObject.Find("/NetworkManager").GetComponent<NetworkManager>();

        backButton = transform.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(BackToMainMenu);

        joinARRoomButton = transform.Find("RoomNamePanel/JoinARRoomButton").GetComponent<Button>();
        joinARRoomButton.onClick.AddListener(JoinARRoom);

        roomNameInputField = transform.Find("RoomNamePanel/RoomNameInputField").GetComponent<InputField>();

        warningSign = transform.Find("WarningSign").gameObject;
    }

    void JoinARRoom ()
    {
        if (roomNameInputField.text.Length > 0)
            networkManager.JoinARRoom(roomNameInputField.text);
        else
            warningSign.GetComponent<WarningSignHandler>().Show();
    }

    void BackToMainMenu()
    {
        warningSign.GetComponent<WarningSignHandler>().Hide();
        canvasManager.SwitchCanvas(CanvasType.Home);
    }
}
