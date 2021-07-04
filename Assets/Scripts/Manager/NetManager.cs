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

    private string suicideStr;
    private int langInt;

    #region UI
    public InputField chatInput;
    public GameObject chatPanel, chatPlus, chatMinus, mouseCursorTxt;
    public Text chatText, TestInformTxt;
    public Scrollbar chatScroll;
    public Button sendBtn;
    [SerializeField] private Text newMsgTxt;
    [SerializeField] private Button[] userBtns;

    [SerializeField] GameObject DevPanel;
    [SerializeField] InputField DevChatInput;
    #endregion

    private void Awake()
    {
        instance = this;
        PV = photonView;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.ConnectUsingSettings();

        TestInformTxt.gameObject.SetActive(isDev);
        DevPanel.SetActive(false);
    }

    private void Start()
    {
        LangPatch(GameManager.Instance.savedData.option.language);
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
    /*public override void OnDisconnected(DisconnectCause cause)
    {

    }*/
    public override void OnJoinedRoom()
    {
        p = PhotonNetwork.LocalPlayer;
        id = p.ActorNumber;

        for(int i=0; i<PhotonNetwork.PlayerList.Length; i++)
        {
            idToPlayer.Add(PhotonNetwork.PlayerList[i].ActorNumber, PhotonNetwork.PlayerList[i]);
        }

        SpawnPlayer();
        msgClass.sValue = "<color=green>'" + p.NickName + "'</color>���� �����Ͽ����ϴ�.";
        msgClass.sValue2 = "<color=green>'" + p.NickName + "'</color> has participated.";
        PV.RPC("SpecialChat", RpcTarget.AllViaServer, JsonUtility.ToJson(msgClass));
        SetTag("GOALUSER", false);
        RenewalMainUserList();

        GameManager.Instance.savedData.userInfo.playCnt++;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        msgClass.sValue = !(bool)GetTag("GOALUSER", otherPlayer) ? "<color=purple>'" + p.NickName + "'</color>���� Ż���߽��ϴ�." : "<color=purple>'" + p.NickName + "'</color>���� ������ �����Ͽ����ϴ�.";
        msgClass.sValue2= !(bool)GetTag("GOALUSER", otherPlayer) ? "<color=purple>'" + p.NickName + "'</color> has escaped." : "<color=purple>'" + p.NickName + "'</color> has made a splendid exit.";
        PV.RPC("SpecialChat", RpcTarget.AllViaServer, JsonUtility.ToJson(msgClass));
        idToPlayer.Remove(otherPlayer.ActorNumber);
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
        GameManager.Instance.player = player;
        GameManager.Instance.mainManager.objManager.playerLoad = true;
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
        if (player == null) return;

        if (player.bCompulsoryIdle) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!chatPanel.activeSelf)
            {
                if (player.IsJumping || player.MoveVec != Vector3.zero) return;

                ChatPanelOnOff(true);
                newMsgTxt.gameObject.SetActive(false);
                //chatInput.MoveTextEnd(false);
            }
            else
            {
                SendMsg();
            }
        }
        else if(Input.GetKeyDown(KeyCode.R) && !chatInput.isFocused)
        {
            player.Die(suicideStr);
        }
    }

    public void LangPatch(Language lang)
    {
        langInt = (int)lang;
        if (lang == Language.English)
        {
            newMsgTxt.text = "New Message!";
            sendBtn.transform.GetChild(0).GetComponent<Text>().text = "Send";
            suicideStr = "Suicide";
        }
        else if(lang==Language.Korean)
        {
            newMsgTxt.text = "���ο� �޽���!";
            sendBtn.transform.GetChild(0).GetComponent<Text>().text = "����";
            suicideStr = "�ڻ�";
        }
    }

    public void ChatPanelOnOff(bool isOn)
    {
        chatPanel.SetActive(isOn);
        chatInput.gameObject.SetActive(isOn);
      
        chatPlus.SetActive(!isOn);
        chatMinus.SetActive(isOn);

        if (isOn)
        {
            newMsgTxt.gameObject.SetActive(false);
            chatInput.ActivateInputField();
        }

        //chatInput.interactable = isOn;
        //sendBtn.interactable = isOn;
        //mouseCursorTxt.SetActive(!isOn);

        //if (!chatInput.interactable && chatInput.isFocused) chatInput.Select();
    }

    public void HitPlayer(int myAct, int otherAct, int damage)
    {
        msgClass.myAct = myAct;
        msgClass.otherAct = otherAct;
        msgClass.iValue = damage;
        SoundManager.Instance.PlaySoundEffect(2);
        PV.RPC("Damaged", idToPlayer[otherAct], JsonUtility.ToJson(msgClass));
    }

    [PunRPC]
    void Damaged(string msg)
    {
        SoundManager.Instance.PlaySoundEffect(2);
        UIManager.Instance.DamageEffect();
        player.Damaged(msg);
    }

    public void DiedPlayer(string msg) => PV.RPC("RPCDied", RpcTarget.AllViaServer, msg);

    [PunRPC]
    private void RPCDied(string msg)
    {
        msgClass = JsonUtility.FromJson<Message>(msg);
        //diedUsers.Add(idToPlayer[msgClass.myAct]);
        string attacker = idToPlayer[msgClass.otherAct].NickName;
        string victim = idToPlayer[msgClass.myAct].NickName;
        
        if(langInt==1)
            Chatting($"<color=blue>'{attacker}'</color>���� '<color=red>{victim}</color>'���� �����Ͽ����ϴ�.");
        else if(langInt==0)
            Chatting($"<color=blue>'{attacker}'</color> killed '<color=red>{victim}</color>'.");

        if (id == msgClass.otherAct)
        {
            GameManager.Instance.savedData.userInfo.killCnt++;
        }
    }

    public void SendMsg()
    {
        if (chatInput.text.Trim() == "")
        {
            //chatInput.Select();
            ChatPanelOnOff(false);
            
            return;
        }

        PV.RPC("Chatting", RpcTarget.AllViaServer,"<color=green>"+p.NickName+ "</color>: " +chatInput.text);
        chatInput.text = "";
    }

    [PunRPC]
    void SpecialChat(string json)
    {
        msgClass = JsonUtility.FromJson<Message>(json);
        if(langInt==0)
        {
            Chatting(msgClass.sValue2);
        }
        else if(langInt==1)
        {
            Chatting(msgClass.sValue);
        }
    }

    [PunRPC]
    void Chatting(string msg)
    {
        if (!chatPanel.activeSelf) newMsgTxt.gameObject.SetActive(true);
        string ms=msg.Replace("�ù�", "**").Replace("����", "**").Replace("����", "**").Replace("fuck","*");  //������ ���͸�
        chatText.text += chatText.text != "" ? "\n" + ms : ms;
        chatScroll.value = 0;
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        GameManager.Instance.SceneChange("Lobby");
    }

    public void AllSystemMsg(string msg) => PV.RPC("AllSystemMsgRPC", RpcTarget.AllViaServer, msg);

    [PunRPC]
    private void AllSystemMsgRPC(string msg)
    {
        msgClass = JsonUtility.FromJson<Message>(msg);
        string m = langInt == 1 ? msgClass.sValue : msgClass.sValue2;
        UIManager.Instance.ShowSystemMsg(m, 0.5f, msgClass.fValue, 0.5f);
        Chatting(msgClass.sValue);
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }



    #region �±�
    public void SetTag(string key, object value, Player player = null)
    {
        if (player == null) player = PhotonNetwork.LocalPlayer;
        player.SetCustomProperties(new Hashtable { { key, value } });
    }
    public object GetTag(string key, Player player = null)
    {
        if (player == null) player = PhotonNetwork.LocalPlayer;
        return player.CustomProperties[key];
    }
    public bool AllhasTag(string key)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            if (PhotonNetwork.PlayerList[i].CustomProperties[key] == null) return false;
        return true;
    }
    #endregion












    #region .
    public void DeportBtn()
        => PV.RPC("UserDeport", RpcTarget.AllViaServer, id);
    
    public void SpeakerBtn()
    {
        PV.RPC("DevAllChat", RpcTarget.AllViaServer, DevChatInput.text);
        DevChatInput.text = "";
    }

    [PunRPC]
    void DevAllChat(string s)
    {
        string[] sa = s.Split('|');
        if (sa.Length != 5)
        {
            UIManager.Instance.ShowSystemMsg(s, 3f, 3f, 2f, 1);
            return;
        }

        UIManager.Instance.ShowSystemMsg(sa[0],float.Parse(sa[1]), float.Parse(sa[2]), float.Parse(sa[3]),int.Parse(sa[4]));
    }

    [PunRPC] void UserDeport(int act)
    {
        if(act!=id)
        {
            Disconnect();
        }
    }
    #endregion
}
