using System;
using UnityEngine;
using Firebase.Database;
using Photon.Pun;
using Firebase.Extensions;
using System.Collections;

public class Room
{
    public string roomName;
    public string aRModelType;
    public string lastSavedDate;

    public float rotationy;
    public float allScale;

    public Room(string roomName, string aRModelType, Transform aRModelTransform)
    {
        this.roomName = roomName;
        this.aRModelType = aRModelType;
        lastSavedDate = DateTime.Now.ToShortDateString();

        this.rotationy = aRModelTransform.rotation.eulerAngles.y;
        this.allScale = aRModelTransform.localScale.x;
    }
}

public class ARDataObject
{
    public string objectType;
    public string gameObjectName;
    public string objectName;
    public string whichFloor;
    public float x;
    public float y;
    public float z;

    public float rotationz;

    public ARDataObject(string objectType, string gameObjectName, string objectName, string whichFloor, Vector3 position, Transform transform)
    {
        this.objectType = objectType;
        this.gameObjectName = gameObjectName;
        this.objectName = objectName;
        this.whichFloor = whichFloor;
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
        this.rotationz = transform.rotation.eulerAngles.y;
    }
}

public class RoomPaintData
{
    public string gameObjectName;
    public string materialName;
    public string whichFloor;

    public RoomPaintData(string gameObjectName, string materialName, string whichFloor)
    {
        this.gameObjectName = gameObjectName;
        this.materialName = materialName;
        this.whichFloor = whichFloor;
    }
}

public class RoomFloorData
{
    public string gameObjectName;
    public string materialName;
    public string whichFloor;

    public RoomFloorData(string gameObjectName, string materialName, string whichFloor)
    {
        this.gameObjectName = gameObjectName;
        this.materialName = materialName;
        this.whichFloor = whichFloor;
    }
}

public class LoadSaveManager : MonoBehaviour
{

    ObjectsPrefabStorage objectsPrefabStorage;

    ARManager aRManager;

    DatabaseReference mDatabaseRef;

    public GameObject aRModel;

    GameObject[] beds;
    GameObject[] sofas;
    GameObject[] racks;
    GameObject[] cabinets;
    GameObject[] tvTables;
    GameObject[] coffeeTables;
    GameObject[] lamps;
    GameObject[] officeTables;
    GameObject[] chairs;
    GameObject[] kitchenChairs;
    GameObject[] kitchenTables;
    GameObject[] kitchenShelfs;
    GameObject[] modularKitchenTables;
    GameObject[] washbasins;
    RoomPaint[] paints;
    RoomFloor[] floors;

    void Start()
    {
        // Get the root reference location of the database.
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        aRManager = GameObject.Find("Canvas").GetComponent<ARManager>();

        objectsPrefabStorage = GameObject.Find("ObjectsPrefabStorage").GetComponent<ObjectsPrefabStorage>();
    }

    private void WriteNewRoom(string roomName, GameObject aRModel)
    {
        Room room = new Room(roomName, aRModel.GetComponent<ARModel>().ModelName, aRModel.transform);
        string json = JsonUtility.ToJson(room);

        

        mDatabaseRef.Child("users").Child(AuthManager.Instance.user.UserId).Child("rooms").Child(MainManager.Instance.roomKey).SetRawJsonValueAsync(json);
    }

    private void OverwriteRoom(string roomName, GameObject aRModel)
    {
        Room room = new Room(roomName, aRModel.GetComponent<ARModel>().ModelName, aRModel.transform);
        string json = JsonUtility.ToJson(room);

        mDatabaseRef.Child("users").Child(AuthManager.Instance.user.UserId).Child("rooms").Child(MainManager.Instance.roomKey).SetRawJsonValueAsync(json);
    }

