using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class LoginUIHandler : MonoBehaviour
{

    NetworkManager networkManager;

    InputField emailInputField;
    InputField passwordInputField;
    Button loginButton;

    GameObject warningSign;

    // Store the PlayerPref Key to avoid typos
    const string playerNamePrefKey = "PlayerName";

    void Start()
    {
        string defaultName = string.Empty;
        emailInputField = GameObject.Find("/Canvas/LoginPage/LoginPanel/EmailInputField").GetComponent<InputField>();
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

        networkManager = GameObject.Find("/NetworkManager").GetComponent<NetworkManager>();

        loginButton = GameObject.Find("/Canvas/LoginPage/LoginPanel/LoginButton").GetComponent<Button>();
        loginButton.onClick.AddListener(Login);

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
            ConnectToPhotonNetworkWithNetworkManager();
    }

    void ConnectToPhotonNetworkWithNetworkManager ()
    {
        networkManager.Connect();
    }
}
