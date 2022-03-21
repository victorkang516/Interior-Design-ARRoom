using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARModificationManager : MonoBehaviour
{
    ObjectListHandler objectListHandler;
    GameObject objectListPanel;

    Button fullWallButton;
    Button halfWallButton;
    Button closeModificationGuidePanelButton;
    Image triggerWallButtonBg;
    Image moveGuidePanel;
    Image pinchGuidePanel;
    Image modificationGuidePanel;
    bool isFullWall = true;

    [HideInInspector] public UpperWall[] upperWallList;
    [HideInInspector] public MiddleWall[] middleWallList;

    GameObject currentSelectable;

    float yBoundary;

    void Start()
    {
        objectListHandler = GameObject.Find("/Canvas/ARModificationMode/ObjectListPanel/Scroll/Panel").GetComponent<ObjectListHandler>();
        objectListPanel = GameObject.Find("/Canvas/ARModificationMode/ObjectListPanel");
        objectListPanel.transform.localScale = new Vector3(0, 1, 0);

        moveGuidePanel = GameObject.Find("/Canvas/ARModificationMode/MoveGuidePanel").gameObject.GetComponent<Image>();
        moveGuidePanel.gameObject.transform.localScale = Vector2.zero;

        pinchGuidePanel = GameObject.Find("/Canvas/ARModificationMode/PinchGuidePanel").gameObject.GetComponent<Image>();
        pinchGuidePanel.gameObject.transform.localScale = Vector2.zero;

        fullWallButton = GameObject.Find("/Canvas/ARModificationMode/WallTriggerPanel/FullWallButton").gameObject.GetComponent<Button>();
        fullWallButton.onClick.AddListener(TriggerFullWall);

        halfWallButton = GameObject.Find("/Canvas/ARModificationMode/WallTriggerPanel/HalfWallButton").gameObject.GetComponent<Button>();
        halfWallButton.onClick.AddListener(TriggerHalfWall);

        modificationGuidePanel = GameObject.Find("/Canvas/ARModificationMode/ModificationGuidePanel").gameObject.GetComponent<Image>();
        modificationGuidePanel.gameObject.transform.localScale = Vector2.zero;

        closeModificationGuidePanelButton = GameObject.Find("/Canvas/ARModificationMode/ModificationGuidePanel/CloseButton").gameObject.GetComponent<Button>();
        closeModificationGuidePanelButton.onClick.AddListener(CloseModificationGuidePanel);

        triggerWallButtonBg = GameObject.Find("/Canvas/ARModificationMode/WallTriggerPanel/ButtonBg").gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        if (currentSelectable == null)
            return;

        FixObjectYPosition();
    }

    private void FixObjectYPosition()
    {
        if (currentSelectable.transform.position.y != yBoundary)
        {
            currentSelectable.transform.position = new Vector3(currentSelectable.transform.position.x, yBoundary, currentSelectable.transform.position.z);
        }
    }

    public void RestartUIFlow ()
    {
        moveGuidePanel.gameObject.transform.localScale = Vector2.zero;
        pinchGuidePanel.gameObject.transform.localScale = Vector2.zero;

        if (currentSelectable != null)
        {
            //MainManager.Instance.Debug2("ARModification: Deselect object");
            DeselectARObject();
        }
            
    }

    private void TriggerFullWall()
    {
        isFullWall = true;
        SearchAndTriggerAllWalls(isFullWall);

        LeanTween.moveLocalY(triggerWallButtonBg.gameObject, 50.0f, 0.25f).setEaseInOutQuart();
    }

    private void TriggerHalfWall()
    {
        isFullWall = false;
        SearchAndTriggerAllWalls(isFullWall);

        LeanTween.moveLocalY(triggerWallButtonBg.gameObject, -50.0f, 0.25f).setEaseInOutQuart();
    }

    private void SearchAndTriggerAllWalls (bool isFullWall)
    {
        foreach (UpperWall upperWall in upperWallList)
        {
            upperWall.gameObject.SetActive(isFullWall);
            Lean.Touch.LeanSelectableByFinger parentWall = upperWall.GetComponent<Lean.Touch.LeanSelectableByFinger>();
            if (parentWall != null)
                parentWall.enabled = isFullWall;
        }

        foreach (MiddleWall middleWall in middleWallList)
        {
            middleWall.gameObject.SetActive(isFullWall);
        }

    }

    public void SelectARObject (Lean.Common.LeanSelectable leanSelectable)
    {
        currentSelectable = leanSelectable.gameObject;
        yBoundary = currentSelectable.transform.position.y;

        TriggerOutline(true);
        ShowObjectListPanel();
        ShowGuidePanels();
    }

    public void DeselectARObject()
    {
        HideGuidePanels();
        HideObjectListPanel();
        TriggerOutline(false);
        
        currentSelectable = null;
    }

    private void TriggerOutline (bool isEnabled)
    {
        if (currentSelectable.CompareTag("Paint") || currentSelectable.CompareTag("Floor"))
        {
            GameObject room = currentSelectable.transform.parent.gameObject;
            Outline[] childrenOutline = room.GetComponentsInChildren<Outline>();
            foreach (Outline outline in childrenOutline)
            {
                outline.enabled = isEnabled;
            }
        }
        else
        {
            currentSelectable.GetComponent<Outline>().enabled = isEnabled;
        }
    }

    private void ShowObjectListPanel ()
    {
        if (!currentSelectable.CompareTag("Toilet") && !currentSelectable.CompareTag("Shower"))
        {
            LeanTween.scale(objectListPanel.gameObject, new Vector2(1, 1), 0.25f).setEaseInOutQuart();
            objectListHandler.CreateObjectList(currentSelectable);
        }
    }

    private void HideObjectListPanel()
    {
        if (!currentSelectable.CompareTag("Toilet") && !currentSelectable.CompareTag("Shower"))
        {
            LeanTween.scale(objectListPanel.gameObject, new Vector2(0, 1), 0.25f).setEaseInOutQuart();
            objectListHandler.EmptyObjectList();
        }
    }

    private void ShowGuidePanels ()
    {
        if (!currentSelectable.CompareTag("Paint") && !currentSelectable.CompareTag("Floor"))
        {
            LeanTween.scale(moveGuidePanel.gameObject, new Vector2(1, 1), 0.5f).setEaseOutBack();
            LeanTween.scale(pinchGuidePanel.gameObject, new Vector2(1, 1), 0.5f).setEaseOutBack();
        }
    }

    private void HideGuidePanels()
    {
        if (!currentSelectable.CompareTag("Paint") && !currentSelectable.CompareTag("Floor"))
        {
            LeanTween.scale(moveGuidePanel.gameObject, new Vector2(0, 0), 0.5f).setEaseInBack();
            LeanTween.scale(pinchGuidePanel.gameObject, new Vector2(0, 0), 0.5f).setEaseInBack();
        }
    }

    private void CloseModificationGuidePanel()
    {
        LeanTween.scale(modificationGuidePanel.gameObject, new Vector2(0, 0), 0.1f);
    }
}
