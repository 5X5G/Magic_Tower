using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTableData : MonoBehaviour {

    protected List<CellData> cellDatas = new List<CellData>();

    public List<CellData> CellDatas
    {
        get
        {
            return cellDatas;
        }
    }

    public virtual void Init()
    {

    }
}
