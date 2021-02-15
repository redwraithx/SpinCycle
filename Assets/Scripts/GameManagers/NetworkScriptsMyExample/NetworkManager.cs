using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


namespace Networking
{

    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }


        #region Photon Callbacks

            public override void OnLeftRoom()
            {
                SceneManager.LoadScene(0);
            }

            public override void OnPlayerEnteredRoom(Player other)
            {
                Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                    
                    LoadArena();
                }
            }

            public override void OnPlayerLeftRoom(Player other)
            {
                Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);

                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                    
                    LoadArena();
                }
            }
        
        #endregion // Photon Callbacks
        
        
        
        #region Public Methods

            public void LeaveRoom()
            {
                PhotonNetwork.LeaveRoom();
                
            }
        
        #endregion // Public Methods
        
        
        
        #region Private Methods

            private void LoadArena()
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    Debug.LogError("PhotonNetwork: Trying to load a level but we are not the master client");
                }
                
                Debug.LogFormat("PhotonNetwork: Loading level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

                PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
            }
        
        #endregion // Private Methods
        
        
        
    }

}