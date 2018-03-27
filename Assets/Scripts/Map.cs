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
        floor = 1;
    }

    private void Load()
    {
        Nglobal.readSource.LoadMapBytes(mapInfo, ref floorList, Application.persistentDataPath);
        InitializeCell(cellInfos);
        LoadFloorList(floorList[floor - 1]);
        cellInfos = floorList[floor - 1];
        GetPlayerVec(cellInfos);
    }

    private void InitializeCell(CellData[,] cellInfos)
    {
        for (int i = 0; i < cellInfos.GetLength(0); i++)
        {
            for (int j = 0; j < cellInfos.GetLength(1); j++)
            {
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
                    ChangeCellData(playerVecRef1 + 1, playerVecRef2, Nglobal.DictionaryName.Character.ToString(), Nglobal.playerCharactername);
                    ChangeCellView(playerVecRef1 + 1, playerVecRef2);
                    ChangeCellData(playerVecRef1, playerVecRef2, null, null);
                    ChangeCellView(playerVecRef1, playerVecRef2);
                    playerVecRef1++;
                }
                else
                    Debug.Log("Move Character down " + Nglobal.OutOfBorder);
            }
            else
            {
                if (playerVecRef1 - 1 >= 0)
                {
                    ChangeCellData(playerVecRef1 - 1, playerVecRef2, Nglobal.DictionaryName.Character.ToString(), Nglobal.playerCharactername);
                    ChangeCellView(playerVecRef1 - 1, playerVecRef2);
                    ChangeCellData(playerVecRef1, playerVecRef2, null, null);
                    ChangeCellView(playerVecRef1, playerVecRef2);
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
                    ChangeCellData(playerVecRef1, playerVecRef2 + 1, Nglobal.DictionaryName.Character.ToString(), Nglobal.playerCharactername);
                    ChangeCellView(playerVecRef1, playerVecRef2 + 1);
                    ChangeCellData(playerVecRef1, playerVecRef2, null, null);
                    ChangeCellView(playerVecRef1, playerVecRef2);
                    playerVecRef2++;
                }
                else
                    Debug.Log("Move Character right" + Nglobal.OutOfBorder);
            }   
            else
            {
                if (playerVecRef2 - 1 >= 0)
                {
                    ChangeCellData(playerVecRef1, playerVecRef2 - 1, Nglobal.DictionaryName.Character.ToString(), Nglobal.playerCharactername);
                    ChangeCellView(playerVecRef1, playerVecRef2 - 1);
                    ChangeCellData(playerVecRef1, playerVecRef2, null, null);
                    ChangeCellView(playerVecRef1, playerVecRef2);
                    playerVecRef2--;
                }
                else
                    Debug.Log("Move Character left" + Nglobal.OutOfBorder);                
            }
        }
    }

    private void GetPlayerVec(CellData[,] cellInfos)
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

    private void ChangeCellView(int x, int y)
    {
        Transform tempTrans = cells[x * cellInfos.GetLength(0) + y];
        CellData tempData = new CellData();
        tempData = cellInfos[x, y];

        if (tempData == null)
        {
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
            cellInfos[x,y] = null;        
    }
}