    public void SaveData ()
    {
        Debug.Log("Save Data");

        // PhotonNetwork.CurrentRoom.Name
        // MainManager.Instance.selectedARModelPrefab.GetComponent<ARModel>().ModelName

        if (MainManager.Instance.roomKey == "")
        {
            MainManager.Instance.roomKey = mDatabaseRef.Child("users").Child(AuthManager.Instance.user.UserId).Push().Key;
            WriteNewRoom(PhotonNetwork.CurrentRoom.Name, aRModel);
        }
        else
        {
            OverwriteRoom(PhotonNetwork.CurrentRoom.Name, aRModel);
        }
        

        beds = GameObject.FindGameObjectsWithTag("Bed");
        sofas = GameObject.FindGameObjectsWithTag("Sofa");
        racks = GameObject.FindGameObjectsWithTag("Rack");
        cabinets = GameObject.FindGameObjectsWithTag("Cabinet");
        tvTables = GameObject.FindGameObjectsWithTag("TV Table");
        coffeeTables = GameObject.FindGameObjectsWithTag("Coffee Table");
        lamps = GameObject.FindGameObjectsWithTag("Lamp");
        officeTables = GameObject.FindGameObjectsWithTag("Office Table");
        chairs = GameObject.FindGameObjectsWithTag("Chair");
        kitchenChairs = GameObject.FindGameObjectsWithTag("Kitchen Chair");
        kitchenTables = GameObject.FindGameObjectsWithTag("Kitchen Table");
        kitchenShelfs = GameObject.FindGameObjectsWithTag("Kitchen Shelf");
        modularKitchenTables = GameObject.FindGameObjectsWithTag("Modular Kitchen Table");
        washbasins = GameObject.FindGameObjectsWithTag("Washbasin");

        paints = FindObjectsOfType<RoomPaint>();
        floors = FindObjectsOfType<RoomFloor>();

        foreach (GameObject bed in beds)
        {
            SaveThisObjectData(bed, "beds");
        }

        foreach (GameObject sofa in sofas)
        {
            SaveThisObjectData(sofa, "sofas");
        }

        foreach (GameObject rack in racks)
        {
            SaveThisObjectData(rack, "racks");
        }

        foreach (GameObject cabinet in cabinets)
        {
            SaveThisObjectData(cabinet, "cabinets");
        }

        foreach (GameObject tvTable in tvTables)
        {
            SaveThisObjectData(tvTable, "tvTables");
        }

        foreach (GameObject coffeeTable in coffeeTables)
        {
            SaveThisObjectData(coffeeTable, "coffeeTables");
        }

        foreach (GameObject lamp in lamps)
        {
            SaveThisObjectData(lamp, "lamps");
        }

        foreach (GameObject officeTable in officeTables)
        {
            SaveThisObjectData(officeTable, "officeTables");
        }

        foreach (GameObject chair in chairs)
        {
            SaveThisObjectData(chair, "chairs");
        }

        foreach (GameObject kitchenChair in kitchenChairs)
        {
            SaveThisObjectData(kitchenChair, "kitchenChairs");
        }

        foreach (GameObject kitchenTable in kitchenTables)
        {
            SaveThisObjectData(kitchenTable, "kitchenTables");
        }

        foreach (GameObject kitchenShelf in kitchenShelfs)
        {
            SaveThisObjectData(kitchenShelf, "kitchenShelfs");
        }

        foreach (GameObject modularKitchenTable in modularKitchenTables)
        {
            SaveThisObjectData(modularKitchenTable, "modularKitchenTables");
        }

        foreach (GameObject washbasin in washbasins)
        {
            SaveThisObjectData(washbasin, "washbasins");
        }

        foreach (RoomPaint paint in paints)
        {
            SaveThisRoomPaintData(paint);
        }

        foreach (RoomFloor floor in floors)
        {
            SaveThisRoomFloorData(floor);
        }
    }

    void SaveThisObjectData(GameObject aRObject, string aRObjectType)
    {
        string whichFloor = "GroundFloor";

        if (aRObject.transform.parent.parent.name == "GroundFloor")
            whichFloor = "GroundFloor";
        else if (aRObject.transform.parent.parent.name == "FirstFloor")
            whichFloor = "FirstFloor";

        ARDataObject aRDataObject = new ARDataObject(
            aRObjectType, 
            aRObject.gameObject.name, 
            aRObject.GetComponent<ARObject>().ObjectName, 
            whichFloor, 
            aRObject.transform.position - aRModel.transform.position, 
            aRObject.transform
            );

        string json = JsonUtility.ToJson(aRDataObject);

        mDatabaseRef.Child("users")
            .Child(AuthManager.Instance.user.UserId)
            .Child("rooms")
            .Child(MainManager.Instance.roomKey)
            .Child("furnitures").Push().SetRawJsonValueAsync(json);
    }

