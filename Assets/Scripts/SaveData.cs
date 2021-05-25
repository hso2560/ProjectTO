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
    public bool isClear = false;
    public int bestTime;
}

[Serializable]
public class Option
{
    public bool isFullScr = true;
}

