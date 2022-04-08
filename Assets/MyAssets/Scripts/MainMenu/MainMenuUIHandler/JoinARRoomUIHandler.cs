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
    GameObject errorMessageBox;

    private void Start()
    {
        canvasManager = gameObject.GetComponentInParent<CanvasManager>();
        networkManager = GameObject.Find("/NetworkManager").GetComponent<NetworkManager>();

        backButton = transform.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(BackToMainMenu);

        joinARRoomButton = transform.Find("RoomNamePanel/JoinARRoomButton").GetComponent<Button>();
        joinARRoomButton.onClick.AddListener(JoinARRoom);

        roomNameInputField = transform.Find("RoomNamePanel/RoomNameInputField").GetComponent<InputField>();

        errorMessageBox = transform.Find("ErrorMessageBox").gameObject;
        errorMessageBox.transform.localScale = new Vector2(0, 1);
    }

    void JoinARRoom ()
    {
        if (roomNameInputField.text.Length > 0)
            networkManager.JoinARRoom(roomNameInputField.text);
        else
        {
            errorMessageBox.transform.localScale = new Vector2(0, 1);
            LeanTween.scale(errorMessageBox, new Vector2(1, 1), 0.1f).setEaseInSine();
        }
        
    }

    void BackToMainMenu()
    {
        errorMessageBox.transform.localScale = new Vector2(0, 1);
        canvasManager.SwitchCanvas(CanvasType.Home);
    }
}
