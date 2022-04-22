using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{

    #region Network Variables

    private PhotonView myPhotonView;
    public RoomManager roomManager;
    private ARManager aRManager;
    public ObjectsPrefabStorage objectsPrefabStorage;

    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUiPrefab;

    public GameObject _uiGo;
    public Color myPlayerColor;

    public GameObject myCurrentSelectedObject = null;

    private Quaternion previousModelRotation = Quaternion.Euler(0, 0, 0);
    private Vector3 previousModelScaling = Vector3.zero;

    private Vector3 previousPosition = Vector3.zero;
    private Quaternion previousRotation = Quaternion.Euler(0, 0, 0);

    #endregion


    #region MonoBehaviorCallBacks


    private void Start()
    {

        /// Network variables/gameobjects initialization
        myPhotonView = GetComponent<PhotonView>();
        roomManager = GameObject.Find("/RoomManager").GetComponent<RoomManager>();
        roomManager.allPhotonViews.Add(myPhotonView);

        aRManager = GameObject.Find("Canvas").GetComponent<ARManager>();
        objectsPrefabStorage = GameObject.Find("ObjectsPrefabStorage").GetComponent<ObjectsPrefabStorage>();

        if (PlayerUiPrefab != null)
        {
            _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        InitializePlayerColor();
    }

    private void InitializePlayerColor()
    {
        float hue = UnityEngine.Random.Range(0.0f, 1.0f);
        myPlayerColor = Color.HSVToRGB(hue, 1, 1);
    }


    private void Update()
    {
        if (photonView.IsMine)
        {
            if (CheckIfTheARModelExistAndARModelIsActiveInHierarchy() && Lean.Touch.LeanTouch.Fingers.Count == 2)
            {
                if (CheckIfThisPlayerRotateOrScalingTheARModel())
                    EmitRotateAndScaleTheARModel(aRManager.aRModel.transform.rotation, aRManager.aRModel.transform.localScale);
            }

            if (myCurrentSelectedObject != null)
            {
                if (CheckIfThisPlayerMovingTheObject())
                    EmitMoveTheObject(myCurrentSelectedObject.transform.position - previousPosition);

                if (CheckIfThisPlayerRotatingTheObject())
                    EmitRotateTheObject(myCurrentSelectedObject.transform.rotation);
            }
        }
    }

    #endregion


    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Debug.Log("NetworkManager: Sending");
        }
        else
        {
            //Debug.Log("NetworkManager: Receiving");
        }
    }

    bool CheckIfTheARModelExistAndARModelIsActiveInHierarchy ()
    {
        return aRManager.aRModel.activeInHierarchy && aRManager.aRModel != null;
    }

    bool CheckIfThisPlayerRotateOrScalingTheARModel ()
    {
        return previousModelRotation != aRManager.aRModel.transform.rotation || previousModelScaling != aRManager.aRModel.transform.localScale;
    }

    bool CheckIfThisPlayerMovingTheObject()
    {
        return previousPosition != myCurrentSelectedObject.transform.position && Lean.Touch.LeanTouch.Fingers.Count == 1;
    }

    bool CheckIfThisPlayerRotatingTheObject()
    {
        return previousRotation != myCurrentSelectedObject.transform.rotation && Lean.Touch.LeanTouch.Fingers.Count == 2;
    }

    public void EmitSyncWithHost(string aRModelName)
    {
        photonView.RPC("SyncWithHost", RpcTarget.Others, aRModelName);
    }

    public void EmitRotateAndScaleTheARModel (Quaternion modelRotation, Vector3 modelScale)
    {
        photonView.RPC("RotateAndScaleARModel", RpcTarget.Others, modelRotation, modelScale);
        previousModelRotation = aRManager.aRModel.transform.rotation;
        previousModelScaling = aRManager.aRModel.transform.localScale;
    }

    public void EmitSelectObject(GameObject selectedGameObject)
    {
        previousPosition = selectedGameObject.transform.position;
        photonView.RPC("ISelectedAnObject", RpcTarget.All, selectedGameObject.name);
    }

    public void EmitMoveTheObject(Vector3 travelDistance)
    {
        previousPosition = myCurrentSelectedObject.transform.position;
        photonView.RPC("IMovedTheObject", RpcTarget.Others, travelDistance);
    }

    public void EmitRotateTheObject(Quaternion objectRotation)
    {
        previousRotation = myCurrentSelectedObject.transform.rotation;
        photonView.RPC("IRotatedTheObject", RpcTarget.Others, objectRotation);
    }

    public void EmitChangeTheObjectModelTo(int itemIndex, string objectTag)
    {
        photonView.RPC("IChangeTheObjectModelTo", RpcTarget.All, itemIndex, objectTag);
    }

    public void EmitDeselectObject()
    {
        photonView.RPC("IDeselectedTheObject", RpcTarget.All, myCurrentSelectedObject.name);
        previousPosition = Vector3.zero;
    }


    #endregion


    #region Pun RPC


    [PunRPC]
    void SyncWithHost(string roomARModelName)
    {
        GameObject[] aRModelPrefabs = MainManager.Instance.aRModelPrefabs;
        foreach (GameObject aRModelPrefab in aRModelPrefabs)
        {
            if (aRModelPrefab.GetComponent<ARModel>().ModelName == roomARModelName)
            {
                Debug.Log("SocketManager: Set my selectedARModelPrefab To " + aRModelPrefab.GetComponent<ARModel>().ModelName);
                MainManager.Instance.selectedARModelPrefab = aRModelPrefab;
            }
        }
    }

    [PunRPC]
    void RotateAndScaleARModel(Quaternion modelRotation, Vector3 modelScale)
    {
        aRManager.aRModel.transform.rotation = modelRotation;
        aRManager.aRModel.transform.localScale = modelScale;
    }

    [PunRPC]
    void ISelectedAnObject(string objectName)
    {
        GameObject theSelectedObject = GameObject.Find(objectName);
        myCurrentSelectedObject = theSelectedObject;

        _uiGo.GetComponent<PlayerUI>().Show();
        _uiGo.GetComponent<PlayerUI>().SetPositionToSelectedObject();

        TriggerOutline(myCurrentSelectedObject, true);
    }


    [PunRPC]
    void IMovedTheObject(Vector3 travelDistance)
    {
        Vector3 myCurrentObjectPosition = myCurrentSelectedObject.transform.position;

        myCurrentSelectedObject.transform.position = new Vector3(myCurrentObjectPosition.x + travelDistance.x, myCurrentObjectPosition.y, myCurrentObjectPosition.z + travelDistance.z);
    }

    [PunRPC]
    void IRotatedTheObject(Quaternion objectRotation)
    {
        myCurrentSelectedObject.transform.rotation = objectRotation;
    }

    [PunRPC]
    void IChangeTheObjectModelTo(int itemIndex, string objectTag)
    {
        if (myCurrentSelectedObject == null)
            return;

        //debug1.text = Time.fixedTime + ": PlayerManager: IChange: index is " + itemIndex + " with tag " + objectTag;
        switch (objectTag)
        {
            case "Bed":
                ReplaceObjectWith(objectsPrefabStorage.bedPrefabs[itemIndex]);
                break;
            case "Sofa":
                ReplaceObjectWith(objectsPrefabStorage.sofaPrefabs[itemIndex]);
                break;
            case "Rack":
                ReplaceObjectWith(objectsPrefabStorage.rackPrefabs[itemIndex]);
                break;
            case "Cabinet":
                ReplaceObjectWith(objectsPrefabStorage.cabinetPrefabs[itemIndex]);
                break;
            case "TV Table":
                ReplaceObjectWith(objectsPrefabStorage.tvTablePrefabs[itemIndex]);
                break;
            case "Coffee Table":
                ReplaceObjectWith(objectsPrefabStorage.coffeeTablePrefabs[itemIndex]);
                break;
            case "Lamp":
                ReplaceObjectWith(objectsPrefabStorage.lampPrefabs[itemIndex]);
                break;
            case "Office Table":
                ReplaceObjectWith(objectsPrefabStorage.officeTablePrefabs[itemIndex]);
                break;
            case "Chair":
                ReplaceObjectWith(objectsPrefabStorage.chairPrefabs[itemIndex]);
                break;
            case "Kitchen Chair":
                ReplaceObjectWith(objectsPrefabStorage.kitchenChairPrefabs[itemIndex]);
                break;
            case "Kitchen Table":
                ReplaceObjectWith(objectsPrefabStorage.kitchenTablePrefabs[itemIndex]);
                break;
            case "Kitchen Shelf":
                ReplaceObjectWith(objectsPrefabStorage.kitchenShelfPrefabs[itemIndex]);
                break;
            case "Modular Kitchen Table":
                ReplaceObjectWith(objectsPrefabStorage.modularKitchenTablePrefabs[itemIndex]);
                break;
            case "Washbasin":
                ReplaceObjectWith(objectsPrefabStorage.washbasinPrefabs[itemIndex]);
                break;
            case "Paint":
                ReplacePaintWith(objectsPrefabStorage.paintMaterials[itemIndex].GetComponent<Renderer>().material);
                break;
            case "Floor":
                ReplaceFloorWith(objectsPrefabStorage.floorMaterials[itemIndex].GetComponent<Renderer>().material);
                break;
        }
    }

    void ReplaceObjectWith(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, myCurrentSelectedObject.transform.position, myCurrentSelectedObject.transform.rotation, myCurrentSelectedObject.transform.parent);
        newObject.name = myCurrentSelectedObject.name;

        Destroy(myCurrentSelectedObject);
    }

    private void ReplacePaintWith(Material material)
    {
        GameObject room = myCurrentSelectedObject.transform.parent.gameObject;
        foreach (Transform wall in room.transform)
        {
            WallPaint[] wallPaints = wall.GetComponentsInChildren<WallPaint>();
            foreach (WallPaint wallPaint in wallPaints)
            {
                wallPaint.GetComponent<Renderer>().material = material;
            }
        }
    }

    private void ReplaceFloorWith(Material material)
    {
        GameObject room = myCurrentSelectedObject.transform.parent.gameObject;
        foreach (Transform floor in room.transform)
        {
            floor.GetComponent<Renderer>().material = material;
        }
    }

    [PunRPC]
    void IDeselectedTheObject(string objectName)
    {
        GameObject theSelectedObject = GameObject.Find(objectName);
        TriggerOutline(theSelectedObject, false);

        myCurrentSelectedObject = null;

        _uiGo.GetComponent<PlayerUI>().Hide();
    }


    private void TriggerOutline(GameObject selectedObject, bool isEnabled)
    {
        if (selectedObject.CompareTag("Paint") || selectedObject.CompareTag("Floor"))
        {
            GameObject room = selectedObject.transform.parent.gameObject;
            Outline[] childrenOutline = room.GetComponentsInChildren<Outline>();
            foreach (Outline outline in childrenOutline)
            {
                outline.enabled = isEnabled;
                outline.OutlineColor = myPlayerColor;
            }
        }
        else
        {
            selectedObject.GetComponent<Outline>().enabled = isEnabled;
            selectedObject.GetComponent<Outline>().OutlineColor = myPlayerColor;
        }
    }

    #endregion
}
