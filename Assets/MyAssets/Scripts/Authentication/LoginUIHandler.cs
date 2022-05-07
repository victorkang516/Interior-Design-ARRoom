using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class LoginUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;

    private AuthManager authManager;

    //Login variables
    [Header("Login")]
    private InputField emailLoginField;
    private InputField passwordLoginField;
    private Button loginButton;
    Button registerPageButton;
    WarningSignHandler warningSignHandler;

    // Store the PlayerPref Key to avoid typos
    const string userEmailPrefKey = "UserEmail";
    const string userPasswordPrefKey = "UserPassword";

    // Inputs Position
    Vector2 positionBesideEmail = new Vector2(500f, 170f);
    Vector2 positionBesidePassword = new Vector2(500f, 50f);

    void Start()
    {
        canvasManager = GameObject.Find("/Canvas").GetComponent<CanvasManager>();

        authManager = GameObject.Find("AuthManager").GetComponent<AuthManager>();

        emailLoginField = GameObject.Find("EmailLoginField").GetComponent<InputField>();
        passwordLoginField = GameObject.Find("PasswordLoginField").GetComponent<InputField>();

        loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        loginButton.onClick.AddListener(LoginButton);

        registerPageButton = GameObject.Find("RegisterPageButton").GetComponent<Button>();
        registerPageButton.onClick.AddListener(SwitchToRegisterPage);

        warningSignHandler = GameObject.Find("WarningSign").GetComponent<WarningSignHandler>();

        InitializeEmailLoginField();
        InitializePasswordLoginField();
    }

    void InitializeEmailLoginField ()
    {
        string savedEmail = string.Empty;
        if (emailLoginField != null)
        {
            if (PlayerPrefs.HasKey(userEmailPrefKey))
            {
                savedEmail = PlayerPrefs.GetString(userEmailPrefKey);
                emailLoginField.text = savedEmail;
            }
        }
    }

    void InitializePasswordLoginField()
    {
        string savedPassword = string.Empty;
        if (passwordLoginField != null)
        {
            if (PlayerPrefs.HasKey(userPasswordPrefKey))
            {
                savedPassword = PlayerPrefs.GetString(userPasswordPrefKey);
                passwordLoginField.text = savedPassword;
            }
        }
    }

    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = authManager.auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        warningSignHandler.Hide();

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            Vector2 position = positionBesideEmail;

            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    position = positionBesideEmail;
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    position = positionBesidePassword;
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    position = positionBesidePassword;
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    position = positionBesideEmail;
                    break;
                case AuthError.UserNotFound:
                    message = "Account not exist";
                    position = positionBesideEmail;
                    break;
            }
            warningSignHandler.Show(message, position);
        }
        else
        {
            authManager.user = LoginTask.Result;

            LoginSucceed();
        }
    }

    private void LoginSucceed ()
    {
        SaveUserInfoIntoPlayerPref(authManager.user);
        ChangeToMainMenuScene();
    }

    private void SaveUserInfoIntoPlayerPref(FirebaseUser User)
    {
        PlayerPrefs.SetString(userEmailPrefKey, User.Email);
    }

    void ChangeToMainMenuScene()
    {
        SceneManager.LoadScene(1);
    }

    void SwitchToRegisterPage ()
    {
        warningSignHandler.Hide();
        canvasManager.SwitchCanvas(CanvasType.Register);
    }
}
