using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;

    Button skipGuideButton;
    Button nextGuideButton;
    Button previousGuideButton;

    Button okayGuideButton;

    public GuidePage[] guidePages;

    int guidePageIndex = 0;

    bool isPlayingAnimation = false;

    void Start()
    {
        canvasManager = GameObject.Find("/Canvas").GetComponent<CanvasManager>();

        skipGuideButton = transform.Find("SkipGuideButton").GetComponent<Button>();
        skipGuideButton.onClick.AddListener(Hide);

        nextGuideButton = transform.Find("NextGuideButton").GetComponent<Button>();
        nextGuideButton.onClick.AddListener(NextGuide);

        previousGuideButton = transform.Find("PreviousGuideButton").GetComponent<Button>();
        previousGuideButton.onClick.AddListener(PreviousGuide);
        previousGuideButton.gameObject.SetActive(false);

        okayGuideButton = transform.Find("OkayGuideButton").GetComponent<Button>();
        okayGuideButton.onClick.AddListener(Hide);
        okayGuideButton.gameObject.SetActive(false);

        if (MainManager.Instance.firstTime == true)
        {
            Show();
            MainManager.Instance.firstTime = false;
        }
        else
            Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        guidePageIndex = 0;
        previousGuideButton.gameObject.SetActive(false);
        okayGuideButton.gameObject.SetActive(false);
        ControlGuidePages();
    }

    private void Hide ()
    {
        gameObject.SetActive(false);
    }

    private void ControlGuidePages ()
    {
        foreach (GuidePage guidePage in guidePages)
        {
            guidePage.GetComponent<CanvasGroup>().alpha = 0;
        }

        isPlayingAnimation = true;
    }

    private void Update()
    {
        if (isPlayingAnimation)
        {
            guidePages[guidePageIndex].GetComponent<CanvasGroup>().alpha += 0.05f;
        }

        if (guidePages[guidePageIndex].GetComponent<CanvasGroup>().alpha >= 1f)
        {
            isPlayingAnimation = false;
        }
    }

    private void NextGuide ()
    {
        guidePageIndex += 1;

        if (guidePageIndex >= guidePages.Length - 1)
        {
            okayGuideButton.gameObject.SetActive(true);
            skipGuideButton.gameObject.SetActive(false);
            nextGuideButton.gameObject.SetActive(false);
        }    

        previousGuideButton.gameObject.SetActive(true);

        ControlGuidePages();
    }

    private void PreviousGuide()
    {
        guidePageIndex -= 1;

        if (guidePageIndex <= 0)
            previousGuideButton.gameObject.SetActive(false);

        okayGuideButton.gameObject.SetActive(false);
        skipGuideButton.gameObject.SetActive(true);
        nextGuideButton.gameObject.SetActive(true);

        ControlGuidePages();
    }

}
