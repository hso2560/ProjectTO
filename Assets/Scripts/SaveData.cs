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
    public int killCnt;  //�ð� ������ ��
    public int playCnt;  //�ð� ������ 
    public string nickName;
    public string playerRosoName = "Player1"; //�ϴ��� �⺻���� ����
    public string bestRecordDate;  //�ð� ������
}

[Serializable]
public class Option
{
    public Language language = Language.English;

    public bool isFullScr = true;
    public float soundEffect = 0.5f;
    public float bgmSize = 0.5f;
}

