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
    ARPointCloudManager pointCloudManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    ARManager aRManager;

    #endregion


    #region GameObject

    [HideInInspector] public GameObject aRModel;

    Vector3 aRModelInitialPosition;
    Vector3 aRModelInitialScale;
    #endregion


    #region UI

    Image movePhoneImage;
    Image moveGuidePanel;
    Image pinchGuidePanel;
    Image confirmGuidePanel;
    Button closeButton;
    int displayCount = 0;
    Button confirmButton;

    Image modificationGuidePanel;

    Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0f);
    Vector3 maxScale = new Vector3(2.0f, 2.0f, 2.0f);
    Vector3 minScale = new Vector3(0.2f, 0.2f, 0.2f);

    #endregion


    void Start()
    {
        raycastManager = GameObject.Find("/AR Session Origin").gameObject.GetComponent<ARRaycastManager>();
        pointCloudManager = GameObject.Find("/AR Session Origin").gameObject.GetComponent<ARPointCloudManager>();
        aRManager = transform.GetComponentInParent<ARManager>();

        movePhoneImage = GameObject.Find("/Canvas/ARPlacementMode/MovePhoneImage").gameObject.GetComponent<Image>();
        movePhoneImage.gameObject.SetActive(true);
        PlayMovePhoneImageAnimation();

        moveGuidePanel = GameObject.Find("/Canvas/ARPlacementMode/MoveGuidePanel").gameObject.GetComponent<Image>();
        moveGuidePanel.gameObject.transform.localScale = Vector2.zero;

        pinchGuidePanel = GameObject.Find("/Canvas/ARPlacementMode/PinchGuidePanel").gameObject.GetComponent<Image>();
        pinchGuidePanel.gameObject.transform.localScale = Vector2.zero;

        confirmGuidePanel = GameObject.Find("/Canvas/ARPlacementMode/ConfirmGuidePanel").gameObject.GetComponent<Image>();
        confirmGuidePanel.gameObject.transform.localScale = Vector2.zero;

        closeButton = GameObject.Find("/Canvas/ARPlacementMode/ConfirmGuidePanel/CloseButton").gameObject.GetComponent<Button>();
        closeButton.onClick.AddListener(CloseConfirmGuidePanel);

        confirmButton = GameObject.Find("/Canvas/ARPlacementMode/ConfirmButton").gameObject.GetComponent<Button>();
        confirmButton.gameObject.SetActive(false);
        confirmButton.onClick.AddListener(ChangeToARModificationMode);

        modificationGuidePanel = GameObject.Find("/Canvas/ARModificationMode/ModificationGuidePanel").gameObject.GetComponent<Image>();

    }

    private void PlayMovePhoneImageAnimation()
    {
        LeanTween.moveLocalX(movePhoneImage.gameObject, -100.0f, 1.0f).setEaseInOutSine().setLoopPingPong();
    }

    public void RestartUIFlow ()
    {
        displayCount = 0;
        aRModelInitialPosition = Vector3.zero;
        aRModelInitialScale = Vector3.zero;

        aRModel.transform.Find("MoveIndicator").gameObject.SetActive(true);

        pointCloudManager.SetTrackablesActive(true);
        pointCloudManager.enabled = true;

        movePhoneImage.gameObject.SetActive(true);
        moveGuidePanel.gameObject.transform.localScale = Vector2.zero;
        pinchGuidePanel.gameObject.transform.localScale = Vector2.zero;
        confirmGuidePanel.gameObject.transform.localScale = Vector2.zero;
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

        if (aRModel.transform.position != aRModelInitialPosition && displayCount == 0 && moveGuidePanel.transform.localScale != Vector3.zero)
        {
            LeanTween.scale(moveGuidePanel.gameObject, new Vector2(0, 0), 0.5f).setEaseInBack().setDelay(0.5f);
            LeanTween.scale(pinchGuidePanel.gameObject, new Vector2(1, 1), 0.5f).setEaseOutBack().setDelay(1.5f);
            displayCount = 1;
        } 
        else if (aRModel.transform.localScale != aRModelInitialScale && displayCount == 1 && moveGuidePanel.transform.localScale == Vector3.zero)
        {
            LeanTween.scale(pinchGuidePanel.gameObject, new Vector2(0, 0), 0.5f).setEaseInBack().setDelay(0.5f);
            displayCount = 2;
        }
        else if (displayCount == 2 && pinchGuidePanel.transform.localScale == Vector3.zero)
        {
            LeanTween.scale(confirmGuidePanel.gameObject, new Vector2(1, 1), 0.1f).setEaseOutBack();
            displayCount = 3;
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

                aRModelInitialPosition = aRModel.transform.position;
                aRModelInitialScale = aRModel.transform.localScale;

                movePhoneImage.gameObject.SetActive(false);
                LeanTween.scale(moveGuidePanel.gameObject, new Vector2(1, 1), 0.5f).setEaseOutBack().setDelay(1.0f);
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
        //if (raycastManager.Raycast(Camera.current.ViewportPointToRay(viewportCenter), hits))
        //{
            //aRModel.SetActive(true);
            //aRModel.transform.position = hits[0].pose.position;

            //aRModelInitialPosition = aRModel.transform.position;
            //aRModelInitialScale = aRModel.transform.localScale;

            //movePhoneImage.gameObject.SetActive(false);
            //LeanTween.scale(moveGuidePanel.gameObject, new Vector2(1, 1), 0.5f).setEaseOutBack().setDelay(1.0f);
            //confirmButton.gameObject.SetActive(true);
        //}

        
        if (IsPointerOverUIObject())
            return;

        if (Input.touchCount == 1)
        {
            if (raycastManager.Raycast(Input.GetTouch(0).position, hits))
            {

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    //aRModel.transform.position = hits[0].pose.position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved && aRModel != null)
                {
                    aRModel.transform.position = hits[0].pose.position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    aRModel.transform.position = hits[0].pose.position;
                }
            }
        }
        
    }

    private void CloseConfirmGuidePanel ()
    {
        LeanTween.scale(confirmGuidePanel.gameObject, new Vector2(0, 0), 0.1f);
    }

    private void ChangeToARModificationMode ()
    {
        aRModel.transform.Find("MoveIndicator").gameObject.SetActive(false);

        pointCloudManager.SetTrackablesActive(false);
        pointCloudManager.enabled = false;

        aRModel.GetComponent<Lean.Touch.LeanPinchScale>().enabled = false;
        aRModel.GetComponent<Lean.Touch.LeanTwistRotateAxis>().enabled = false;

        aRManager.SetActiveARModificationMode(true);
        aRManager.SetActiveARPlacementMode(false);

        LeanTween.scale(modificationGuidePanel.gameObject, new Vector2(1, 1), 0.1f).setEaseOutBack().setDelay(1.0f);
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
