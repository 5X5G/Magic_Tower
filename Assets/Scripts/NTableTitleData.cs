using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTableTitleData : NTableData {

    public override void Init()
    {
        InitTitleInfo();
    }

    private void InitTitleInfo()
    {        
        foreach (Nglobal.DictionaryName name in Enum.GetValues(typeof(Nglobal.DictionaryName)))
        {
            CellData cellData = new CellData();
            cellData.type = "title";
            cellData.altas = name.ToString();
            CellDatas.Add(cellData);
        }
    }    

}
