using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTable : UITable {

    public NTableData tableData;
    public Transform mRoot;
    public Transform mInfoTable;
    public GameObject cell;
    public UIScrollView scrollView;
    public UIDragScrollView mDragScrollView;
    protected CellData cellInfo;       

    protected override void Start()
    {
        tableData.Init();
        //Init();
        base.Start();
    }

    protected override void Init()
    {
        base.Init();

        if (tableData.CellDatas.Count == 0)
        {
            Debug.Log(gameObject.transform.parent.name + "无元素");
            return;
        }

        if (tableData.CellDatas[0].type == "title")
            InitCellView1();
    }

    public void InitCellView1()
    {
        List<CellData> celldatas = new List<CellData>(celldatas = tableData.CellDatas);        
        int length = tableData.CellDatas.Count;

        if (tableData.CellDatas == null)
        {
            Debug.Log("无元素");
            return;
        }            
        for (int i = 0; i < length; i++)
        {
            var go = Instantiate(cell) as GameObject;
            go.transform.parent = mRoot;
            go.transform.localScale = Vector3.one;
            go.name = tableData.CellDatas[i].altas;

            UILabel label = go.transform.GetComponentInChildren<UILabel>();
            label.text = go.name;
            UIDragScrollView dragScrollView = go.GetComponent<UIDragScrollView>();
            dragScrollView = mDragScrollView;

            ObjState objState = go.GetComponent<ObjState>();
            objState.CellInfo = celldatas[i];
            objState.InfoTable = mInfoTable.GetComponent<NTable>();
            objState.parentTable = this;
        }
    }

    public void InitCellView2()
    {
        if (Creater.chooseTitle == null)
        {
            Debug.Log("无标题");
            return;
        }  

        if (gameObject.transform.childCount != 0)
            gameObject.transform.DestroyChildren();

        scrollView.ResetPosition();

        List<SpriteInfo> spriteInfo = Creater.spriteInfos[Creater.chooseTitle];
        int length = spriteInfo.Count;
        for (int i = 0; i < length; i++)
        {
            var go = Instantiate(cell) as GameObject;
            go.transform.parent = mRoot;
            go.transform.localScale = Vector3.one;
            go.name = spriteInfo[i].name;

            ObjState objState = go.GetComponent<ObjState>();
            Debug.Log("Art/UI/" + Creater.chooseTitle);
            var atlas_go = Resources.Load<GameObject>("Art/UI/" + Creater.chooseTitle);
            objState.mUISprite.atlas = atlas_go.GetComponent<UIAtlas>();
            objState.CellInfo = new CellData();
            objState.CellInfo.altas = spriteInfo[i].parent;
            objState.CellInfo.sName = spriteInfo[i].name;
            objState.CellInfo.type = "cell";
            //objState.CellInfo.property

            objState.mUISprite.spriteName = go.name;
            objState.mUISprite.depth = 1;
            UIDragScrollView dragScrollView = go.GetComponent<UIDragScrollView>();
            dragScrollView = mDragScrollView;
        }
    }
}
