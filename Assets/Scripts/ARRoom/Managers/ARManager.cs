using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARManager : MonoBehaviour
{
    ARSession aRSession;
    ARPlacementManager aRPlacementManager;
    ARModificationManager aRModificationManager;

    GameObject aRModel;

    Button resetButton;

    [SerializeField] GameObject aRModelPrefab;

    int firstCount = 0;

    void Start()
    {
        aRSession = GameObject.Find("/AR Session").GetComponent<ARSession>();

        aRPlacementManager = transform.Find("ARPlacementMode").gameObject.GetComponent<ARPlacementManager>();
        aRModificationManager = transform.Find("ARModificationMode").gameObject.GetComponent<ARModificationManager>();

        resetButton = GameObject.Find("/Canvas/ARBasicMode/ResetButton").GetComponent<Button>();
        resetButton.onClick.AddListener(ResetARSession);
    }

    private void Update()
    {
        if (firstCount == 0 && aRModificationManager.gameObject.activeInHierarchy && aRPlacementManager.gameObject.activeInHierarchy)
        {
            InitializeManagers();
            InitializeARModel();
            firstCount++;
        }
    }

    private void ResetARSession ()
    {
        aRSession.Reset();
        aRModel.GetComponent<Lean.Touch.LeanPinchScale>().enabled = true;
        aRModel.GetComponent<Lean.Touch.LeanTwistRotateAxis>().enabled = true;
        aRModel.SetActive(false);
        ResetAllMode();
    }

    private void ResetAllMode ()
    {
        aRPlacementManager.RestartUIFlow();
        aRModificationManager.RestartUIFlow();
        SetActiveARPlacementMode(true);
        SetActiveARModificationMode(false);
    }

    public void SetActiveARPlacementMode (bool isActive)
    {
        aRPlacementManager.gameObject.SetActive(isActive);
    }

    public void SetActiveARModificationMode (bool isActive)
    {
        aRModificationManager.gameObject.SetActive(isActive);
    }

    private void InitializeManagers ()
    {
        SetActiveARPlacementMode(true);
        SetActiveARModificationMode(false);
    }

    private void InitializeARModel()
    {
        // TODO Check which condotype selected
        aRModel = Instantiate(aRModelPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        aRPlacementManager.aRModel = aRModel;
        aRModificationManager.middleWallList = FindObjectsOfType<MiddleWall>();
        aRModificationManager.upperWallList = FindObjectsOfType<UpperWall>();

        aRModel.SetActive(false);
    }
}
