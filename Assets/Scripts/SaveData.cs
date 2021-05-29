using System;

[Serializable]
public class SaveData
{
    public Option option = new Option();
    public UserInfo userInfo = new UserInfo();
}

[Serializable]
public class UserInfo
{
    public bool isFirstStart = true;
    public bool isClear = false;
    public int bestTime;
    public int killCnt;
    public int playCnt;
    public string nickName;
    public string playerRosoName = "Player1";
}

[Serializable]
public class Option
{
    public bool isFullScr = true;
}

