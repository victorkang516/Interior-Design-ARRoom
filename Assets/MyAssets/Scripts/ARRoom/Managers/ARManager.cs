using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

using Photon.Pun;

public class ARManager : MonoBehaviour
{
    ARSession aRSession;
    ARPlacementManager aRPlacementManager;
    ARModificationManager aRModificationManager;
    LoadSaveManager loadSaveManager;


    public GameObject aRModel;
    ARModel selectedARModel;


    // Options
    Button optionButton;
    GameObject optionPanel;

    Button saveButton;
    Button resetButton;


    GameObject floorTriggerPanel;

    bool isFirstTime = true;
    

    void Start()
    {
        aRSession = GameObject.Find("/AR Session").GetComponent<ARSession>();

        aRPlacementManager = transform.Find("ARPlacementMode").gameObject.GetComponent<ARPlacementManager>();
        aRModificationManager = transform.Find("ARModificationMode").gameObject.GetComponent<ARModificationManager>();
        loadSaveManager = GameObject.Find("LoadSaveManager").GetComponent<LoadSaveManager>();

        optionButton = GameObject.Find("OptionButton").GetComponent<Button>();
        optionButton.onClick.AddListener(ShowOptionPanel);

        saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
        saveButton.onClick.AddListener(SaveData);

        resetButton = GameObject.Find("ResetButton").GetComponent<Button>();
        resetButton.onClick.AddListener(ResetARSession);

        optionPanel = GameObject.Find("OptionPanel");
        optionPanel.transform.localScale = Vector2.zero;
        

        floorTriggerPanel = GameObject.Find("/Canvas/ARModificationMode/FloorTriggerPanel");
    }

    private void Update()
    {
        if (isFirstTime && CheckAllManagersAreReady() && CheckSelectedARModelPrefabIsExist() )
        {
            selectedARModel = MainManager.Instance.selectedARModelPrefab.GetComponent<ARModel>();

            InitializeUI();
            InitializeManagers();
            InitializeARModel();

            isFirstTime = false;
        }
    }

    private bool CheckAllManagersAreReady () => 
        aRModificationManager.gameObject.activeInHierarchy && aRPlacementManager.gameObject.activeInHierarchy;
    private bool CheckSelectedARModelPrefabIsExist () =>  MainManager.Instance.selectedARModelPrefab != null;

    private void InitializeUI()
    {
        if (selectedARModel.modelType == ModelType.Studio && selectedARModel.modelType == ModelType.TwoBedroomUnit)
            floorTriggerPanel.SetActive(false);
        else if (selectedARModel.modelType == ModelType.Loft)
            floorTriggerPanel.SetActive(true);
    }

    private void InitializeManagers()
    {
        SetActiveARPlacementMode(false);
        SetActiveARModificationMode(false);
    }

    private void InitializeARModel()
    {
        if (CheckIfRoomKeyExist())
        {
            loadSaveManager.LoadData();
        }
        else
        {
            aRModel = Instantiate(selectedARModel.gameObject, Vector3.zero, 
                selectedARModel.gameObject.transform.rotation);
            AssignARModelToManagers();
            aRModel.SetActive(false);
        }
    }

    private bool CheckIfRoomKeyExist() => MainManager.Instance.roomKey != "";


    public void AssignARModelToManagers ()
    {
        aRPlacementManager.aRModel = aRModel;
        loadSaveManager.aRModel = aRModel;

        if (selectedARModel.modelType == ModelType.Loft)
        {
            aRModificationManager.firstFloor = aRModel.transform.Find("FirstFloor").gameObject;
        }

        aRModificationManager.middleWallList = FindObjectsOfType<MiddleWall>();
        aRModificationManager.upperWallList = FindObjectsOfType<UpperWall>();

        SetActiveARPlacementMode(true);
    }


    public void SetActiveARPlacementMode (bool isActive)
    {
        aRPlacementManager.gameObject.SetActive(isActive);
    }

    public void SetActiveARModificationMode (bool isActive)
    {
        aRModificationManager.gameObject.SetActive(isActive);
    }


    private void ShowOptionPanel()
    {
        optionPanel.GetComponent<OptionPanelHandler>().Show();
    }

    private void SaveData ()
    {
        if (aRModel.activeInHierarchy)
            loadSaveManager.SaveData();
        optionPanel.GetComponent<OptionPanelHandler>().ActionDone();
    }


    private void ResetARSession()
    {
        aRSession.Reset();
        aRModel.GetComponent<Lean.Touch.LeanPinchScale>().enabled = true;
        aRModel.GetComponent<Lean.Touch.LeanTwistRotateAxis>().enabled = true;
        aRModel.SetActive(false);
        ResetAllMode();
    }

    private void ResetAllMode()
    {
        aRPlacementManager.RestartUIFlow();
        aRModificationManager.RestartUIFlow();
        SetActiveARPlacementMode(true);
        SetActiveARModificationMode(false);
    }

}
