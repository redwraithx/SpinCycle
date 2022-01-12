using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Networking
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("The maximum number of players per room. when a room is full, it can't be joined by new players. So a new room will be created."), SerializeField] private byte maxPlayersPerRoom = 4;

        // #critical: we failed to join a random room, maybe none exists or they are all full. creating a new room

        #endregion Private Serializable Fields

        #region Private Fields

        [Tooltip("The UI Panel to let the user enter name, connect and play"), SerializeField] private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress"), SerializeField] private GameObject progressLabel;

        private string gameVersion = "1";
        private bool isConnecting = false;

        #endregion Private Fields

        #region MonoBehaviourPunCallBacks Callbacks

        public override void OnConnectedToMaster()
        {
            //Debug.Log("Pun basic launcher OnConnectedToMaster() was called by PUN");

            if (isConnecting)
            {
                // #critical: first we try to do is to join a potential existing room. if there is, good connection, else, well be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            Debug.LogWarningFormat("Pun basic launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            //Debug.Log("PunExtensions NetworkMatch.BasicResponseDelegate Launcher: OnJoinRandomFailed() WaitForSeconds called by PUN. no random room available, so we created one.\nCalling: PhotonNetowrk.CreateRoom");

            // critical if we fail to join a random room, we will create one to join
            //PhotonNetwork.CreateRoom(null, new RoomOptions());
            PhotonNetwork.CreateRoom(null, new RoomOptions
            { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            //Debug.Log("Pun basic Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                // Debug.Log("We load the 'Room for 1'");

                // critical!
                // load the room level
                PhotonNetwork.LoadLevel("LoadingScreen1");
            }
        }

        #endregion MonoBehaviourPunCallBacks Callbacks

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion MonoBehaviour CallBacks

        #region Public Methods

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion Public Methods
    }
}