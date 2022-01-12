using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomButtonInfo : MonoBehaviour
{
    public TMP_Text roomName = null;
    public TMP_Text mapName = null;
    public TMP_Text playersCounter = null;

    public Button joinRoomButton = null;
    public Button leaveRoomButton = null;

    public bool hasBeenClicked = false;

    //public void LeaveRoom(GameObject gameRoomGameObject)
    //{
    //    // if (PhotonNetwork.CurrentRoom == null)
    //    //     return;
    //    //
    //    // // need to add functionality here to leave a room you maybe in or hosting
    //    // Debug.Log("Connection status: " + PhotonNetwork.CurrentRoom);
    //    //
    //    // PhotonNetwork.LeaveRoom();
    //    //
    //    // Destroy(gameRoomGameObject);
    //}

    //public void LeaveRoomList()
    //{
    //    // if (PhotonNetwork.CurrentRoom == null)
    //    //     return;
    //    //
    //    // PhotonNetwork.LeaveRoom();
    //}
}