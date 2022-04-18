using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields

    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    #endregion

    #region Private Fields

    string gameVersion = "1";

    CanvasManager canvasManager;
    ExceptionMessageBoxHandler exceptionMessageBoxHandler;

    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool isConnecting;

    #endregion

    #region MonoBehaviour CallBacks

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        canvasManager = GameObject.Find("/Canvas").GetComponent<CanvasManager>();
        exceptionMessageBoxHandler = GameObject.Find("/Canvas/MessagePage").GetComponent<ExceptionMessageBoxHandler>();

        Connect();
    }

    #endregion

    #region MonoBehaviourPunCallBacks


    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            Debug.Log("PUN: OnConnectedToMaster");
            isConnecting = false;
        }
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        canvasManager.SwitchCanvas(CanvasType.Home);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        canvasManager.SwitchCanvas(CanvasType.Login);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //Debug.Log("PUN Basics Tutorial/Launcher:OnCreateRoomFailed() was called by PUN. The room name you enter has been used by existing room, try another room name");
        exceptionMessageBoxHandler.DisplayMessage("The room name has been used, try another room name.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No such room existing, check if you have enter a incorrect room name, or your host not yet create the room.");
        exceptionMessageBoxHandler.DisplayMessage("Room not found, incorrect room name or the room haven't been create.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'ARRoomScene' ");

            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("ARRoomScene");
        }
    }
    #endregion

    #region Public Methods

    public void Connect()
    {
        canvasManager.SwitchCanvas(CanvasType.Loading);

        if (PhotonNetwork.IsConnected)
        {
            //
        }
        else
        {
            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void CreateARRoom(string roomName)
    {
        Debug.Log("PUN/NetworkManager: Create Room with name " + roomName);
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public void JoinARRoom(string roomName)
    {
        Debug.Log("PUN/NetworkManager: Join Room with room name " + roomName);
        PhotonNetwork.JoinRoom(roomName);
    }

    #endregion

}

