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
    private ARManager aRManager;
    private RoomManager roomManager;

    private GameObject aRModel;

    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUiPrefab;

    public GameObject _uiGo;
    public Color myPlayerColor;

    public bool someoneEnteredRoom = false;

    public GameObject myPreviousSelectedObject = null;
    public GameObject myCurrentSelectedObject = null;

    public bool isDeselected = true;


    private Quaternion previousModelRotation = Quaternion.Euler(0, 0, 0);
    private Vector3 previousModelScaling = Vector3.zero;

    private Vector3 previousPosition = Vector3.zero;
    private Quaternion previousRotation = Quaternion.Euler(0, 0, 0);

    // temporary button
    //private Button item1;
    public Text debug1;
    public Text debug2;

    #endregion


    #region MonoBehaviorCallBacks


    private void Start()
    {

        /// Network variables/gameobjects initialization
        myPhotonView = GetComponent<PhotonView>();

        aRManager = GameObject.Find("Canvas").GetComponent<ARManager>();

        roomManager = GameObject.Find("/RoomManager").GetComponent<RoomManager>();
        roomManager.allPhotonViews.Add(myPhotonView);

        if (PlayerUiPrefab != null)
        {
            _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        // Temporary
        //item1 = GameObject.Find("ChangeModelButton").GetComponent<Button>();
        //item1.onClick.AddListener(ChangeObjectModel);

        InitializePlayerColor();

        debug1 = GameObject.Find("DebugText1").GetComponent<Text>();
        debug2 = GameObject.Find("DebugText2").GetComponent<Text>();

        debug1.text = Time.fixedTime + ": PlayerManager: " + photonView.Owner.NickName;
    }

    private void InitializePlayerColor()
    {
        switch (photonView.ViewID)
        {
            case 1001:
                myPlayerColor = roomManager.playerColors[1];
                break;
            case 2001:
                myPlayerColor = roomManager.playerColors[2];
                break;
            case 3001:
                myPlayerColor = roomManager.playerColors[3];
                break;
            case 4001:
                myPlayerColor = roomManager.playerColors[4];
                break;
            default:
                myPlayerColor = roomManager.playerColors[0];
                break;
        }
    }

    
    #endregion


    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            if (someoneEnteredRoom)
            {
                EmitSyncWithHost(MainManager.Instance.selectedARModelPrefab.GetComponent<ARModel>().ModelName);
            }


            if (aRManager.aRModel.activeInHierarchy && aRManager.aRModel != null && Lean.Touch.LeanTouch.Fingers.Count == 2)
            {
                if (CheckIfThisPlayerRotateAndScalingTheARModel())
                    EmitRotateAndScaleTheARModel(aRManager.aRModel.transform.rotation, aRManager.aRModel.transform.localScale);
            }
   

            // We own this player: send the others our data
            if (myCurrentSelectedObject != null)
            {

                if (myCurrentSelectedObject != myPreviousSelectedObject || isDeselected == true)
                    EmitSelectObject(myCurrentSelectedObject.name);

                if (CheckIfThisPlayerMovingTheObject())
                    EmitMoveTheObject(myCurrentSelectedObject.transform.position-previousPosition);

                if (CheckIfThisPlayerRotatingTheObject())
                    EmitRotateTheObject(myCurrentSelectedObject.transform.rotation);

                //if (desiredObjectPrefab != null)
                //    EmitChangeTheObjectModelTo(desiredObjectPrefab.name, desiredObjectPrefab.tag);
            }
            else if (myCurrentSelectedObject == null && isDeselected == false)
            {
                EmitDeselectObject(myPreviousSelectedObject.name);
            }
        }
        else
        {
            Debug.Log("NetworkManager: Receiving");
        }
    }

    bool CheckIfThisPlayerRotateAndScalingTheARModel ()
    {
        if (previousModelRotation != aRManager.aRModel.transform.rotation || previousModelScaling != aRManager.aRModel.transform.localScale)
            return true;
        
        return false;
    }

    bool CheckIfThisPlayerMovingTheObject()
    {
        return previousPosition != myCurrentSelectedObject.transform.position && Lean.Touch.LeanTouch.Fingers.Count == 0;
    }

    bool CheckIfThisPlayerRotatingTheObject()
    {
        return previousRotation != myCurrentSelectedObject.transform.rotation && Lean.Touch.LeanTouch.Fingers.Count == 2;
    }

    void EmitSyncWithHost(string aRModelName)
    {
        Debug.Log("SocketManager: Emit SyncWithHost" + aRModelName);
        photonView.RPC("SyncWithHost", RpcTarget.Others, aRModelName);
        someoneEnteredRoom = false;
    }

    void EmitRotateAndScaleTheARModel (Quaternion modelRotation, Vector3 modelScale)
    {
        photonView.RPC("RotateAndScaleARModel", RpcTarget.Others, modelRotation, modelScale);
        previousModelRotation = aRManager.aRModel.transform.rotation;
        previousModelScaling = aRManager.aRModel.transform.localScale;
    }

    void EmitSelectObject(string objectName)
    {
        previousPosition = myCurrentSelectedObject.transform.position;
        photonView.RPC("ISelectedAnObject", RpcTarget.All, objectName);
    }

    void EmitMoveTheObject(Vector3 travelDistance)
    {
        photonView.RPC("IMovedTheObject", RpcTarget.Others, travelDistance);
        previousPosition = myCurrentSelectedObject.transform.position;
    }

    void EmitRotateTheObject(Quaternion objectRotation)
    {
        photonView.RPC("IRotatedTheObject", RpcTarget.All, objectRotation);
        previousRotation = myCurrentSelectedObject.transform.rotation;
    }

    //void EmitChangeTheObjectModelTo(string desiredObjectName, string objectTag)
    //{
    //    photonView.RPC("IChangeTheObjectModelTo", RpcTarget.All, desiredObjectName, objectTag);
    //}

    void EmitDeselectObject(string objectName)
    {
        photonView.RPC("IDeselectedTheObject", RpcTarget.All, objectName);
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

        //foreach (PhotonView photonView in roomManager.allPhotonViews)
        //{
        //    if (photonView != myPhotonView)
        //    {
        //        if (photonView.GetComponent<PlayerManager>().myCurrentSelectedObject == myCurrentSelectedObject)
        //        {
        //            Debug.Log(photonView.ViewID + " Call DeselectedObject");
        //            photonView.GetComponent<PlayerManager>().EmitDeselectObject(myCurrentSelectedObject.name);
        //        }
        //    }
        //}

        StartCoroutine(WaitAWhileThenShowOutline());

        _uiGo.GetComponent<PlayerUI>().Show();
        _uiGo.GetComponent<PlayerUI>().SetPositionToSelectedObject();

        isDeselected = false;
        myPreviousSelectedObject = myCurrentSelectedObject;


    }
    IEnumerator WaitAWhileThenShowOutline()
    {
        yield return new WaitForSeconds(0.5f);

        if (myPreviousSelectedObject != null)
            TriggerOutline(myPreviousSelectedObject, false);

        TriggerOutline(myCurrentSelectedObject, true);
    }


    [PunRPC]
    void IMovedTheObject(Vector3 travelDistance)
    {
        Vector3 newPosition = new Vector3(myCurrentSelectedObject.transform.position.x + travelDistance.x, myCurrentSelectedObject.transform.position.y, myCurrentSelectedObject.transform.position.z + travelDistance.z);
        myCurrentSelectedObject.transform.position = newPosition;
    }

    //[PunRPC]
    //void IMovedTheObject(Vector3 objectPosition, Quaternion modelRotation)
    //{
    //    Vector3 positionAfterRotation = CalculateVectorAfterRotation(objectPosition, modelRotation);
    //    debug1.text = Time.fixedTime + ": PlayerManager: positionBefore " + objectPosition + "" + " positionAfter " + positionAfterRotation;

    //    Vector3 newPosition = new Vector3(myCurrentSelectedObject.transform.position.x + positionAfterRotation.x, myCurrentSelectedObject.transform.position.y, myCurrentSelectedObject.transform.position.z + positionAfterRotation.z);
    //    myCurrentSelectedObject.transform.position = newPosition;

        
    //}

    Vector3 CalculateVectorAfterRotation(Vector3 objectPosition, Quaternion modelRotation)
    {
        double angleInRadian = (Quaternion.Angle(aRManager.aRModel.transform.rotation, modelRotation) * Math.PI) / 180;
        float x2 = (float)( (Math.Cos(angleInRadian) * objectPosition.x) - (Math.Sin(angleInRadian) * objectPosition.z) );
        float z2 = (float)( (Math.Sin(angleInRadian) * objectPosition.x) + (Math.Cos(angleInRadian) * objectPosition.z) );
        debug2.text = Time.fixedTime + ": PlayerManager: Angle Radian Between is " + angleInRadian + " x2 & z2 are " + x2 + "&" + z2;
        return new Vector3(x2, 0, z2);
    }

    [PunRPC]
    void IRotatedTheObject(Quaternion objectRotation)
    {
        myCurrentSelectedObject.transform.rotation = objectRotation;
    }

    //[PunRPC]
    //void IChangeTheObjectModelTo(string desiredObjectName, string objectTag)
    //{
    //    switch (objectTag)
    //    {
    //        case "Sofa":
    //            foreach (GameObject sofa in roomManager.sofaPrefabs)
    //            {
    //                if (sofa.name == desiredObjectName)
    //                {
    //                    GameObject newObject = Instantiate(sofa, myCurrentSelectedObject.transform.position, myCurrentSelectedObject.transform.rotation, myCurrentSelectedObject.transform.parent);
    //                    newObject.name = myCurrentSelectedObject.name;

    //                    myPreviousSelectedObject = newObject;

    //                    Destroy(myCurrentSelectedObject);
    //                    desiredObjectPrefab = null;
    //                }
    //            }
    //            break;
    //    }
    //}

    [PunRPC]
    void IDeselectedTheObject(string objectName)
    {
        GameObject theSelectedObject = GameObject.Find(objectName);
        TriggerOutline(theSelectedObject, false);

        myCurrentSelectedObject = null;
        isDeselected = true;

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
