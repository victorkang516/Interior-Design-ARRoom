using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase.Extensions;

public class LoadARRoomUIHandler : MonoBehaviour
{

    public GameObject roomItemPrefab;

    CanvasManager canvasManager;
    NetworkManager networkManager;

    GameObject roomListPanel;

    Button backButton;

    private void Start()
    {
        canvasManager = gameObject.GetComponentInParent<CanvasManager>();
        networkManager = GameObject.Find("/NetworkManager").GetComponent<NetworkManager>();

        roomListPanel = GameObject.Find("RoomListPanel");

        backButton = gameObject.transform.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(BackToCreateARRoom);

        LoadAllRooms();
    }

    private void LoadAllRooms ()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("users").Child(AuthManager.Instance.user.UserId).Child("rooms")
      .GetValueAsync().ContinueWithOnMainThread(task => {
          if (task.IsFaulted)
          {
              // Handle the error...
              Debug.Log("Error");
          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
              // Do something with snapshot...

              foreach (DataSnapshot room in snapshot.Children)
              {
                  IDictionary dictRoom = (IDictionary)room.Value;

                  string aRModelType = (string)dictRoom["aRModelType"];
                  string roomName = (string)dictRoom["roomName"];
                  string lastSavedDate = (string)dictRoom["lastSavedDate"];

                  Debug.Log(aRModelType + " " + roomName + " " + lastSavedDate);

                  GameObject roomItem = Instantiate(roomItemPrefab, roomListPanel.transform);
                  roomItem.transform.GetChild(0).GetComponent<Text>().text = roomName;
                  roomItem.transform.GetChild(1).GetComponent<Text>().text = aRModelType;
                  roomItem.transform.GetChild(2).GetComponent<Text>().text = lastSavedDate;

                  roomItem.GetComponent<Button>().onClick.AddListener(() => LoadARRoom(room.Key, roomName, aRModelType) );
              }
          }
      });
    }

    void BackToCreateARRoom()
    {
        canvasManager.SwitchCanvas(CanvasType.CreateARRoom);
    }

    void LoadARRoom (string roomKey, string roomName, string aRModelType)
    {
        MainManager.Instance.roomKey = roomKey;

        if (aRModelType == "Studio")
            MainManager.Instance.selectedARModelPrefab = MainManager.Instance.aRModelPrefabs[0];
        else if (aRModelType == "Loft Apartment")
            MainManager.Instance.selectedARModelPrefab = MainManager.Instance.aRModelPrefabs[1];

        networkManager.CreateARRoom(roomName);
    }
}
