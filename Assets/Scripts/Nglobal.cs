using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nglobal : Singleton<Nglobal> {

    public enum DictionaryName
    {
        Item,
        Character,
        Monster,
        Wall,
    }
    public static string playerCharactername = "Actor01-Braver01_1_1";
    public static string constantWall = "gray";

    public static Nglobal nglobal;    
    public static Map map;
    public static Menu menu;    

    public static ReadSource readSource;
    public static PoolManager poolManager;
    public static RefreshManger refreshManger;
    public static PlayerManger playerManager;
  

    public static Dictionary<string, List<SpriteInfo>> spriteInfos = new Dictionary<string, List<SpriteInfo>>();

    private void Start()
    {
        Init();
        SceneManager.LoadScene("Game");
    }
    private void Init()
    {
        nglobal = Instance;
        DontDestroyOnLoad(this.gameObject);        
        spriteInfos = InitSpriteList();
    }

    public static Dictionary<string, List<SpriteInfo>> InitSpriteList()
    {
        Dictionary<string, List<SpriteInfo>> spriteInfos = new Dictionary<string, List<SpriteInfo>>();
        foreach (DictionaryName name in Enum.GetValues(typeof(DictionaryName)))
        {
            List<SpriteInfo> tempList = readSource.LoadSpiteName(name.ToString());
            spriteInfos.Add(name.ToString(), tempList);
        }
        return spriteInfos;
    }

    #region 提示语
    public static string forgetChooseCell = "没有选中的元素";
    public static string mapInfoMiss = "没有mapInfo的bytes文件";
    public static string showFloor = "层数: ";
    public static string showFloorError1 = "层数到底了 ";
    public static string saveSuccess = "保存bytes成功";
    public static string loadSuccess = "读取bytes成功";
    public static string initSuccess = "Init地图成功";
    public static string OutOfBorder = "超出边界";
    #endregion
}
