using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using Photon.Pun;

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
        moveGuidePanel.gameObject.SetActive(false);

        pinchGuidePanel = GameObject.Find("/Canvas/ARPlacementMode/PinchGuidePanel").gameObject.GetComponent<Image>();
        pinchGuidePanel.gameObject.SetActive(false);

        confirmGuidePanel = GameObject.Find("/Canvas/ARPlacementMode/ConfirmGuidePanel").gameObject.GetComponent<Image>();
        confirmGuidePanel.gameObject.transform.localScale = Vector2.zero;

        closeButton = GameObject.Find("/Canvas/ARPlacementMode/ConfirmGuidePanel/CloseButton").gameObject.GetComponent<Button>();
        closeButton.onClick.AddListener(CloseConfirmGuidePanel);

        confirmButton = GameObject.Find("/Canvas/ARPlacementMode/ConfirmButton").gameObject.GetComponent<Button>();
        confirmButton.gameObject.SetActive(false);
        confirmButton.onClick.AddListener(ChangeToARModificationMode);

        modificationGuidePanel = GameObject.Find("/Canvas/ARModificationMode/ModificationGuidePanel").gameObject.GetComponent<Image>();
    }


    void Update()
    {
        
        if (IfARModelNotActiveInHierachy())
            PlaceARModel();
        else
            MoveARModel();

        HandleGuidanceUIAnimation();

        ConstraintARModelScale();
    }

    private bool IfARModelNotActiveInHierachy() => aRModel.activeInHierarchy == false;

    private void PlaceARModel()
    {
        if (Camera.current != null)
        {
            if (raycastManager.Raycast(Camera.current.ViewportPointToRay(viewportCenter), hits))
            {
                aRModel.SetActive(true);
                aRModel.transform.position = hits[0].pose.position;

                aRModelInitialPosition = aRModel.transform.position;
                aRModelInitialScale = aRModel.transform.localScale;

                movePhoneImage.gameObject.SetActive(false);

                StopAllCoroutines();
                StartCoroutine(PlayMoveGuidePanelAnimation());
            }
        }
    }

    private void MoveARModel()
    {

        if (IsPointerOverUIObject())
            return;

        if (Input.touchCount == 1)
        {
            if (raycastManager.Raycast(Input.GetTouch(0).position, hits))
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved && aRModel != null)
                    aRModel.transform.position = hits[0].pose.position;
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    aRModel.transform.position = hits[0].pose.position;
            }
        }
    }

    private void HandleGuidanceUIAnimation()
    {
        if (aRModel.transform.position != aRModelInitialPosition && displayCount == 0 && moveGuidePanel.gameObject.activeInHierarchy == true)
        {
            moveGuidePanel.gameObject.SetActive(false);

            StopAllCoroutines();
            StartCoroutine(PlayPinchGuidePanelAnimation());

            displayCount = 1;
        }
        else if (aRModel.transform.localScale != aRModelInitialScale && displayCount == 1 && pinchGuidePanel.gameObject.activeInHierarchy == true)
        {
            pinchGuidePanel.gameObject.SetActive(false);

            StopAllCoroutines();
            confirmButton.gameObject.SetActive(true);
            LeanTween.scale(confirmButton.gameObject, new Vector3(1.25f, 1.25f, 1.25f), 1.0f).setEaseInOutSine().setLoopPingPong();

            displayCount = 2;
        }
        else if (displayCount == 2 && pinchGuidePanel.gameObject.activeInHierarchy == false)
        {
            LeanTween.scale(confirmGuidePanel.gameObject, new Vector2(1, 1), 0.1f).setEaseOutBack().setDelay(1.5f);
            displayCount = 3;
        }
    }

    IEnumerator PlayMoveGuidePanelAnimation()
    {
        yield return new WaitForSeconds(1.0f);

        moveGuidePanel.gameObject.SetActive(true);
        LeanTween.moveLocalX(moveGuidePanel.gameObject, -200f, 2.0f).setEaseInOutSine().setLoopPingPong();
    }

    IEnumerator PlayPinchGuidePanelAnimation()
    {
        yield return new WaitForSeconds(1.0f);

        pinchGuidePanel.gameObject.SetActive(true);
        LeanTween.rotateAroundLocal(pinchGuidePanel.transform.GetChild(1).gameObject, new Vector3(0, 0, 1), 90f, 2.0f).setEaseInOutSine().setLoopPingPong().setDelay(1.0f);
    }
    

    private void ConstraintARModelScale()
    {
        if (aRModel.transform.localScale.x > maxScale.x)
        {
            aRModel.transform.localScale = maxScale;
        }
        else if (aRModel.transform.localScale.x < minScale.x)
        {
            aRModel.transform.localScale = minScale;
        }
    }

    private void PlayMovePhoneImageAnimation()
    {
        LeanTween.scale(movePhoneImage.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 1.0f).setEaseInOutSine().setLoopPingPong();
    }

    public void RestartUIFlow()
    {
        displayCount = 0;
        aRModelInitialPosition = Vector3.zero;
        aRModelInitialScale = Vector3.zero;

        aRModel.transform.Find("MoveIndicator").gameObject.SetActive(true);

        pointCloudManager.SetTrackablesActive(true);
        pointCloudManager.enabled = true;

        movePhoneImage.gameObject.SetActive(true);
        moveGuidePanel.gameObject.SetActive(false);
        pinchGuidePanel.gameObject.SetActive(false);
        confirmGuidePanel.gameObject.transform.localScale = Vector2.zero;
        confirmButton.gameObject.SetActive(false);
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
