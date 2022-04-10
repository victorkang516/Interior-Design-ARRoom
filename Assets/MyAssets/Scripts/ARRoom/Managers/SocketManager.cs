using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SocketManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [HideInInspector] public GameObject mySelectedObject;
    GameObject someonesSelectedObject;

    [HideInInspector] public bool someoneEnteredRoom = false;

    bool onFirstTimeSelect = true;
    Vector3 currentPosition = Vector3.zero;
    Quaternion currentRotation = Quaternion.Euler(0, 0, 0);


    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            if (someoneEnteredRoom)
            {
                EmitSyncWithHost(MainManager.Instance.selectedARModelPrefab.GetComponent<ARModel>().ModelName);
            }

            // We own this player: send the others our data
            if (mySelectedObject != null)
            {
                if (onFirstTimeSelect)
                {
                    EmitObjectName(mySelectedObject.name);
                }

                if (CheckIfThisPlayerMovingTheObject())
                {
                    EmitMoveTheObject(mySelectedObject.transform.position);
                }

                if (CheckIfThisPlayerRotatingTheObject())
                {
                    EmitRotateTheObject(mySelectedObject.transform.rotation);
                }
            }
            else if (mySelectedObject == null && onFirstTimeSelect == false)
            {
                photonView.RPC("SomeoneDeselectedTheObject", RpcTarget.Others);
                onFirstTimeSelect = true;
                currentPosition = Vector3.zero;
            }
        }
        else
        {
            // Network player, receive data
            // this.objectPosition = (Vector3)stream.ReceiveNext();
            Debug.Log("NetworkManager: Receiving");
        }
    }

    bool CheckIfThisPlayerMovingTheObject()
    {
        return currentPosition != mySelectedObject.transform.position && Lean.Touch.LeanTouch.Fingers.Count == 2;
    }

    bool CheckIfThisPlayerRotatingTheObject()
    {
        return currentRotation != mySelectedObject.transform.rotation && Lean.Touch.LeanTouch.Fingers.Count == 3;
    }

    void EmitSyncWithHost(string aRModelName)
    {
        Debug.Log("SocketManager: Emit SyncWithHost" + aRModelName);
        photonView.RPC("SyncWithHost", RpcTarget.Others, aRModelName);
        someoneEnteredRoom = false;
    }

    void EmitObjectName(string objectName)
    {
        photonView.RPC("SomeoneSelectedAnObject", RpcTarget.Others, objectName);
        onFirstTimeSelect = false;
    }

    void EmitMoveTheObject(Vector3 objectPosition)
    {
        photonView.RPC("SomeoneMovingTheObject", RpcTarget.Others, objectPosition);
        currentPosition = mySelectedObject.transform.position;
    }

    void EmitRotateTheObject(Quaternion objectRotation)
    {
        photonView.RPC("SomeoneRotatingTheObject", RpcTarget.Others, objectRotation);
        currentRotation = mySelectedObject.transform.rotation;
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
    void SomeoneSelectedAnObject(string name)
    {
        someonesSelectedObject = GameObject.Find(name);
        someonesSelectedObject.GetComponent<Outline>().enabled = true;
    }

    [PunRPC]
    void SomeoneMovingTheObject(Vector3 objectPosition)
    {
        someonesSelectedObject.transform.position = objectPosition;
    }

    [PunRPC]
    void SomeoneRotatingTheObject(Quaternion objectRotation)
    {
        someonesSelectedObject.transform.rotation = objectRotation;
    }

    [PunRPC]
    void SomeoneDeselectedTheObject()
    {
        someonesSelectedObject.GetComponent<Outline>().enabled = false;
        someonesSelectedObject = null;
    }


    #endregion
}
