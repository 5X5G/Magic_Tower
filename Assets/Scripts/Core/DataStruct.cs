using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteInfo
{
    public FileInfo fileinfo;
    public string name;
    public string name_extra;
    public string parent;
}

public class CellData
{
    //1为地板，2为生物，3为道具,4为宝物
    public string type;
    public string sName;
    public string altas;
    public Property property;
}


public class Property
{
    public string describe;
    //宝物效果，临时用string格式
    public string power;
    public StandardProperty standardPro;
    //区分道具等级（包括钥匙），同时也区别一次性宝物
    //消耗道具1，钥匙2，怪物3,墙壁4,楼梯5，宝物6，一次性宝物7
    public int type;
    //基础属性type
    public int sType;
    public int count;
}


public class PlayerInfo
{
    public StandardProperty standardPro;
    public List<CellData> items;
    public List<CellData> treasures;
    public int gold;
}

public class StandardProperty
{
    public int Hp;
    public int ATK;
    public int DEF;
}
