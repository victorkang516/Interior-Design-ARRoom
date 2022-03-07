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

    void Start()
    {
        aRSession = GameObject.Find("/AR Session").GetComponent<ARSession>();

        aRPlacementManager = transform.Find("ARPlacementMode").gameObject.GetComponent<ARPlacementManager>();
        aRModificationManager = transform.Find("ARModificationMode").gameObject.GetComponent<ARModificationManager>();

        resetButton = GameObject.Find("/Canvas/ARBasicMode/ResetButton").GetComponent<Button>();
        resetButton.onClick.AddListener(ResetARSession);

        InitializeARModel();
    }

    private void ResetARSession ()
    {
        aRSession.Reset();
        aRModel.SetActive(false);
        SetActiveARPlacementManager(true);
        SetActiveARModificationManager(false);
    }

    public void SetActiveARPlacementManager (bool isActive)
    {
        aRPlacementManager.gameObject.SetActive(isActive);
        aRPlacementManager.RestartUIFlow();
    }

    public void SetActiveARModificationManager (bool isActive)
    {
        aRModificationManager.gameObject.SetActive(isActive);
        aRModificationManager.RestartUIFlow();
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
