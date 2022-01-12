using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Networking
{
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        // store the PlayerPref key to avoid typos
        private const string playerNamePrefKey = "PlayerName";

        #endregion Private Constants

        #region MonoBehaviour CallBacks

        private void Start()
        {
            string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();

            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion MonoBehaviour CallBacks

        #region Public Methods

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                //Debug.LogError("Player Name is null or empty");

                return;
            }

            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

        #endregion Public Methods
    }
}