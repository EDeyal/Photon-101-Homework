using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Photon101.HW
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        [Tooltip("The Maximum number of players per room, when the room is full it can't be joined by new players and so new room will be created")]
        [SerializeField]
        private byte _maxPlayerPerRoom = 4;

        [Tooltip("The UI panel to let the user entername, connect and play")]
        [SerializeField]
        private GameObject _controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject _progressLabel;
        #endregion

        #region Private Fields
        string gameVersion = "1";
        bool _isConnecting;
        #endregion
        #region MonoBehaivior CallBacks
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        private void Start()
        {
            _progressLabel.SetActive(false);
            _controlPanel.SetActive(true);
        }
        #endregion
        #region Public Methods
        public void Connect()
        {
            _progressLabel.SetActive(true);
            _controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                _isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        #endregion
        #region MonoBehaviorPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            if (_isConnecting)
            {
                Debug.Log("On Connected to Master was Called by PUN");
                PhotonNetwork.JoinRandomRoom();
                _isConnecting = false;
            }
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            _isConnecting = false;
            if (_progressLabel!=null)
            {
                _progressLabel.SetActive(false);
            }
            if (_controlPanel!=null)
            {
                _controlPanel.SetActive(true);
            }
            Debug.LogWarningFormat("On Disconnected Was Called with Reason{0}", cause);
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed was Called By PUN, No random room available so we create one");
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = _maxPlayerPerRoom });
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom was called now this client is in the room");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");
            }
            PhotonNetwork.LoadLevel("Room for 1");
        }
        #endregion
    }
}
