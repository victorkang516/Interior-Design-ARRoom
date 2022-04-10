using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExceptionMessageBoxHandler : MonoBehaviour
{
    Text messageText;
    Button closeButton;
    bool onFirstTime = true;

    void Start()
    {
        messageText = transform.Find("MessageBox/MessageText").GetComponent<Text>();
        Debug.Log("MessageHandler: messageText");

        closeButton = transform.Find("MessageBox/CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(CloseMessagePage);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy && onFirstTime)
        {
            gameObject.SetActive(false);
            onFirstTime = false;
        }
            
    }

    public void DisplayMessage (string message)
    {
        gameObject.SetActive(true);
        messageText.text = message;
    }

    void CloseMessagePage ()
    {
        gameObject.SetActive(false);
    }
}
