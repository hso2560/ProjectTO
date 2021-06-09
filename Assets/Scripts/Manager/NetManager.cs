using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetManager : MonoBehaviourPunCallbacks
{
    [SerializeField] bool isDev = false; //�׽�Ʈ�� �ڵ�
    public bool IsDev { get { return isDev; } }
    public static NetManager instance;
    private PhotonView PV;
    public int id;
    public Player p;
    private PlayerScript player;
    private Message msgClass;
    
    public Vector3 firstPos,v;

    //public List<Player> diedUsers = new List<Player>();
    public Dictionary<int, Player> idToPlayer = new Dictionary<int, Player>();
    public Vector3 startPos;
    public float spawnRandomRange;

    #region UI
    public InputField chatInput;
    public GameObject chatPanel, chatPlus, chatMinus;
    public Text chatText, TestInformTxt;
    public Scrollbar chatScroll;
    [SerializeField] private Text newMsgTxt;
    [SerializeField] private Button[] userBtns;
    #endregion

    private void Awake()
    {
        instance = this;
        PV = photonView;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.ConnectUsingSettings();

        TestInformTxt.gameObject.SetActive(isDev);
    }
    public override void OnConnectedToMaster()
    {
        msgClass = new Message();
        
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
        PV.RPC("Chatting", RpcTarget.AllViaServer, "<color=green>'" + p.NickName + "'</color>���� �����Ͽ����ϴ�.");
        RenewalMainUserList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        idToPlayer.Remove(otherPlayer.ActorNumber);
        PV.RPC("Chatting", RpcTarget.AllViaServer, "<color=purple>'" + p.NickName + "'</color>���� Ż���߽��ϴ�.");
        RenewalMainUserList();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        idToPlayer.Add(newPlayer.ActorNumber, newPlayer);
        RenewalMainUserList();
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(Random.Range(0, 10000).ToString(), new RoomOptions { MaxPlayers = 18 });
    }

    private void SpawnPlayer()
    {
        v = new Vector3(startPos.x + Random.Range(-spawnRandomRange, spawnRandomRange), 1, startPos.z + Random.Range(-spawnRandomRange, spawnRandomRange));
        firstPos = v;

        player = PhotonNetwork.Instantiate(GameManager.Instance.savedData.userInfo.playerRosoName,
            v,Quaternion.identity).GetComponent<PlayerScript>();

        player.playerId = id;
        GameManager.Instance.mainManager.player = player;
        UIManager.Instance.LoadingFade();
    }

    private void RenewalMainUserList()
    {
        int idx = PhotonNetwork.PlayerList.Length;
        for(int i=0; i<idx; i++)
        {
            userBtns[i].gameObject.SetActive(true);
            userBtns[i].transform.GetChild(0).GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }
        for(int i=idx; i<userBtns.Length; ++i)
        {
            userBtns[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        _Input();
    }

    private void _Input()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!chatPanel.activeSelf)
            {
                ChatPanelOnOff(true);
                newMsgTxt.gameObject.SetActive(false);
                //chatInput.MoveTextEnd(false);
            }
            else
            {
                SendMsg();
            }
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            player.Die("�ڻ�");
        }
    }

    public void ChatPanelOnOff(bool isOn)
    {
        chatPanel.SetActive(isOn);
        if (isOn) chatInput.ActivateInputField();
        chatPlus.SetActive(!isOn);
        chatMinus.SetActive(isOn);

        if (chatPanel.activeSelf) newMsgTxt.gameObject.SetActive(false);
    }

    public void HitPlayer(int myAct, int otherAct, int damage)
    {
        msgClass.myAct = myAct;
        msgClass.otherAct = otherAct;
        msgClass.iValue = damage;
        PV.RPC("Damaged", idToPlayer[otherAct], JsonUtility.ToJson(msgClass));
    }

    [PunRPC]
    void Damaged(string msg) => player.Damaged(msg);

    public void DiedPlayer(string msg) => PV.RPC("RPCDied", RpcTarget.AllViaServer, msg);

    [PunRPC]
    private void RPCDied(string msg)
    {
        msgClass = JsonUtility.FromJson<Message>(msg);
        //diedUsers.Add(idToPlayer[msgClass.myAct]);
        string attacker = idToPlayer[msgClass.otherAct].NickName;
        string victim = idToPlayer[msgClass.myAct].NickName;
        
        Chatting($"<color=blue>'{attacker}'</color>���� '<color=red>{victim}</color>'���� �����Ͽ����ϴ�.");
    }

    public void SendMsg()
    {
        if (chatInput.text.Trim() == "")
        {
            //chatInput.Select();
            chatPanel.SetActive(false);
            chatPlus.SetActive(true);
            chatMinus.SetActive(false);
            return;
        }

        PV.RPC("Chatting", RpcTarget.AllViaServer,"<color=green>"+p.NickName+ "</color>: " +chatInput.text);
        chatInput.text = "";
    }

    [PunRPC]
    void Chatting(string msg)
    {
        if (!chatPanel.activeSelf) newMsgTxt.gameObject.SetActive(true);
        string ms=msg.Replace("�ù�", "**").Replace("����", "**").Replace("����", "**");
        chatText.text += chatText.text != "" ? "\n" + ms : ms;
        chatScroll.value = 0;
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }
}
