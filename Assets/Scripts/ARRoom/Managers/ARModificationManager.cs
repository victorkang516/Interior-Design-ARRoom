using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARModificationManager : MonoBehaviour
{
    SelectionGenerator selectionGenerator;
    GameObject objectSelection;

    Button lowerWallButton;
    bool hideWall = false;

    [HideInInspector] public UpperWall[] upperWallList;
    [HideInInspector] public MiddleWall[] middleWallList;

    GameObject currentSelectable;

    public Material paintMaterial01;

    void Start()
    {
        selectionGenerator = GameObject.Find("/Canvas/ARModificationMode/ObjectSelection/Scroll/Panel").GetComponent<SelectionGenerator>();
        objectSelection = GameObject.Find("/Canvas/ARModificationMode/ObjectSelection");
        objectSelection.SetActive(false);

        lowerWallButton = GameObject.Find("/Canvas/ARModificationMode/HideWallButton").gameObject.GetComponent<Button>();
        lowerWallButton.onClick.AddListener(HideTheWall);
        lowerWallButton.gameObject.SetActive(true);
    }

    public void RestartUIFlow ()
    {
        lowerWallButton.gameObject.SetActive(true);
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

        objectSelection.SetActive(true);
        selectionGenerator.DisplaySelections(currentSelectable);
    }

    public void DeselectARObject()
    {
        currentSelectable.GetComponent<Outline>().enabled = false;

        objectSelection.SetActive(false);
        selectionGenerator.EmptyScrollList();
    }

    private void Update()
    {
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
