using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class LobbyManagement : MonoBehaviourPunCallbacks
{
    public TMP_InputField joinInput, createInput;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        Debug.Log($"{joinInput.text}");
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Main");
    }
}