    void SaveThisRoomPaintData (RoomPaint roomPaint)
    {
        string whichFloor = "GroundFloor";

        if (roomPaint.transform.parent.parent.name == "GroundFloor")
            whichFloor = "GroundFloor";
        else if (roomPaint.transform.parent.parent.name == "FirstFloor")
            whichFloor = "FirstFloor";

        RoomPaintData roomPaintData = new RoomPaintData(roomPaint.gameObject.name, roomPaint.materialName, whichFloor);
        string json = JsonUtility.ToJson(roomPaintData);

        mDatabaseRef.Child("users").Child(AuthManager.Instance.user.UserId).Child("rooms").Child(MainManager.Instance.roomKey).Child("roomPaints").Push().SetRawJsonValueAsync(json);
    }

    void SaveThisRoomFloorData(RoomFloor roomFloor)
    {
        string whichFloor = "GroundFloor";

        if (roomFloor.transform.parent.parent.name == "GroundFloor")
            whichFloor = "GroundFloor";
        else if (roomFloor.transform.parent.parent.name == "FirstFloor")
            whichFloor = "FirstFloor";

        RoomFloorData roomFloorData = new RoomFloorData(roomFloor.gameObject.name, roomFloor.materialName, whichFloor);
        string json = JsonUtility.ToJson(roomFloorData);

        mDatabaseRef.Child("users").Child(AuthManager.Instance.user.UserId).Child("rooms").Child(MainManager.Instance.roomKey).Child("roomFloors").Push().SetRawJsonValueAsync(json);
    }

