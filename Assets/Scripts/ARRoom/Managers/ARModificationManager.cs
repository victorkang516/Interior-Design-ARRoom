using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARModificationManager : MonoBehaviour
{
    ObjectListHandler objectListHandler;
    GameObject objectListPanel;

    Button lowerWallButton;
    bool hideWall = false;

    [HideInInspector] public UpperWall[] upperWallList;
    [HideInInspector] public MiddleWall[] middleWallList;

    GameObject currentSelectable;

    float yBoundary;

    void Start()
    {
        objectListHandler = GameObject.Find("/Canvas/ARModificationMode/ObjectListPanel/Scroll/Panel").GetComponent<ObjectListHandler>();
        objectListPanel = GameObject.Find("/Canvas/ARModificationMode/ObjectListPanel");
        objectListPanel.SetActive(false);

        lowerWallButton = GameObject.Find("/Canvas/ARModificationMode/HideWallButton").gameObject.GetComponent<Button>();
        lowerWallButton.onClick.AddListener(HideTheWall);
        lowerWallButton.gameObject.SetActive(true);
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
        lowerWallButton.gameObject.SetActive(true);
        if (currentSelectable != null)
            DeselectARObject();
    }

    private void HideTheWall()
    {
        
        hideWall = !hideWall;

        foreach (UpperWall upperWall in upperWallList)
        {
            upperWall.gameObject.SetActive(!hideWall);
        }

        foreach (MiddleWall middleWall in middleWallList)
        {
            middleWall.gameObject.SetActive(!hideWall);
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
