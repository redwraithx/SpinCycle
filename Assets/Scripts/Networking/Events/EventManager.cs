using System.Collections.Generic;
using ExitGames.Client.Photon;
using NetworkProfile;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NetworkEvent
{
    public class PlayerInfo
    {
        public ProfileData profile;
        public int netID;
        public int gameScore;
        public bool isOnTeamTwo;

        public PlayerInfo(ProfileData pData, int id, int gameScore, bool isOnTeamTwo)
        {
            profile = pData;

            netID = id;
            this.gameScore = gameScore;
            this.isOnTeamTwo = isOnTeamTwo;
        }
    }

    public class EventManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public List<PlayerInfo> playerInfo = new List<PlayerInfo>();

        #region Event_Codes

        public enum EventCodes : byte
        {
            TestEvent3 = 197,
            TestEvent2 = 198,
            TestEvent = 199
        }

        #endregion Event_Codes

        #region PhotonCode

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code >= 200)
                return;

            EventCodes eventCode = (EventCodes)photonEvent.Code;
            object[] obj = (object[])photonEvent.CustomData;

            switch (eventCode)
            {
                case EventCodes.TestEvent:
                    {
                        TestEvent_Recv();

                        break;
                    }
                case EventCodes.TestEvent2:
                    {
                        TestEvent_2_Recv(obj);

                        break;
                    }
                case EventCodes.TestEvent3:
                    {
                        TestEvent_3_Recv(obj);

                        break;
                    }
            }
        }

        #endregion PhotonCode

        private void TestEvent_Recv()
        {
            Debug.Log("TestEvent Received");

            UpdateTestEvent_Send();
        }

        private void TestEvent_2_Recv(object[] data)
        {
            PlayerInfo player = new PlayerInfo
            (
                new ProfileData
                (
                    (string)data[0],   // username
                    (bool)data[1],     // played tutorial?
                    (float)data[2],    // music volume
                    (float)data[3],     // sfx volume
                    (int)data[4],       // high score
                    (int)data[5],       // games won
                    (int)data[6],       // games lost
                    (int)data[7],       // game draws
                    (int)data[8]       // incomplete games - left for some reason, quit_internet_etc...

                ),
                (int)data[9],
                (int)data[10],
                (bool)data[11]

            );

            playerInfo.Add(player);

            // resync
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
            {
                // sync local players data if needed
            }

            // update all clients
            UpdateTestEvent_2_Send(playerInfo);
        }

        private void TestEvent_3_Recv(object[] data)
        {
            string receivedMessage = (string)data[0];

            Debug.Log("TestEvent 3 Received message from: " + receivedMessage);

            UpdateTextEvent_3_Send(PhotonNetwork.NickName);
        }

        public void UpdateTestEvent_Send()
        {
            Debug.Log("Sending update");

            PhotonNetwork.RaiseEvent
            (
                (byte)EventCodes.TestEvent,
                null,
                new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.All
                },
                new SendOptions
                {
                    Reliability = true
                }
            );
        }

        public void UpdateTestEvent_2_Send(List<PlayerInfo> info)
        {
            object[] package = new object[info.Count + 1];

            for (int i = 0; i < info.Count; i++)
            {
                object[] piece = new object[4];

                piece[0] = info[i].profile.userName;
                piece[1] = info[i].profile.hasPlayedTutorial;
                piece[2] = info[i].netID;
                piece[3] = info[i].gameScore;
                piece[4] = info[i].isOnTeamTwo;

                package[i + 1] = piece;
            }

            PhotonNetwork.RaiseEvent
            (
                (byte)EventCodes.TestEvent2,
                package,
                new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.All
                },
                new SendOptions
                {
                    Reliability = true
                }
            );
        }

        public void UpdateTextEvent_3_Send(string message)
        {
            Debug.Log("Sending message from " + message);

            object[] package = new object[1];

            package[0] = message;

            PhotonNetwork.RaiseEvent
            (
                (byte)EventCodes.TestEvent3,
                package,
                new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.All
                },
                new SendOptions
                {
                    Reliability = true
                }
            );
        }

        // // player class sync local player EXAMPLES
        // public void TrySync()
        // {
        //     if (!photonView.IsMine)
        //         return;
        //
        //     photonView.RPC("SyncProfile", RpcTarget.All, Launcher.myProfile.username, Launcher.myProfile.level, Launcher.myProfile.xp);
        //
        //     if (GameSettings.GameMode == GameMode.TDM)
        //     {
        //         photonView.RPC("SyncTeam", RpcTarget.All, GameSettings.IsAwayTeam);
        //     }
        // }

        // [PunRPC]
        // private void SyncProfile(string p_username, int p_level, int p_xp)
        // {
        //     playerProfile = new ProfileData(p_username, p_level, p_xp);
        //     playerUsername.text = playerProfile.username;
        // }
        //
        // [PunRPC]
        // private void SyncTeam(bool p_awayTeam)
        // {
        //     awayTeam = p_awayTeam;
        //
        //     if (awayTeam)
        //     {
        //         ColorTeamIndicators(Color.red);
        //     }
        //     else
        //     {
        //         ColorTeamIndicators(Color.blue);
        //     }
        // }

        #region Monobehaviors

        // private void OnEnable()
        // {
        //     PhotonNetwork.AddCallbackTarget(this);
        // }
        //
        // private void OnDisable()
        // {
        //     PhotonNetwork.RemoveCallbackTarget(this);
        // }

        #endregion Monobehaviors
    }
}