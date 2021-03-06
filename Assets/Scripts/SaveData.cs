using System;

public enum Language
{
    English,
    Korean,
    COUNT
}

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
    public int killCnt;  //시간 남으면 함
    public int playCnt;  //시간 남으면 
    public string nickName;
    public string playerResoName = "Player1"; //일단은 기본값만 쓸거
    public string bestRecordDate;  //시간 남으면
    public short devId;
}

[Serializable]
public class Option
{
    public Language language = Language.English;

    public bool isFullScr = true;
    public float soundEffect = 0.5f;
    public float bgmSize = 0.5f;
}

