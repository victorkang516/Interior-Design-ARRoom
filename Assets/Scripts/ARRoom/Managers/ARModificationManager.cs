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

    public Material paintMaterial01;

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
        currentSelectable.GetComponent<Outline>().enabled = true;

        yBoundary = currentSelectable.transform.position.y;

        if (currentSelectable.CompareTag("Toilet") || currentSelectable.CompareTag("Shower"))
        {
            // Do nothing
        }
        else
        {
            objectListPanel.SetActive(true);
            objectListHandler.CreateObjectList(currentSelectable);
        }
    }

    public void DeselectARObject()
    {
        if (currentSelectable.CompareTag("Toilet") || currentSelectable.CompareTag("Shower"))
        {
            // Do nothing
        }
        else
        {
            objectListPanel.SetActive(false);
            objectListHandler.EmptyObjectList();
        }

        currentSelectable.GetComponent<Outline>().enabled = false;
        currentSelectable = null;
    }



    private void Update()
    {
        if (currentSelectable == null)
            return;

        //MainManager.Instance.Debug1("ARModificationManager: selectableY" + currentSelectable.transform.position.y);
        //MainManager.Instance.Debug1("ARModificationManager: yBoundary:" + yBoundary);

        if (currentSelectable.transform.position.y != yBoundary)
        {
            currentSelectable.transform.position = new Vector3(currentSelectable.transform.position.x, yBoundary, currentSelectable.transform.position.z);
        }

        //if (Input.GetTouch(0).tapCount > 0)
        //{
        //    RaycastHit hitInfo = new RaycastHit();
        //    bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hitInfo);
        //    if (hit)
        //    {
        //        debug2.text = Time.deltaTime + ": Hit " + hitInfo.transform.gameObject.name;
        //        //Debug.Log("Hit " + hitInfo.transform.gameObject.name);
        //        if (hitInfo.transform.gameObject.tag == "Selectable")
        //        {
        //            if (hideWall == false)
        //            {
        //                WallPaint[] wallpaintList = hitInfo.transform.GetComponentsInChildren<WallPaint>();
        //                foreach (WallPaint wallPaint in wallpaintList)
        //                {
        //                    wallPaint.GetComponent<Renderer>().material = paintMaterial01;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            debug2.text = Time.deltaTime + "nopz";
        //            Debug.Log("nopz");
        //        }
        //    }
        //    else
        //    {
        //        debug2.text = Time.deltaTime + "No hit";
        //        Debug.Log("No hit");
        //    }
        //    debug2.text = Time.deltaTime + "Touch is down";
        //    Debug.Log("Mouse is down");
        //}
    }
}
