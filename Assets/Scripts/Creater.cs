using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Creater : MonoBehaviour
{
    public static string chooseTitle;
    public static CellData chooseCell;
    public static Dictionary<string, List<SpriteInfo>> spriteInfos = new Dictionary<string, List<SpriteInfo>>();

    public Transform[] cells;
    public UILabel showFloor;
    private CellData[,] cellInfos;
    private List<CellData[,]> floorList;
    private string MapPath = "Assets/Resources/";
    private string mapInfo = "mapInfo.bytes";
    private int floor;

    private void Awake()
    {
        Nglobal.creater = this;
    }

    private void Start()
    {        
        spriteInfos = Nglobal.InitSpriteList();
        cellInfos = new CellData[11, 11];
        floorList = new List<CellData[,]>();
        floor = 1;
        ShowFloor();
        Debug.Log(Application.dataPath);
        Debug.Log(Application.streamingAssetsPath);
        Debug.Log(Application.persistentDataPath); 
        Debug.Log(Application.temporaryCachePath);
    }

    public void Save()
    {
        SaveCellState();
        if (floorList.Count >= floor)
            Nglobal.readSource.ReplaceMapInfo(cellInfos, ref floorList, floor - 1);
        else
            Nglobal.readSource.AddMapInfo(cellInfos, ref floorList);

        Nglobal.readSource.CreateMapBytes(MapPath, mapInfo, floorList);
        
    }

    //Load前会清除未保存的设置
    public void Load()
    {
        floorList = new List<CellData[,]>();
        floor = 1;
        Init();
        Nglobal.readSource.LoadMapBytes(mapInfo, ref floorList, MapPath);
        SaveCellState(0);
        InitializeCell1(cellInfos);        
        ShowFloor();
    }

    public void Init()
    {
        cellInfos = new CellData[11, 11];
        Nglobal.readSource.InitMap(ref cells);
        Debug.Log(Nglobal.initSuccess);
    }

    public void UpFloor()
    {
        if (floorList.Count < floor)
        {
            SaveCellState();
            Nglobal.readSource.AddMapInfo(cellInfos, ref floorList);
            Init();
            floor++;            
        }
        else if (floorList.Count == floor)
        {
            SaveCellState();
            Nglobal.readSource.ReplaceMapInfo(cellInfos, ref floorList, floor - 1);
            Init();
            floor++;
        }
        else
        {
            SaveCellState();
            Nglobal.readSource.ReplaceMapInfo(cellInfos, ref floorList, floor - 1);
            Init();
            floor++;
            InitializeCell1(floorList[floor - 1]);
        }
        ShowFloor();
    }

    public void DownFloor()
    {
        if (floor > 1)
        {
            if (floorList.Count >= floor)
            {
                SaveCellState();
                Nglobal.readSource.ReplaceMapInfo(cellInfos, ref floorList, floor - 1);
                Init();
                floor--;
                InitializeCell1(floorList[floor - 1]);
            }
            else
            {
                SaveCellState();
                Nglobal.readSource.AddMapInfo(cellInfos, ref floorList);
                Init();
                floor--;
                InitializeCell1(floorList[floor - 1]);               
            }
        }
        else
            Debug.Log(Nglobal.showFloorError1);
        ShowFloor();
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

    private void SaveCellState(int floor)
    {
        cellInfos = floorList[floor];     
    }
    
    private void InitializeCell1(CellData[,] cellInfos)
    {
        for (int i = 0; i < cellInfos.GetLength(0); i++)
        {
            for (int j = 0; j < cellInfos.GetLength(1); j++)
            {
                if (cellInfos[i, j] == null)
                    continue;

                Transform tempTrans = cells[i * cellInfos.GetLength(0) + j];
                CellData tempData = new CellData();
                tempData= cellInfos[i, j];
                tempTrans.GetComponent<ObjState>().CellInfo = tempData;
                tempTrans.name = tempData.sName;
                var atlas_go = Resources.Load<GameObject>("Art/UI/" + tempData.altas);                
                UISprite mUISprite = tempTrans.GetComponent<UISprite>();
                mUISprite.atlas = atlas_go.GetComponent<UIAtlas>();
                mUISprite.GetComponent<UIButton>().normalSprite = tempData.sName;
            }
        }
    }

    private void ShowFloor()
    {
        showFloor.text = Nglobal.showFloor + floor;
    }
    
}
