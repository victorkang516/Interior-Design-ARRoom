using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARModificationManager : MonoBehaviour
{
    [HideInInspector] public GameObject firstFloor;

    ObjectListHandler objectListHandler;
    GameObject objectListPanel;

    Button firstFloorButton;
    Button groundFloorButton;
    Button fullWallButton;
    Button halfWallButton;

    Image triggerWallButtonBg;
    Image triggerFloorButtonBg;

    Button closeModificationGuidePanelButton;
    
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

        firstFloorButton = GameObject.Find("/Canvas/ARModificationMode/FloorTriggerPanel/FirstFloorButton").gameObject.GetComponent<Button>();
        firstFloorButton.onClick.AddListener(ViewFirstFloor);

        groundFloorButton = GameObject.Find("/Canvas/ARModificationMode/FloorTriggerPanel/GroundFloorButton").gameObject.GetComponent<Button>();
        groundFloorButton.onClick.AddListener(ViewGroundFloor);

        fullWallButton = GameObject.Find("/Canvas/ARModificationMode/WallTriggerPanel/FullWallButton").gameObject.GetComponent<Button>();
        fullWallButton.onClick.AddListener(TriggerFullWall);

        halfWallButton = GameObject.Find("/Canvas/ARModificationMode/WallTriggerPanel/HalfWallButton").gameObject.GetComponent<Button>();
        halfWallButton.onClick.AddListener(TriggerHalfWall);

        modificationGuidePanel = GameObject.Find("/Canvas/ARModificationMode/ModificationGuidePanel").gameObject.GetComponent<Image>();
        modificationGuidePanel.gameObject.transform.localScale = Vector2.zero;

        closeModificationGuidePanelButton = GameObject.Find("/Canvas/ARModificationMode/ModificationGuidePanel/CloseButton").gameObject.GetComponent<Button>();
        closeModificationGuidePanelButton.onClick.AddListener(CloseModificationGuidePanel);

        triggerWallButtonBg = GameObject.Find("/Canvas/ARModificationMode/WallTriggerPanel/ButtonBg").gameObject.GetComponent<Image>();
        triggerFloorButtonBg = GameObject.Find("/Canvas/ARModificationMode/FloorTriggerPanel/ButtonBg").gameObject.GetComponent<Image>();

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
            
            if (firstFloor != null)
            {
                if (firstFloor.activeInHierarchy)
                {
                    if (upperWall.onFloor == FloorType.FirstFloor)
                    {
                        upperWall.gameObject.SetActive(isFullWall);
                        Lean.Touch.LeanSelectableByFinger parentPaintLeanSelectable = upperWall.transform.parent.GetComponent<Lean.Touch.LeanSelectableByFinger>();
                        if (parentPaintLeanSelectable != null)
                            parentPaintLeanSelectable.enabled = isFullWall;
                    }

                }
                else
                {
                    if (upperWall.onFloor == FloorType.GroundFloor)
                    {
                        upperWall.gameObject.SetActive(isFullWall);
                        Lean.Touch.LeanSelectableByFinger parentPaintLeanSelectable = upperWall.transform.parent.GetComponent<Lean.Touch.LeanSelectableByFinger>();
                        if (parentPaintLeanSelectable != null)
                            parentPaintLeanSelectable.enabled = isFullWall;
                    }
                }
            }
            else
            {
                upperWall.gameObject.SetActive(isFullWall);
                Lean.Touch.LeanSelectableByFinger parentPaintLeanSelectable = upperWall.transform.parent.GetComponent<Lean.Touch.LeanSelectableByFinger>();
                if (parentPaintLeanSelectable != null)
                    parentPaintLeanSelectable.enabled = isFullWall;
            }
        }

        foreach (MiddleWall middleWall in middleWallList)
        {
            if (firstFloor != null)
            {
                if (firstFloor.activeInHierarchy)
                {
                    if (middleWall.onFloor == FloorType.FirstFloor)
                    {
                        middleWall.gameObject.SetActive(isFullWall);
                    }
                }
                else
                {
                    if (middleWall.onFloor == FloorType.GroundFloor)
                    {
                        middleWall.gameObject.SetActive(isFullWall);
                    }
                }
            }
            else
            {
                middleWall.gameObject.SetActive(isFullWall);
            }
        }

    }

    private void ViewFirstFloor()
    {
        SearchAndTriggerAllWalls(true);

        firstFloor.SetActive(true);
        LeanTween.moveLocalY(triggerFloorButtonBg.gameObject, 50.0f, 0.25f).setEaseInOutQuart();

        SearchAndTriggerAllWalls(isFullWall);
    }

    private void ViewGroundFloor()
    {
        firstFloor.SetActive(false);
        LeanTween.moveLocalY(triggerFloorButtonBg.gameObject, -50.0f, 0.25f).setEaseInOutQuart();

        SearchAndTriggerAllWalls(isFullWall);
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
            LeanTween.scale(moveGuidePanel.gameObject, new Vector2(1, 1), 0.25f).setEaseOutBack();
            LeanTween.scale(pinchGuidePanel.gameObject, new Vector2(1, 1), 0.25f).setEaseOutBack();
        }
    }

    private void HideGuidePanels()
    {
        if (!currentSelectable.CompareTag("Paint") && !currentSelectable.CompareTag("Floor"))
        {
            LeanTween.scale(moveGuidePanel.gameObject, new Vector2(0, 0), 0.25f).setEaseInBack();
            LeanTween.scale(pinchGuidePanel.gameObject, new Vector2(0, 0), 0.25f).setEaseInBack();
        }
    }

    private void CloseModificationGuidePanel()
    {
        LeanTween.scale(modificationGuidePanel.gameObject, new Vector2(0, 0), 0.1f);
    }
}
