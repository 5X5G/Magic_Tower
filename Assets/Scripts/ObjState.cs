using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjState : MonoBehaviour {

    public UISprite mUISprite;

    public CellData CellInfo     {    set;  get;   }

    public NTable parentTable   { set; get; }
    public NTable InfoTable     { set; get; }

    public void ClickTitle()
    {
        Creater.chooseTitle = gameObject.name;
        Debug.Log(Creater.chooseTitle);
        InfoTable.InitCellView2();
        InfoTable.Reposition();
    }

    public void ClickCell()
    {
        Creater.chooseCell = CellInfo;
        Debug.Log(Creater.chooseCell.altas);
        Debug.Log(Creater.chooseCell.sName);
    }

    public void CilickMapCell()
    {
        if (Creater.chooseCell == null)
        {
            Debug.Log(Nglobal.forgetChooseCell);
            return;
        }
        CellInfo = Creater.chooseCell;
        gameObject.name = CellInfo.sName;
        var atlas_go = Resources.Load<GameObject>("Art/UI/" + Creater.chooseTitle);
        mUISprite.atlas = atlas_go.GetComponent<UIAtlas>();        
        mUISprite.GetComponent<UIButton>().normalSprite = gameObject.name;
        Debug.Log(mUISprite.spriteName);
    }
}
