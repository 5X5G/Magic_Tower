using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public Transform[] cells;
    public UILabel showFloor;
    private CellData[,] cellInfos;
    private List<CellData[,]> floorList;
    private string MapPath = "Assets/Resources/";
    private string mapInfo = "mapInfo.bytes";
    private int floor;
}
