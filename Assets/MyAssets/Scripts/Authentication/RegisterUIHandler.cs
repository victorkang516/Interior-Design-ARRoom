using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;

public class RegisterUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;
    

    private AuthManager authManager;

    //Register variables
    [Header("Register")]
    private InputField usernameRegisterField;
    private InputField emailRegisterField;
    private InputField passwordRegisterField;
    private Button registerButton;
    private Button loginPageButton;
    private WarningSignHandler warningSignHandler;
    private MessageBoxHandler messageBoxHandler;

    // Inputs Position
    Vector2 positionBesideUsername = new Vector2(500f, 200f);
    Vector2 positionBesideEmail = new Vector2(500f, 100f);
    Vector2 positionBesidePassword = new Vector2(500f, 0f);


    void Start()
    {
        canvasManager = GameObject.Find("/Canvas").GetComponent<CanvasManager>();

        authManager = GameObject.Find("AuthManager").GetComponent<AuthManager>();

        usernameRegisterField = GameObject.Find("UsernameRegisterField").GetComponent<InputField>();
        emailRegisterField = GameObject.Find("EmailRegisterField").GetComponent<InputField>();
        passwordRegisterField = GameObject.Find("PasswordRegisterField").GetComponent<InputField>();

        registerButton = GameObject.Find("RegisterButton").GetComponent<Button>();
        registerButton.onClick.AddListener(RegisterButton);

        loginPageButton = GameObject.Find("LoginPageButton").GetComponent<Button>();
        loginPageButton.onClick.AddListener(SwitchToRegisterPage);

        warningSignHandler = GameObject.Find("WarningSign").GetComponent<WarningSignHandler>();

        messageBoxHandler = GameObject.Find("MessagePage").GetComponent<MessageBoxHandler>();
        messageBoxHandler.gameObject.SetActive(false);
    }


    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            warningSignHandler.Show("Missing Username", positionBesideUsername);
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = authManager.auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            warningSignHandler.Hide();

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
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
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        position = positionBesidePassword;
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        position = positionBesideEmail;
                        break;
                }
                warningSignHandler.Show(message, position);
            }
            else
            {
                //User has now been created
                //Now get the result
                authManager.user = RegisterTask.Result;

                if (authManager.user != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = authManager.user.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningSignHandler.Show("Username Set Failed", positionBesideUsername);
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        usernameRegisterField.text = "";
                        emailRegisterField.text = "";
                        passwordRegisterField.text = "";
                        messageBoxHandler.DisplayMessage("Account has been created. You can now log in.");
                        
                    }
                }
            }
        }
    }

    void SwitchToRegisterPage()
    {
        warningSignHandler.Hide();
        canvasManager.SwitchCanvas(CanvasType.Login);
    }
}
