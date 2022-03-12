using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{
    #region Managers

    ARRaycastManager raycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    ARManager aRManager;

    #endregion


    #region GameObject

    [HideInInspector] public GameObject aRModel;

    #endregion


    #region UI

    Image movePhoneImage;
    Button confirmButton;

    Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0f);
    Vector3 maxScale = new Vector3(2.0f, 2.0f, 2.0f);
    Vector3 minScale = new Vector3(0.2f, 0.2f, 0.2f);

    #endregion


    void Start()
    {
        raycastManager = GameObject.Find("/AR Session Origin").gameObject.GetComponent<ARRaycastManager>();
        aRManager = transform.GetComponentInParent<ARManager>();

        movePhoneImage = GameObject.Find("/Canvas/ARPlacementMode/MovePhoneImage").gameObject.GetComponent<Image>();
        confirmButton = GameObject.Find("/Canvas/ARPlacementMode/ConfirmButton").gameObject.GetComponent<Button>();

        movePhoneImage.gameObject.SetActive(true);
        confirmButton.gameObject.SetActive(false);

        confirmButton.onClick.AddListener(ChangeToARModificationMode);
    }

    public void RestartUIFlow ()
    {
        movePhoneImage.gameObject.SetActive(true);
        confirmButton.gameObject.SetActive(false);
    }

    void Update()
    {
        
        if (aRModel.activeInHierarchy == false)
        {
            PlaceObject();
        }
        else
        {
            MoveObject();
        }

        

        if (aRModel.transform.localScale.x > maxScale.x)
        {
            aRModel.transform.localScale = maxScale;
        } 
        else if (aRModel.transform.localScale.x < minScale.x)
        {
            aRModel.transform.localScale = minScale;
        }
    }

    private void PlaceObject()
    {
        if(Camera.current != null)
        {
            if (raycastManager.Raycast(Camera.current.ViewportPointToRay(viewportCenter), hits))
            {
                aRModel.SetActive(true);
                aRModel.transform.position = hits[0].pose.position;

                movePhoneImage.gameObject.SetActive(false);
                confirmButton.gameObject.SetActive(true);
            }
        }
        else
        {
            //Debug1("ARPlacementManager:PlaceObject(): No current camera found");
        }
    }

    private void MoveObject()
    {
        if (IsPointerOverUIObject())
            return;

        if (Input.touchCount == 1)
        {
            if (raycastManager.Raycast(Input.GetTouch(0).position, hits))
            {

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    aRModel.transform.position = hits[0].pose.position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved && aRModel != null)
                {
                    aRModel.transform.position = hits[0].pose.position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    //empty
                }
            }
        }
    }

    private void ChangeToARModificationMode ()
    {
        aRModel.GetComponent<Lean.Touch.LeanPinchScale>().enabled = false;
        aRModel.GetComponent<Lean.Touch.LeanTwistRotateAxis>().enabled = false;

        aRManager.SetActiveARModificationMode(true);
        aRManager.SetActiveARPlacementMode(false);
    }

    bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
