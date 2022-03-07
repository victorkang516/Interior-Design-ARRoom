using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARModificationManager : MonoBehaviour
{
    Button lowerWallButton;
    bool hideWall = false;

    [HideInInspector] public UpperWall[] upperWallList;
    [HideInInspector] public MiddleWall[] middleWallList;

    public Material paintMaterial01;
    void Start()
    {
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "Selectable")
                {
                    if (hideWall == false)
                    {
                        WallPaint[] wallpaintList = hitInfo.transform.GetComponentsInChildren<WallPaint>();
                        foreach (WallPaint wallPaint in wallpaintList)
                        {
                            wallPaint.GetComponent<Renderer>().material = paintMaterial01;
                        }
                    }
                }
                else
                {
                    Debug.Log("nopz");
                }
            }
            else
            {
                Debug.Log("No hit");
            }
            Debug.Log("Mouse is down");
        }
    }
}
