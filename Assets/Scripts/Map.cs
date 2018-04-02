using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public Transform[] cells;
    public UILabel showFloor;
    private CellData[,] cellInfos;
    private List<CellData[,]> floorList;
    private string MapPath = "Assets/Resources/";
    private string mapInfo = "/mapInfo.bytes";
    private Vector2 vec = new Vector2(0, 0);
    private int playerVecRef1;
    private int playerVecRef2;
    private int border;
    private int floor;
    private bool up;
    private bool canMove { set; get; }

    public  StandardProperty playerInitStandProperty = new StandardProperty();
    public  List<int> keyCounts = new List<int>();
    public  int initFloor;

    private void Awake()
    {
        Nglobal.map = this;
    }
    private void Start()
    {
        Init();
        Load();

        showFloor.text = Application.persistentDataPath;
        showFloor.text += "\n" + Application.dataPath;
        showFloor.text += "\n" + Application.streamingAssetsPath;
        showFloor.text += "\n" + Application.temporaryCachePath;
    }

    private void Init()
    {
        cellInfos = new CellData[11, 11];
        border = cellInfos.GetLength(0);
        floorList = new List<CellData[,]>();        
        canMove = true;
        up = true;
    }

    private void Load()
    {
        Nglobal.readSource.LoadMapBytes(mapInfo, ref floorList, Application.persistentDataPath);
        floor = initFloor;
        InitializeCell(cellInfos);
        LoadFloorList(floorList[floor - 1]);
        cellInfos = floorList[floor - 1];
        InitPlayerVec(cellInfos);
        Nglobal.playerManager.Init();
    }

    //初始化map显示，图集置空
    private void InitializeCell(CellData[,] cellInfos)
    {
        for (int i = 0; i < cellInfos.GetLength(0); i++)
        {
            for (int j = 0; j < cellInfos.GetLength(1); j++)
            {
                if (cellInfos[i, j] != null)
                {
                    cellInfos[i, j] = null;
                }

                Transform tempTrans = cells[i * cellInfos.GetLength(0) + j];
                UISprite mUISprite = tempTrans.GetComponent<UISprite>();
                mUISprite.atlas = null;                
            }
        }
    }

    private void LoadFloorList(CellData[,] cellInfos)
    {

        for (int i = 0; i < cellInfos.GetLength(0); i++)
        {
            for (int j = 0; j < cellInfos.GetLength(1); j++)
            {
                if (cellInfos[i, j] == null)
                    continue;

                Transform tempTrans = cells[i * cellInfos.GetLength(0) + j];
                CellData tempData = new CellData();
                tempData = cellInfos[i, j];
                tempTrans.GetComponent<ObjState>().CellInfo = tempData;
                tempTrans.name = tempData.sName;
                var atlas_go = Resources.Load<GameObject>("Art/UI/" + tempData.altas);
                UISprite mUISprite = tempTrans.GetComponent<UISprite>();
                mUISprite.atlas = atlas_go.GetComponent<UIAtlas>();
                mUISprite.GetComponent<UIButton>().normalSprite = tempData.sName;

            }
        }
    }

    //存储cell数据用
    private void SaveCellState()
    {
        for (int i = 0; i < cellInfos.GetLength(0); i++)
        {
            for (int j = 0; j < cellInfos.GetLength(1); j++)
            {
                Transform tempTrans = cells[i * cellInfos.GetLength(0) + j];
                if (!tempTrans.name.Contains("cell"))
                {
                    CellData tempData = tempTrans.GetComponent<ObjState>().CellInfo;
                    cellInfos[i, j] = tempData;
                }
                else
                {
                    cellInfos[i, j] = null;
                }
            }
        }
    }

    //public static void SMoveCharacter(int type)
    //{
    //    Nglobal.map.MoveCharacter(type);
    //}

    public void MoveCharacter(int type)
    {
        if (type == 1 || type == 2)
        {
            bool down = (type % 2 == 0) ? true : false;
            if (down)
            {
                if (playerVecRef1 + 1 < border)
                {
                    DetermineCellType(playerVecRef1 + 1, playerVecRef2);
                    if (!canMove)
                    {
                        canMove = true;
                        return;
                    }

                    ChangeCellData(playerVecRef1, playerVecRef2, null, null);
                    ChangeCellView(playerVecRef1, playerVecRef2);
                    ChangeCellData(playerVecRef1 + 1, playerVecRef2, Nglobal.DictionaryName.Character.ToString(), Nglobal.playerCharactername);
                    ChangeCellView(playerVecRef1 + 1, playerVecRef2);
                    playerVecRef1++;
                }
                else
                    Debug.Log("Move Character down " + Nglobal.OutOfBorder);
            }
            else
            {
                if (playerVecRef1 - 1 >= 0)
                {
                    DetermineCellType(playerVecRef1 - 1, playerVecRef2);
                    if (!canMove)
                    {
                        canMove =true;
                        return;
                    }
                        
                    ChangeCellData(playerVecRef1, playerVecRef2, null, null);
                    ChangeCellView(playerVecRef1, playerVecRef2);
                    ChangeCellData(playerVecRef1 - 1, playerVecRef2, Nglobal.DictionaryName.Character.ToString(), Nglobal.playerCharactername);
                    ChangeCellView(playerVecRef1 - 1, playerVecRef2);
                    playerVecRef1--;
                }

                else
                    Debug.Log("Move Character up" + Nglobal.OutOfBorder);
            }
        }
        else
        {
            bool left = (type % 2 == 0) ? true : false;
            if (left)
            {
                if (playerVecRef2 + 1 < border)
                {
                    DetermineCellType(playerVecRef1, playerVecRef2 + 1);
                    if (!canMove)
                    {
                        canMove = true;
                        return;
                    }

                    ChangeCellData(playerVecRef1, playerVecRef2, null, null);
                    ChangeCellView(playerVecRef1, playerVecRef2);
                    ChangeCellData(playerVecRef1, playerVecRef2 + 1, Nglobal.DictionaryName.Character.ToString(), Nglobal.playerCharactername);
                    ChangeCellView(playerVecRef1, playerVecRef2 + 1);
                    playerVecRef2++;
                }
                else
                    Debug.Log("Move Character right" + Nglobal.OutOfBorder);
            }
            else
            {
                if (playerVecRef2 - 1 >= 0)
                {
                    DetermineCellType(playerVecRef1, playerVecRef2 - 1);
                    if (!canMove)
                    {
                        canMove = true;
                        return;
                    }

                    ChangeCellData(playerVecRef1, playerVecRef2, null, null);
                    ChangeCellView(playerVecRef1, playerVecRef2);
                    ChangeCellData(playerVecRef1, playerVecRef2 - 1, Nglobal.DictionaryName.Character.ToString(), Nglobal.playerCharactername);
                    ChangeCellView(playerVecRef1, playerVecRef2 - 1);
                    playerVecRef2--;
                }
                else
                    Debug.Log("Move Character left" + Nglobal.OutOfBorder);
            }
        }
    }

    private void InitPlayerVec(CellData[,] cellInfos)
    {
        for (int i = 0; i < cellInfos.GetLength(0); i++)
        {
            for (int j = 0; j < cellInfos.GetLength(1); j++)
            {
                if (cellInfos[i, j] == null)
                    continue;

                if (cellInfos[i, j].sName == Nglobal.playerCharactername)
                {
                    playerVecRef1 = i;
                    playerVecRef2 = j;
                    return;
                }
            }
        }
    }

    private void ConfigPlayerVec(CellData[,] cellInfo)
    {
        if (up)
        {
            int ref1 = 0;
            int ref2 = 0;
            for (int i = 0; i < cellInfo.GetLength(0); i++)
            {
                for (int j = 0; j < cellInfo.GetLength(1); j++)
                {
                    if (cellInfo[i, j].sName == "upFloor")
                    {
                        ref1 = i;
                        ref2 = j;
                        break;
                    }
                }
            }

            if (ref1 + 1 < 11)
            {
                if (cellInfo[ref1 + 1, ref2] == null)
                {
                    Transform tempTrans = cells[(ref1 + 1) * cellInfos.GetLength(0) + ref2];
                    CellData tempData = new CellData();
                    tempData.altas = Nglobal.DictionaryName.Character.ToString();
                    tempData.sName = Nglobal.playerCharactername;
                    tempTrans.name = tempData.sName;
                    var atlas_go = Resources.Load<GameObject>("Art/UI/" + tempData.altas);
                    UISprite mUISprite = tempTrans.GetComponent<UISprite>();
                    mUISprite.atlas = atlas_go.GetComponent<UIAtlas>();
                    mUISprite.GetComponent<UIButton>().normalSprite = tempData.sName;
                    cellInfo[ref1 + 1, ref2] = tempData;
                }

                if (cellInfo[ref1 - 1, ref2] == null)
                {
                    Transform tempTrans = cells[(ref1 - 1) * cellInfos.GetLength(0) + ref2];
                    CellData tempData = new CellData();
                    tempData.altas = Nglobal.DictionaryName.Character.ToString();
                    tempData.sName = Nglobal.playerCharactername;
                    tempTrans.name = tempData.sName;
                    var atlas_go = Resources.Load<GameObject>("Art/UI/" + tempData.altas);
                    UISprite mUISprite = tempTrans.GetComponent<UISprite>();
                    mUISprite.atlas = atlas_go.GetComponent<UIAtlas>();
                    mUISprite.GetComponent<UIButton>().normalSprite = tempData.sName;
                    cellInfo[ref1 - 1, ref2] = tempData;
                }

                if (cellInfo[ref1, ref2 + 1] == null)
                {
                    Transform tempTrans = cells[ref1 * cellInfos.GetLength(0) + ref2 + 1];
                    CellData tempData = new CellData();
                    tempData.altas = Nglobal.DictionaryName.Character.ToString();
                    tempData.sName = Nglobal.playerCharactername;
                    tempTrans.name = tempData.sName;
                    var atlas_go = Resources.Load<GameObject>("Art/UI/" + tempData.altas);
                    UISprite mUISprite = tempTrans.GetComponent<UISprite>();
                    mUISprite.atlas = atlas_go.GetComponent<UIAtlas>();
                    mUISprite.GetComponent<UIButton>().normalSprite = tempData.sName;
                    cellInfo[ref1, ref2 + 1] = tempData;
                }

                if (cellInfo[ref1, ref2-1] == null)
                {
                    Transform tempTrans = cells[ref1 * cellInfos.GetLength(0) + ref2 - 1];
                    CellData tempData = new CellData();
                    tempData.altas = Nglobal.DictionaryName.Character.ToString();
                    tempData.sName = Nglobal.playerCharactername;
                    tempTrans.name = tempData.sName;
                    var atlas_go = Resources.Load<GameObject>("Art/UI/" + tempData.altas);
                    UISprite mUISprite = tempTrans.GetComponent<UISprite>();
                    mUISprite.atlas = atlas_go.GetComponent<UIAtlas>();
                    mUISprite.GetComponent<UIButton>().normalSprite = tempData.sName;
                    cellInfo[ref1, ref2 - 1] = tempData;
                }
            }
        }
    }


    private void ChangeCellView(int x, int y)
    {
        Transform tempTrans = cells[x * cellInfos.GetLength(0) + y];
        CellData tempData = new CellData();
        tempData = cellInfos[x, y];

        if (tempData == null)
        {
            tempTrans.name = "cell";
            UISprite nUISprite = tempTrans.GetComponent<UISprite>();
            nUISprite.atlas = null;
            return;
        }

        tempTrans.GetComponent<ObjState>().CellInfo = tempData;
        tempTrans.name = tempData.sName;
        var atlas_go = Resources.Load<GameObject>("Art/UI/" + tempData.altas);
        UISprite mUISprite = tempTrans.GetComponent<UISprite>();
        mUISprite.atlas = atlas_go.GetComponent<UIAtlas>();
        mUISprite.GetComponent<UIButton>().normalSprite = tempData.sName;

    }

    private void ChangeCellData(int x, int y, string altas, string spriteName)
    {
        if (altas != null)
        {
            CellData tempData = new CellData();
            tempData.altas = altas;
            tempData.sName = spriteName;
            cellInfos[x, y] = tempData;
        }
        else
            cellInfos[x, y] = null;
    }

    private void DetermineCellType(int x, int y)
    {
        if (cellInfos[x, y] == null)
            return;
        if (cellInfos[x, y].altas == Nglobal.DictionaryName.Monster.ToString())
        {
            OperateMonster(cellInfos[x, y]);
            Nglobal.playerManager.RenewProperty();
        }
        else if (cellInfos[x, y].altas == Nglobal.DictionaryName.Item.ToString())
        {
            OperateItem(cellInfos[x, y]);
            Nglobal.playerManager.RenewProperty();
            Debug.Log("sName：" + cellInfos[x, y].sName);
            Debug.Log("sType：" + cellInfos[x, y].property.sType);
            Debug.Log("count：" + cellInfos[x, y].property.count);
        }
        else if (cellInfos[x, y].altas == Nglobal.DictionaryName.Character.ToString())
        {

        }
        else if (cellInfos[x, y].altas == Nglobal.DictionaryName.Wall.ToString())
        {
            OperateWall(cellInfos[x, y]);
            Nglobal.playerManager.RenewProperty();
        }
    }

    private void OperateItem(CellData data)
    {
        if (data.property.type == 1)
        {
            StandardProperty sProperty = Nglobal.playerManager.PlayerInfomation.standardPro;
            if (data.property.sType == 1)
                sProperty.Hp += data.property.count;
            else if (data.property.sType == 2)
                sProperty.ATK += data.property.count;
            else
                sProperty.DEF += data.property.count;

            Nglobal.playerManager.PlayerInfomation.standardPro = sProperty;
        }
        else
        {
            foreach (CellData tempData in Nglobal.playerManager.PlayerInfomation.items)
            {
                if (tempData.sName == data.sName)
                    tempData.property.count++;
            }
        }
    }

    private void OperateMonster(CellData data)
    {
        StandardProperty monsterProperty = new StandardProperty();
        monsterProperty.Hp = data.property.standardPro.Hp;
        monsterProperty.ATK = data.property.standardPro.ATK;
        monsterProperty.DEF = data.property.standardPro.DEF;
        StandardProperty playerProperty = new StandardProperty();
        playerProperty.Hp = Nglobal.playerManager.PlayerInfomation.standardPro.Hp;
        playerProperty.ATK = Nglobal.playerManager.PlayerInfomation.standardPro.ATK;
        playerProperty.DEF = Nglobal.playerManager.PlayerInfomation.standardPro.DEF;
        if (playerProperty.ATK - monsterProperty.DEF <= 0)
        {
            canMove = false;
            Debug.Log(Nglobal.StrongerMonser);
            return;
        }

        while (playerProperty.Hp > 0)
        {
            monsterProperty.Hp -= playerProperty.ATK - monsterProperty.DEF;
            if (monsterProperty.Hp <= 0)
                break;
            playerProperty.Hp -= monsterProperty.ATK - playerProperty.DEF;
            if (playerProperty.Hp <= 0)
            {
                canMove = false;
                Debug.Log(Nglobal.StrongerMonser);
                return;
            }
        }

        Nglobal.playerManager.PlayerInfomation.standardPro = playerProperty;
        Nglobal.playerManager.PlayerInfomation.gold += data.property.count;
    }

    private void OperateWall(CellData data)
    {
        if (data.sName == Nglobal.upFloor)
        {
            SaveCellState();
            CellData[,] tempDatas= ConfigData(cellInfos);
            floorList[floor - 1] = tempDatas;
            InitializeCell(cellInfos);
            floor++;
            LoadFloorList(floorList[floor - 1]);
            cellInfos = floorList[floor - 1];
            canMove = false;
            return;
        }
        else if (data.sName == Nglobal.downFloor)
        {
            SaveCellState();
            CellData[,] tempDatas = ConfigData(cellInfos);
            floorList[floor - 1] = tempDatas;
            InitializeCell(cellInfos);
            floor--;
            LoadFloorList(floorList[floor - 1]);
            cellInfos = floorList[floor - 1];
            canMove = false;
            return;
        }


            if (data.sName != Nglobal.constantWall)
            {
                if (data.sName == Nglobal.constantwall2)
                    canMove = false;
                else
                {
                    int door = int.Parse(data.sName[4].ToString());
                    int keyCount = Nglobal.playerManager.PlayerInfomation.items[door].property.count;
                    if (keyCount > 0)
                    {
                        canMove = true;
                        Nglobal.playerManager.PlayerInfomation.items[door].property.count--;
                    }
                    else
                    {
                        canMove = false;
                        Debug.Log(Nglobal.itemKey[door] + Nglobal.NeedKey);
                    }

                }
            }
            else
                canMove = true;
    }

    private CellData[,] ConfigData(CellData[,] cellInfo)
    {
        CellData[,] tempData = new CellData[11, 11];
        for (int i = 0; i < cellInfo.GetLength(0); i++)
        {
            for (int j = 0; j < cellInfo.GetLength(1); j++)
            {
                if (cellInfo[i, j] == null)
                    continue;
                tempData[i, j] = new CellData();
                tempData[i, j].altas = cellInfo[i, j].altas;
                tempData[i, j].sName = cellInfo[i, j].sName;
            }
        }
        return tempData;
    }
}
