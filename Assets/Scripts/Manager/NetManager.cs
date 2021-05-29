using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetManager : MonoBehaviourPunCallbacks
{
    public static NetManager instance;
    private PhotonView PV;
    public int id;
    public Player p;
    private PlayerScript player;

    public List<Player> diedUsers = new List<Player>();
    public Dictionary<int, Player> idToPlayer = new Dictionary<int, Player>();

    #region UI
    public InputField chatInput;
    public GameObject chatPanel;
    public Text chatText;
    public Scrollbar chatScroll;
    #endregion

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
        p = PhotonNetwork.LocalPlayer;
        id = p.ActorNumber;

        for(int i=0; i<PhotonNetwork.PlayerList.Length; i++)
        {
            idToPlayer.Add(PhotonNetwork.PlayerList[i].ActorNumber, PhotonNetwork.PlayerList[i]);
        }

        SpawnPlayer();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        idToPlayer.Remove(otherPlayer.ActorNumber);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        idToPlayer.Add(newPlayer.ActorNumber, newPlayer);
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(Random.Range(0, 10000).ToString(), new RoomOptions { MaxPlayers = 18 });
    }

    private void SpawnPlayer()
    {
        player = PhotonNetwork.Instantiate(GameManager.Instance.savedData.userInfo.playerRosoName,
            GameManager.Instance.mainManager.startPos,Quaternion.identity).GetComponent<PlayerScript>();
        player.playerId = id;
        GameManager.Instance.mainManager.player = player;
        UIManager.Instance.LoadingFade();
    }

    public void SendMsg()
    {
        if (chatInput.text.Trim() == "") return;

        PV.RPC("Chatting", RpcTarget.AllViaServer,"<color=green>"+p.NickName+ "</color>: " +chatInput.text);
        chatInput.text = "";
    }

    [PunRPC]
    void Chatting(string msg)
    {
        msg.Replace("시발", "**").Replace("병신", "**").Replace("지랄", "**");
        chatText.text += chatText.text != "" ? "\n" + msg : msg;
        chatScroll.value = 0;
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }
}
