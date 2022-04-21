using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using TMPro;

namespace Photon101.HW
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants
        const string _playerNamePrefKey = "PlayerName";
        #endregion

        #region MonoBehaicior CallBacks
        private void Start()
        {
            string defaultName = string.Empty;
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            if(inputField!=null)
            {
                if (PlayerPrefs.HasKey(_playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(_playerNamePrefKey);
                    inputField.text = defaultName;
                }
            }
            PhotonNetwork.NickName = defaultName;
        }
        #endregion
        #region
        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;
            Debug.Log($"your nickname is: {PhotonNetwork.NickName}");
            PlayerPrefs.SetString(_playerNamePrefKey, value);
        }
        #endregion
    }
}
