using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class MainMenuUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;
    Button createARRoomButton;
    Button joinARRoomButton;

    // Setting
    Button settingButton;
    Button cancelButton;
    GameObject settingPanel;
    InputField updateUsernameField;
    Button updateUsernameButton;
    Button logoutButton;
    Button howToUseButton;
    GuideUIHandler guideUIHandler;

    ExceptionMessageBoxHandler exceptionMessageBoxHandler;

    private void Start()
    {
        canvasManager = gameObject.GetComponentInParent<CanvasManager>();

        createARRoomButton = transform.Find("CreateARRoomButton").GetComponent<Button>();
        createARRoomButton.onClick.AddListener(GoToCreateARRoomPage);

        joinARRoomButton = transform.Find("JoinARRoomButton").GetComponent<Button>();
        joinARRoomButton.onClick.AddListener(GoToJoinARRoomPage);

        settingButton = GameObject.Find("SettingButton").GetComponent<Button>();
        settingButton.onClick.AddListener(ShowSettingPanel);

        cancelButton = GameObject.Find("CancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(HideSettingPanel);

        updateUsernameField = GameObject.Find("UpdateUsernameField").GetComponent<InputField>();

        updateUsernameButton = GameObject.Find("UpdateUsernameButton").GetComponent<Button>();
        updateUsernameButton.onClick.AddListener(UpdateUsername);

        logoutButton = GameObject.Find("LogoutButton").GetComponent<Button>();
        logoutButton.onClick.AddListener(Logout);

        settingPanel = GameObject.Find("SettingPanel");

        howToUseButton = GameObject.Find("HowToUseButton").GetComponent<Button>();
        howToUseButton.onClick.AddListener(ShowGuidePage);

        guideUIHandler = GameObject.Find("GuidePage").GetComponent<GuideUIHandler>();

        exceptionMessageBoxHandler = GameObject.Find("/SecondCanvas/MessagePage").GetComponent<ExceptionMessageBoxHandler>();

        LoadUsername();
        HideSettingPanel();
    }

    private void LoadUsername ()
    {
        Firebase.Auth.FirebaseUser user = AuthManager.Instance.auth.CurrentUser;
        if (user != null)
        {
            string name = user.DisplayName;
            updateUsernameField.text = name;
        }
    }

    private void ShowSettingPanel ()
    {
        settingPanel.SetActive(true);
    }

    private void HideSettingPanel()
    {
        settingPanel.SetActive(false);
    }


    void GoToCreateARRoomPage()
    {
        canvasManager.SwitchCanvas(CanvasType.CreateARRoom);
    }

    void GoToJoinARRoomPage()
    {
        canvasManager.SwitchCanvas(CanvasType.JoinARRoom);
    }

    private void UpdateUsername ()
    {
        Firebase.Auth.FirebaseUser user = AuthManager.Instance.auth.CurrentUser;

        if (updateUsernameField.text == "")
            return;

        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = updateUsernameField.text,
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                
                Debug.Log("User profile updated successfully.");
            });

            exceptionMessageBoxHandler.DisplayMessage("Username updated successfully.");
        }
    }

    private void Logout ()
    {
        PhotonNetwork.Disconnect();
        AuthManager.Instance.auth.SignOut();
        SceneManager.LoadScene(0);
    }

    private void ShowGuidePage()
    {
        guideUIHandler.Show();
    }
}

