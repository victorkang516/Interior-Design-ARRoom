using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARModificationManager : MonoBehaviour
{
    ObjectListHandler objectListHandler;
    GameObject objectListPanel;

    GameObject wallTriggerPanel;
    Button fullWallButton;
    Button halfWallButton;
    Image triggerWallButtonBg;
    bool isFullWall = true;

    [HideInInspector] public UpperWall[] upperWallList;
    [HideInInspector] public MiddleWall[] middleWallList;

    GameObject currentSelectable;

    float yBoundary;

    void Start()
    {
        objectListHandler = GameObject.Find("/Canvas/ARModificationMode/ObjectListPanel/Scroll/Panel").GetComponent<ObjectListHandler>();
        objectListPanel = GameObject.Find("/Canvas/ARModificationMode/ObjectListPanel");
        objectListPanel.SetActive(false);

        wallTriggerPanel = GameObject.Find("/Canvas/ARModificationMode/WallTriggerPanel").gameObject.GetComponent<GameObject>();

        fullWallButton = GameObject.Find("/Canvas/ARModificationMode/WallTriggerPanel/FullWallButton").gameObject.GetComponent<Button>();
        fullWallButton.onClick.AddListener(TriggerFullWall);

        halfWallButton = GameObject.Find("/Canvas/ARModificationMode/WallTriggerPanel/HalfWallButton").gameObject.GetComponent<Button>();
        halfWallButton.onClick.AddListener(TriggerHalfWall);

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
        //MainManager.Instance.Debug1("ARModification: Current selectable is " + currentSelectable);
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
        ShowObjectListPanel(true);
    }

    public void DeselectARObject()
    {
        ShowObjectListPanel(false);
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

    private void ShowObjectListPanel (bool isEnabled)
    {
        if (!currentSelectable.CompareTag("Toilet") && !currentSelectable.CompareTag("Shower"))
        {
            objectListPanel.SetActive(isEnabled);
            if (isEnabled)
                objectListHandler.CreateObjectList(currentSelectable);
            else
                objectListHandler.EmptyObjectList();
        }

        return;
    }
}
