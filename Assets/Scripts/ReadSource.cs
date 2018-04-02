using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadSource:Singleton<ReadSource>
{
    private string UIpath = "Assets/Resources/Art/Icon/";        
    private int length = 11;

    private void Awake()
    {
        Nglobal.readSource = this;
    }

    public List<SpriteInfo> LoadSpiteName(string dictionary)
    {
        List<SpriteInfo> spriteInfoList = new List<SpriteInfo>();
        string currentPath = UIpath + dictionary;
        if (Directory.Exists(currentPath))
        {
            DirectoryInfo directory = new DirectoryInfo(currentPath);
            FileInfo[] temp_FileInfo = directory.GetFiles();
            ArrayList fileInfos = new ArrayList();
            foreach (var file in temp_FileInfo)
            {
                if (!file.Name.Contains(".meta"))
                {
                    SpriteInfo temp = new SpriteInfo();
                    temp.fileinfo = file;
                    string[] nameList = file.Name.Split('.');
                    temp.name = nameList[0];
                    temp.name_extra = nameList[1];
                    temp.parent = directory.Name;
                    spriteInfoList.Add(temp);
                }
            }
        }
        return spriteInfoList;
    }

    public Dictionary<string, Property> LoadPropertyBytes(string path1,string path2)
    {
        Dictionary<string, Property> tempProperty = new Dictionary<string, Property>();
        StreamReader sReader = new StreamReader(path1 + path2, System.Text.Encoding.UTF8);
        string s = sReader.ReadToEnd();
        string[] lines = s.Split('\n');
        //第一行是注释
        for (int i = 1; i < lines.Length; i++)
        {
            string[] line = lines[i].Split('@');
            //第一个为字典的key
            Property property = new Property();
            StandardProperty sProperty = new StandardProperty();

            if (line[1] != "")
            {
                property.type = int.Parse(line[1]);
            }
            if (line[2] != "")
            {
                property.sType = int.Parse(line[2]);                
            }
            if (line[3] != "")
            {
                string[] stand = line[3].Split(',');
                sProperty.Hp = int.Parse(stand[0]);
                sProperty.ATK = int.Parse(stand[1]);
                sProperty.DEF = int.Parse(stand[2]);
                property.standardPro = sProperty;
            }
            if (line[4] != "")
            {
                property.describe = line[4];
            }

            if (line[5] != "")
            {
                property.power = line[5];
            }
            if (line[6] != "" && line[6] !="\r")
            {
                property.count = int.Parse(line[6]);
            }
            tempProperty.Add(line[0], property);
        }
        return tempProperty;
    }

    public void CreateMapBytes(string path1, string path2, List<CellData[,]> floorList)
    {
        StreamWriter sw;
        if (File.Exists(path1 + path2))
        {
            File.Delete(path1 + path2);
        }
        sw = File.CreateText(path1 + path2);
        for (int z = 0; z < floorList.Count; z++)
        {
            CellData[,] cellInfos = floorList[z];
            sw.WriteLine(z + "#");
            for (int i = 0; i < cellInfos.GetLength(0); i++)
            {
                for (int j = 0; j < cellInfos.GetLength(1); j++)
                {
                    if (cellInfos[i, j] == null)
                        sw.Write("null");
                    else
                        sw.Write(cellInfos[i, j].altas + "," + cellInfos[i, j].sName);

                    if (j == cellInfos.GetLength(1) - 1)
                        sw.Write("\n");
                    else
                        sw.Write("@");
                }
            }
        }
        sw.Flush();
        sw.Close();
        Debug.Log(path1 + path2 + "   " + Nglobal.saveSuccess);

    }

    public void LoadMapBytes(string mapFile,ref List<CellData[,]> floorList,string path1)
    {
        if (!File.Exists(path1 + mapFile))
        {
            Debug.Log(Nglobal.mapInfoMiss);
        }
            
        string[] path = mapFile.Split('.');
        StreamReader sr = new StreamReader(path1 + mapFile, System.Text.Encoding.Default);
        string s = sr.ReadToEnd();

        //去除最后的\n
        string mapArray = s.Remove(s.Length - 1);
        string[] mapLines = mapArray.Split('\n');

        int floor = 0;
        int line = 0;
        CellData[,] cellInfos = new CellData[11, 11];

        //读取保存文件中的玩家信息
        StandardProperty sProperty = new StandardProperty();
        string[] sPropertyLine = mapLines[1].Split('@');
        string[] sPropertyList = sPropertyLine[1].Split(',');
        sProperty.Hp = int.Parse(sPropertyList[0]);
        sProperty.ATK = int.Parse(sPropertyList[1]);
        sProperty.DEF = int.Parse(sPropertyList[2]);
        Nglobal.map.playerInitStandProperty = sProperty;
        Nglobal.map.keyCounts.Add(int.Parse(sPropertyLine[2]));
        Nglobal.map.keyCounts.Add(int.Parse(sPropertyLine[3]));
        Nglobal.map.keyCounts.Add(int.Parse(sPropertyLine[4]));
        Nglobal.map.initFloor = int.Parse(sPropertyLine[5]);

        for (int i = 1; i < mapLines.Length; i++)
        {            
            if (mapLines[i].Contains("#"))
            {                
                string[] temp = mapLines[i].Split('#');
                floor = int.Parse(temp[0]);
                cellInfos = new CellData[11, 11];
                line = 0;
                continue;
            }

            int realLine = floor * (length + 1) + line + 3;
            string[] mapLineInfo = mapLines[realLine].Split('@');
            for (int j = 0; j < mapLineInfo.Length; j++)
            {
                if (mapLineInfo[j] == "null")
                    continue;
                CellData tempData = new CellData();
                string[] names = mapLineInfo[j].Split(',');
                tempData.altas = names[0];
                tempData.sName = names[1];
                if (Nglobal.propertys.ContainsKey(tempData.sName))
                    tempData.property = Nglobal.propertys[tempData.sName];               
                cellInfos[line, j] = tempData;
            }
            line++;
            if (line == 11)
                floorList.Add(cellInfos);
        }              
        Debug.Log(mapFile + "   " + Nglobal.loadSuccess);
    }

    public void AddMapInfo(CellData[,] cellInfos, ref List<CellData[,]> floorList)
    {
        CellData[,] tempData = ConfigData(cellInfos);
        floorList.Add(tempData);
    }

    public void ReplaceMapInfo(CellData[,] cellInfos, ref List<CellData[,]> floorList,int floor)
    {
        CellData[,] tempData = ConfigData(cellInfos);
        floorList[floor] = tempData;
    }

    public void InitMap(ref Transform[] cells)
    {
        var atlas_go = Resources.Load<GameObject>("Art/UI/" + Nglobal.DictionaryName.Wall);
        for (int i = 0; i < cells.Length; i++)
        {
            ObjState objstate = cells[i].GetComponent<ObjState>();
            objstate.CellInfo = null;
            UISprite mUISprite = cells[i].GetComponent<UISprite>();
            mUISprite.atlas = atlas_go.GetComponent<UIAtlas>();
            mUISprite.GetComponent<UIButton>().normalSprite = Nglobal.constantWall;
            cells[i].name = "cell";
        }        
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