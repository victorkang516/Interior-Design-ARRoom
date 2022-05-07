using UnityEngine;
using UnityEngine.UI;

public class WelcomeUIHandler : MonoBehaviour
{
    CanvasGroup canvasGroup;
    CanvasGroup infoGroup;
    Image welcomeImage;
    Button getStartedButton;

    bool isPlayingInfoGroupAnimation = false;
    bool isPlayingGetStartedButtonAnimation = false;

    bool hide = false;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        infoGroup = GameObject.Find("InfoGroup").GetComponent<CanvasGroup>();
        infoGroup.alpha = 0;
        welcomeImage = GameObject.Find("WelcomeImage").GetComponent<Image>();
        welcomeImage.GetComponent<CanvasGroup>().alpha = 1;

        getStartedButton = GameObject.Find("GetStartedButton").GetComponent<Button>();
        getStartedButton.onClick.AddListener(GetStarted);
        getStartedButton.GetComponent<CanvasGroup>().alpha = 0;

        StartAnimation();
    }

    private void StartAnimation ()
    {
        isPlayingInfoGroupAnimation = true;
    }

    private void Update()
    {

        // Second Animation
        if (isPlayingInfoGroupAnimation)
            infoGroup.alpha += 0.01f;

        if (infoGroup.alpha >= 1)
        {
            isPlayingInfoGroupAnimation = false;
            isPlayingGetStartedButtonAnimation = true;
        }
            

        // Third Animation
        if (isPlayingGetStartedButtonAnimation)
            getStartedButton.GetComponent<CanvasGroup>().alpha += 0.05f;

        if (getStartedButton.GetComponent<CanvasGroup>().alpha >= 1)
            isPlayingGetStartedButtonAnimation = false;


        if (hide == true)
        {
            canvasGroup.alpha -= 0.05f;
        }

        if (canvasGroup.alpha <= 0)
        {
            
            gameObject.SetActive(false);
        }
    }

    private void GetStarted ()
    {
        hide = true;
    }
}
