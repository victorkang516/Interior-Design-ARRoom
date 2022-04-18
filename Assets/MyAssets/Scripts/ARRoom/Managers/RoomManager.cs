using System.Collections.Generic;


using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


using Photon.Pun;
using Photon.Realtime;


public class RoomManager : MonoBehaviourPunCallbacks
{


    #region

    ARModificationManager aRModificationManager;


    public List<PhotonView> allPhotonViews;
    public Color[] playerColors = { Color.gray, Color.green, Color.cyan, Color.red, Color.yellow };


    public GameObject playerManagerPrefab;
    PlayerManager playerManager;

    Button exitButton;

    // Temporary here for implementation
    public GameObject[] sofaPrefabs;
    public GameObject[] bedPrefabs;


    #endregion


    #region MonoBehaviour Callbacks

    private void Start()
    {
        aRModificationManager = GameObject.Find("ARModificationMode").GetComponent<ARModificationManager>();

        exitButton = GameObject.Find("/Canvas/ARBasicMode/TopLeftPanel/ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(LeaveRoom);

        if (playerManagerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            playerManager = PhotonNetwork.Instantiate(playerManagerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0).GetComponent<PlayerManager>();
            aRModificationManager.playerManager = playerManager;
        }
    }

    #endregion


    #region Private Methods


    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("ARRoomScene");
    }


    #endregion


    #region Photon Callbacks


    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        playerManager.someoneEnteredRoom = true;

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }


    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
