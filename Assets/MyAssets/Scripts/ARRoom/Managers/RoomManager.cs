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
    ObjectListHandler objectListHandler;


    public List<PhotonView> allPhotonViews;

    public GameObject playerManagerPrefab;
    public PlayerManager playerManager;


    public GameObject roomMemberItemPrefab;

    GameObject roomPanel;
    RoomPanelHandler roomPanelHandler;
    Button bottomPanel;
    Button exitButton;
    Text roomNameText;

    RoomMessageBoxHandler roomMessageBoxHandler;


    #endregion


    #region MonoBehaviour Callbacks

    private void Start()
    {
        aRModificationManager = GameObject.Find("ARModificationMode").GetComponent<ARModificationManager>();
        objectListHandler = GameObject.Find("/Canvas/ARModificationMode/ObjectListPanel/Scroll/Panel").GetComponent<ObjectListHandler>();

        roomPanel = GameObject.Find("/Canvas/ARBasicMode/RoomPanel");
        roomPanelHandler = roomPanel.GetComponent<RoomPanelHandler>();

        bottomPanel = GameObject.Find("/Canvas/ARBasicMode/RoomPanel/Bottom").GetComponent<Button>();
        bottomPanel.onClick.AddListener(ShowRoomPanel);

        roomNameText = GameObject.Find("/Canvas/ARBasicMode/RoomPanel/Bottom/RoomNameText").GetComponent<Text>();
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        exitButton = GameObject.Find("/Canvas/ARBasicMode/TopLeftPanel/ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(LeaveRoom);

        roomMessageBoxHandler = GameObject.Find("/Canvas/ARBasicMode/RoomMessageBox").GetComponent<RoomMessageBoxHandler>();

        if (playerManagerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            playerManager = PhotonNetwork.Instantiate(playerManagerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0).GetComponent<PlayerManager>();
            aRModificationManager.playerManager = playerManager;
            objectListHandler.playerManager = playerManager;
        }
    }

    #endregion


    #region Private Methods

    void RefreshRoomPanelList()
    {
        foreach (Transform child in roomPanel.transform.GetChild(0).GetChild(0))
        {
            Destroy(child.gameObject);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject roomMemberItem = Instantiate(roomMemberItemPrefab, roomPanel.transform.GetChild(0).GetChild(0));
            roomMemberItem.transform.GetChild(0).GetComponent<Text>().text = player.NickName;
            roomMemberItem.name = player.NickName;
        }

    }

    #endregion


    #region Photon Callbacks


    public override void OnPlayerEnteredRoom(Player other)
    {
        roomMessageBoxHandler.onPlayerJoined(other.NickName);
        RefreshRoomPanelList();

        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        
        if (PhotonNetwork.IsMasterClient)
            playerManager.EmitSyncWithHost(MainManager.Instance.selectedARModelPrefab.GetComponent<ARModel>().ModelName);


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        roomMessageBoxHandler.onPlayerLeft(other.NickName);
        RefreshRoomPanelList();

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
        SceneManager.LoadScene(1);
    }


    #endregion


    #region Public Methods


    public void ShowRoomPanel ()
    {
        roomPanelHandler.Trigger();
        RefreshRoomPanelList();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
