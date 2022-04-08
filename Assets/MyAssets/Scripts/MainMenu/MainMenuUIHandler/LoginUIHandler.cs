using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIHandler : MonoBehaviour
{

    NetworkManager networkManager;

    Button loginButton;

    void Start()
    {
        loginButton = GameObject.Find("/Canvas/LoginPage/LoginPanel/LoginButton").GetComponent<Button>();
        loginButton.onClick.AddListener(Login);

        networkManager = GameObject.Find("/NetworkManager").GetComponent<NetworkManager>();
    }

    void Login ()
    {
        ConnectToPhotonNetworkWithNetworkManager();
    }

    void ConnectToPhotonNetworkWithNetworkManager ()
    {
        networkManager.Connect();
    }
}
