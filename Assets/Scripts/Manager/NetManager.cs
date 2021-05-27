using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetManager : MonoBehaviourPunCallbacks
{
    public static NetManager instance;
    private PhotonView PV;

    public List<Player> diedUsers = new List<Player>();
    public Dictionary<int, Player> idToPlayer = new Dictionary<int, Player>();

    private void Awake()
    {
        instance = this;
        PV = photonView;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        PhotonNetwork.LocalPlayer.NickName = GameManager.Instance.savedData.userInfo.nickName;
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message) => CreateRoom();
    public override void OnCreateRoomFailed(short returnCode, string message) => CreateRoom();
    public override void OnDisconnected(DisconnectCause cause)
    {

    }
    public override void OnJoinedRoom()
    {
        for(int i=0; i<PhotonNetwork.PlayerList.Length; i++)
        {
            idToPlayer.Add(PhotonNetwork.PlayerList[i].ActorNumber, PhotonNetwork.PlayerList[i]);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
       
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(Random.Range(0, 10000).ToString(), new RoomOptions { MaxPlayers = 18 });
    }
    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }
}
