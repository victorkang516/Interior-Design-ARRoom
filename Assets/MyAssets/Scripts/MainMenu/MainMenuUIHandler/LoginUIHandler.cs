using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class LoginUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;

    InputField emailInputField;
    InputField passwordInputField;
    Button loginButton;
    Button registerPageButton;

    GameObject warningSign;

    // Store the PlayerPref Key to avoid typos
    const string playerNamePrefKey = "PlayerName";

    void Start()
    {

        string defaultName = string.Empty;
        emailInputField = GameObject.Find("EmailInputField").GetComponent<InputField>();
        if (emailInputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                emailInputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
        emailInputField.onValueChanged.AddListener((name) => { SetPlayerName(name); });


        canvasManager = GameObject.Find("/Canvas").GetComponent<CanvasManager>();

        loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        loginButton.onClick.AddListener(Login);

        registerPageButton = GameObject.Find("RegisterPageButton").GetComponent<Button>();
        registerPageButton.onClick.AddListener(SwitchToRegisterPage);

        warningSign = transform.Find("WarningSign").gameObject;
    }

    public void SetPlayerName(string value)
    {
        // #Important
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            warningSign.GetComponent<WarningSignHandler>().Show();
            return;
        }
        warningSign.GetComponent<WarningSignHandler>().Hide();
        PhotonNetwork.NickName = value;


        PlayerPrefs.SetString(playerNamePrefKey, value);
    }

    void Login ()
    {
        if (PlayerPrefs.GetString(playerNamePrefKey).Length > 0)
            ChangeToMainMenuScene();
    }

    void ChangeToMainMenuScene ()
    {
        SceneManager.LoadScene(1);
    }

    void SwitchToRegisterPage ()
    {
        canvasManager.SwitchCanvas(CanvasType.Register);
    }
}
