using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewARRoomUIHandler : MonoBehaviour
{
    CanvasManager canvasManager;
    NetworkManager networkManager;

    Button backButton;

    Button previousButton;
    Button nextButton;

    InputField roomNameInputField;
    Button createButton;

    Image modelImage;
    Text modelNameText;
    Text modelDescriptionText;

    GameObject errorMessageBox;

    int modelIndex = 0;

    private void Start()
    {
        canvasManager = gameObject.GetComponentInParent<CanvasManager>();
        networkManager = GameObject.Find("/NetworkManager").GetComponent<NetworkManager>();

        backButton = transform.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(BackToCreateARRoom);

        previousButton = transform.Find("ModelSelectionPanel/PreviousButton").GetComponent<Button>();
        previousButton.onClick.AddListener(PreviousModelItem);
        nextButton = transform.Find("ModelSelectionPanel/NextButton").GetComponent<Button>();
        nextButton.onClick.AddListener(NextModelItem);

        modelImage = transform.Find("ModelSelectionPanel/ModelImage").GetComponent<Image>();
        modelNameText = transform.Find("ModelInfoPanel/ModelNameText").GetComponent<Text>();
        modelDescriptionText = transform.Find("ModelInfoPanel/ModelDescriptionText").GetComponent<Text>();

        roomNameInputField = transform.Find("RoomNamePanel/RoomNameInputField").GetComponent<InputField>();
        createButton = transform.Find("RoomNamePanel/CreateNewRoomButton").GetComponent<Button>();
        createButton.onClick.AddListener(CreateARRoom);

        errorMessageBox = transform.Find("ErrorMessageBox").gameObject;
        errorMessageBox.transform.localScale = new Vector2(0, 1);

        BindModelData(0);
    }

    void PreviousModelItem()
    {
        modelIndex--;
        CheckModelPrefabsBoundary();
    }

    void NextModelItem ()
    {
        modelIndex++;
        CheckModelPrefabsBoundary();
    }

    void CheckModelPrefabsBoundary ()
    {
        if (modelIndex >= MainManager.Instance.aRModelPrefabs.Length)
        {
            modelIndex = 0;
        }
        else if (modelIndex < 0)
        {
            modelIndex = MainManager.Instance.aRModelPrefabs.Length - 1;
        }

        BindModelData(modelIndex);
    }

    void BindModelData (int modelIndex)
    {
        MainManager.Instance.selectedARModelPrefab = MainManager.Instance.aRModelPrefabs[modelIndex];

        modelImage.sprite = MainManager.Instance.aRModelPrefabs[modelIndex].GetComponent<ARModel>().Thumbnail;
        modelNameText.text = MainManager.Instance.aRModelPrefabs[modelIndex].GetComponent<ARModel>().ModelName;
        modelDescriptionText.text = MainManager.Instance.aRModelPrefabs[modelIndex].GetComponent<ARModel>().ModelDescription;
    }

    void CreateARRoom ()
    {
        if (roomNameInputField.text.Length > 0)
            networkManager.CreateARRoom(roomNameInputField.text);
        else
        {
            errorMessageBox.transform.localScale = new Vector2(0, 1);
            LeanTween.scale(errorMessageBox, new Vector2(1, 1), 0.1f).setEaseInSine();
        }
    }

    void BackToCreateARRoom()
    {
        errorMessageBox.transform.localScale = new Vector2(0, 1);
        canvasManager.SwitchCanvas(CanvasType.CreateARRoom);
    }

}