    public void LoadData ()
    {

        FirebaseDatabase.DefaultInstance
      .GetReference("users").Child(AuthManager.Instance.user.UserId).Child("rooms").Child(MainManager.Instance.roomKey)
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
              
              IDictionary dictRoom = (IDictionary)snapshot.Value;
              

              string aRModelType = (string)dictRoom["aRModelType"];
              string roomName = (string)dictRoom["roomName"];
              double allScale = (double)dictRoom["allScale"];
              double rotationy = (double)dictRoom["rotationy"];

              Debug.Log(aRModelType + " " + allScale + " " + allScale.GetType() + rotationy.GetType());

              LoadARModel(aRModelType, (float)allScale, (float)rotationy);
          }
      });
    }


    private void LoadARModel (string aRModelType, float allScale, float rotationy)
    {
        aRModel = Instantiate(MainManager.Instance.selectedARModelPrefab, new Vector3(0, 0, 0), MainManager.Instance.selectedARModelPrefab.transform.rotation);
        aRModel.transform.localScale = new Vector3(allScale, allScale, allScale);
        aRModel.transform.rotation = Quaternion.Euler(new Vector3(0, rotationy, 0));

        Debug.Log(aRModel.name);
        
        if (aRModel != null)
        {
            aRManager.aRModel = aRModel;
            aRManager.AssignARModelToManagers();
        }

        DestroyDefaultFurnitures();
        LoadAllFurnitures();
        LoadAllRoomPaints();
        LoadAllRoomFloors();

        aRModel.SetActive(false);
    }

    private void DestroyDefaultFurnitures ()
    {
        beds = GameObject.FindGameObjectsWithTag("Bed");
        sofas = GameObject.FindGameObjectsWithTag("Sofa");
        racks = GameObject.FindGameObjectsWithTag("Rack");
        cabinets = GameObject.FindGameObjectsWithTag("Cabinet");
        tvTables = GameObject.FindGameObjectsWithTag("TV Table");
        coffeeTables = GameObject.FindGameObjectsWithTag("Coffee Table");
        lamps = GameObject.FindGameObjectsWithTag("Lamp");
        officeTables = GameObject.FindGameObjectsWithTag("Office Table");
        chairs = GameObject.FindGameObjectsWithTag("Chair");
        kitchenChairs = GameObject.FindGameObjectsWithTag("Kitchen Chair");
        kitchenTables = GameObject.FindGameObjectsWithTag("Kitchen Table");
        kitchenShelfs = GameObject.FindGameObjectsWithTag("Kitchen Shelf");
        modularKitchenTables = GameObject.FindGameObjectsWithTag("Modular Kitchen Table");
        washbasins = GameObject.FindGameObjectsWithTag("Washbasin");

        foreach (GameObject bed in beds)
            Destroy(bed);

        foreach (GameObject sofa in sofas)
            Destroy(sofa);

        foreach (GameObject rack in racks)
            Destroy(rack);

        foreach (GameObject cabinet in cabinets)
            Destroy(cabinet);

        foreach (GameObject tvTable in tvTables)
            Destroy(tvTable);

        foreach (GameObject coffeeTable in coffeeTables)
            Destroy(coffeeTable);

        foreach (GameObject lamp in lamps)
            Destroy(lamp);

        foreach (GameObject officeTable in officeTables)
            Destroy(officeTable);

        foreach (GameObject chair in chairs)
            Destroy(chair);

        foreach (GameObject kitchenChair in kitchenChairs)
            Destroy(kitchenChair);

        foreach (GameObject kitchenTable in kitchenTables)
            Destroy(kitchenTable);

        foreach (GameObject kitchenShelf in kitchenShelfs)
            Destroy(kitchenShelf);

        foreach (GameObject modularKitchenTable in modularKitchenTables)
            Destroy(modularKitchenTable);

        foreach (GameObject washbasin in washbasins)
            Destroy(washbasin);
    }

    private void LoadAllFurnitures ()
    {
        

        FirebaseDatabase.DefaultInstance
      .GetReference("users").Child(AuthManager.Instance.user.UserId).Child("rooms").Child(MainManager.Instance.roomKey).Child("furnitures")
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

              foreach (DataSnapshot aRDataObject in snapshot.Children)
              {
                  IDictionary dictARDataObject = (IDictionary)aRDataObject.Value;

                  string objectType = (string)dictARDataObject["objectType"];
                  string gameObjectName = (string)dictARDataObject["gameObjectName"];
                  string objectName = (string)dictARDataObject["objectName"];
                  string whichFloor = (string)dictARDataObject["whichFloor"];
                  double rotationz = (double)dictARDataObject["rotationz"];
                  double x = (double)dictARDataObject["x"];
                  double y = (double)dictARDataObject["y"];
                  double z = (double)dictARDataObject["z"];

                  Vector3 position = new Vector3((float)x, (float)y, (float)z);

                  Debug.Log(gameObjectName + " " + objectName + " " + rotationz + " " + whichFloor);
                  Debug.Log(x + ", " + y + ", " + z);
                  

                  switch (objectType)
                  {
                      case "beds":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.bedPrefabs, whichFloor);
                          break;
                      case "sofas":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.sofaPrefabs, whichFloor);
                          break;
                      case "racks":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.rackPrefabs, whichFloor);
                          break;
                      case "cabinets":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.cabinetPrefabs, whichFloor);
                          break;
                      case "tvTables":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.tvTablePrefabs, whichFloor);
                          break;
                      case "coffeeTables":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.coffeeTablePrefabs, whichFloor);
                          break;
                      case "lamps":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.lampPrefabs, whichFloor);
                          break;
                      case "officeTables":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.officeTablePrefabs, whichFloor);
                          break;
                      case "chairs":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.chairPrefabs, whichFloor);
                          break;
                      case "kitchenChairs":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.kitchenChairPrefabs, whichFloor);
                          break;
                      case "kitchenTables":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.kitchenTablePrefabs, whichFloor);
                          break;
                      case "kitchenShelfs":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.kitchenShelfPrefabs, whichFloor);
                          break;
                      case "modularKitchenTables":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.modularKitchenTablePrefabs, whichFloor);
                          break;
                      case "washbasins":
                          InstantiateThisObject(gameObjectName, objectName, (float)rotationz, position, objectsPrefabStorage.washbasinPrefabs, whichFloor);
                          break;
                  }
              }

          }
          
      });
    }

    private void InstantiateThisObject(string gameObjectName, string objectName, float rotationz, Vector3 position, GameObject[] prefabs, string whichFloor)
    {
        
        for (int i=0; i< prefabs.Length; i++)
        {
            if (objectName == prefabs[i].GetComponent<ARObject>().ObjectName)
            {
                string tranformHierachy = "";
                if (whichFloor == "GroundFloor")
                    tranformHierachy = "GroundFloor/Furnitures";
                else if (whichFloor == "FirstFloor")
                    tranformHierachy = "FirstFloor/Furnitures";

                GameObject thisObject = Instantiate(prefabs[i], position, Quaternion.Euler(-90, 0, rotationz), aRModel.transform.Find(tranformHierachy).transform);
                thisObject.name = gameObjectName;
            }
        }
    }

    private void LoadAllRoomPaints()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("users").Child(AuthManager.Instance.user.UserId).Child("rooms").Child(MainManager.Instance.roomKey).Child("roomPaints")
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

              foreach (DataSnapshot roomPaint in snapshot.Children)
              {
                  IDictionary dictRoomPaint = (IDictionary)roomPaint.Value;

                  string gameObjectName = (string)dictRoomPaint["gameObjectName"];
                  string materialName = (string)dictRoomPaint["materialName"];
                  string whichFloor = (string)dictRoomPaint["whichFloor"];

                  Debug.Log(gameObjectName + " " + materialName);


                  for (int i = 0; i < objectsPrefabStorage.paintMaterials.Length; i++)
                  {
                      if (objectsPrefabStorage.paintMaterials[i].GetComponent<ARObject>().ObjectName == materialName)
                      {
                          Debug.Log(materialName);
                          ReplacePaintWith(gameObjectName, objectsPrefabStorage.roomPaintMaterials[i], materialName, whichFloor);
                      }
                  }
              }

          }
      });
    }

    private void ReplacePaintWith(string gameObjectName, Material material, string materialName, string whichFloor)
    {
        string tranformHierachy = "";
        if (whichFloor == "GroundFloor")
            tranformHierachy = "GroundFloor/Paints/";
        else if (whichFloor == "FirstFloor")
            tranformHierachy = "FirstFloor/Paints/";

        GameObject room = aRModel.transform.Find(tranformHierachy + gameObjectName).gameObject;
        if (room == null)
            Debug.Log("Room is null");
        foreach (Transform wall in room.transform)
        {
            WallPaint[] wallPaints = wall.GetComponentsInChildren<WallPaint>();
            foreach (WallPaint wallPaint in wallPaints)
            {
                wallPaint.GetComponent<Renderer>().material = material;
            }
        }

        room.GetComponent<RoomPaint>().materialName = materialName;
    }

    private void LoadAllRoomFloors()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("users").Child(AuthManager.Instance.user.UserId).Child("rooms").Child(MainManager.Instance.roomKey).Child("roomFloors")
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

              foreach (DataSnapshot roomFloor in snapshot.Children)
              {
                  IDictionary dictRoomFloor = (IDictionary)roomFloor.Value;

                  string gameObjectName = (string)dictRoomFloor["gameObjectName"];
                  string materialName = (string)dictRoomFloor["materialName"];
                  string whichFloor = (string)dictRoomFloor["whichFloor"];

                  Debug.Log(gameObjectName + " " + materialName);


                  for (int i = 0; i < objectsPrefabStorage.floorMaterials.Length; i++)
                  {
                      if (objectsPrefabStorage.floorMaterials[i].GetComponent<ARObject>().ObjectName == materialName)
                      {
                          ReplaceFloorWith(gameObjectName, objectsPrefabStorage.roomFloorMaterials[i], materialName, whichFloor);
                      }
                  }
              }

          }
      });
    }

    private void ReplaceFloorWith(string gameObjectName, Material material, string materialName, string whichFloor)
    {
        string tranformHierachy = "";
        if (whichFloor == "GroundFloor")
            tranformHierachy = "GroundFloor/Floors/";
        else if (whichFloor == "FirstFloor")
            tranformHierachy = "FirstFloor/Floors/";

        GameObject room = aRModel.transform.Find(tranformHierachy + gameObjectName).gameObject;
        foreach (Transform floor in room.transform)
        {
            floor.GetComponent<Renderer>().material = material;
        }

        room.GetComponent<RoomFloor>().materialName = materialName;
    }
}
