using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public Option option = new Option();
}

[Serializable]
public class Option
{
    public bool isFullScr = true;
}

