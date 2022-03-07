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
        Debug1(movePhoneImage.ToString());
        Debug2(confirmButton.ToString());

        if (aRModel.activeInHierarchy == false)
        {
            PlaceObject();
        }
        else
        {
            MoveObject();
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
        if (Input.touchCount > 0)
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
        aRManager.SetActiveARModificationManager(true);
        aRManager.SetActiveARPlacementManager(false);
    }


    // For Development
    public Text debugText1;
    public Text debugText2;

    private void Debug1(string message)
    {
        debugText1.text = Time.fixedTime + " :" + message;
    }

    private void Debug2(string message)
    {
        debugText2.text = Time.fixedTime + " :" + message;
    }

}
