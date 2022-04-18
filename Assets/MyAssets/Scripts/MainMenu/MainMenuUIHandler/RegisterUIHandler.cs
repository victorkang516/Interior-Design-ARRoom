using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;
    Button loginPageButton;

    void Start()
    {
        canvasManager = GameObject.Find("/Canvas").GetComponent<CanvasManager>();

        loginPageButton = GameObject.Find("LoginPageButton").GetComponent<Button>();
        loginPageButton.onClick.AddListener(SwitchToRegisterPage);
    }

    void SwitchToRegisterPage()
    {
        canvasManager.SwitchCanvas(CanvasType.Login);
    }
}
