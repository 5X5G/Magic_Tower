using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManger : MonoBehaviour {

    public float moveSpeed;
    public UILabel hp;
    public UILabel atk;
    public UILabel def;
    public UILabel yellowKey;
    public UILabel blueKey;
    public UILabel redKey;
    public UILabel goldCount;
    private int type;
    private PlayerInfo playerInfo;

    public PlayerInfo PlayerInfomation
    {
        set
        {
            playerInfo = value;
        }
        get
        {
            return playerInfo;
        }
    }
    public int Type
    {
        set
        {
            type = value;
        }
        get
        {
            return type;
        }
    }

    //1234对应上下左右

    private bool upPressed;
    private bool downPressed;
    private bool leftPressed;
    private bool rightPressed;
    private float time;

    private void Awake()
    {
        Nglobal.playerManager = this;
    }

    private void Start()
    {


    }

    public void Init()
    {
        InitPlayerInfomation();
        RenewProperty();
        upPressed = false;
        downPressed = false;
        leftPressed = false;
        rightPressed = false;
        time = Time.time;
    }

    private void Update()
    {
        if (Time.time - time > moveSpeed)
        {
            //PC测试用
            if (Input.GetKeyDown(KeyCode.W) || upPressed)
            {
                UpClickEvent();
            }

            if (Input.GetKeyDown(KeyCode.S) || downPressed)
            {
                DownClickEvent();
            }

            if (Input.GetKeyDown(KeyCode.A) || leftPressed)
            {
                LeftClickEvent();
            }

            if (Input.GetKeyDown(KeyCode.D) || rightPressed)
            {
                RightClickEvent();
            }
            time = Time.time;
        }
    }

    public void Up()
    {
        if (!downPressed && !leftPressed && !rightPressed)
        {
            upPressed = true;
            UpClickEvent();
        }
    }

    public void Down()
    {
        if (!upPressed && !leftPressed && !rightPressed)
        {
            DownClickEvent();
            downPressed = true;
        }

    }

    public void Left()
    {
        if (!upPressed && !downPressed && !rightPressed)
            leftPressed = true;
    }

    public void Right()
    {
        if (!upPressed && !downPressed && !leftPressed)
            rightPressed = true;
    }

    public void ReleaseButton()
    {
        upPressed = false;
        downPressed = false;
        leftPressed = false;
        rightPressed = false;
    }

    public void UpClick()
    {
        ReleaseButton();
        UpClickEvent();
    }

    public void DownClick()
    {
        ReleaseButton();
        DownClickEvent();
    }

    public void LeftClick()
    {
        ReleaseButton();
        LeftClickEvent();
    }

    public void RightClick()
    {
        ReleaseButton();
        RightClickEvent();
    }

    private void UpClickEvent()
    {
        Type = 1;
        Nglobal.map.MoveCharacter(Type);
    }

    private void DownClickEvent()
    {
        Type = 2;
        Nglobal.map.MoveCharacter(Type);
    }

    private void LeftClickEvent()
    {
        Type = 3;
        Nglobal.map.MoveCharacter(Type);
    }

    private void RightClickEvent()
    {
        Type = 4;
        Nglobal.map.MoveCharacter(Type);
    }

    public void RenewProperty()
    {
        hp.text = "HP：" + PlayerInfomation.standardPro.Hp.ToString();
        atk.text = "ATK：" + PlayerInfomation.standardPro.ATK.ToString();
        def.text = "DEF：" + PlayerInfomation.standardPro.DEF.ToString();
        yellowKey.text = PlayerInfomation.items[0].property.count.ToString();
        blueKey.text = PlayerInfomation.items[1].property.count.ToString();
        redKey.text = PlayerInfomation.items[2].property.count.ToString();
        goldCount.text = PlayerInfomation.gold.ToString();
    }

    private void InitPlayerInfomation()
    {
        PlayerInfomation = new PlayerInfo();
        PlayerInfomation.items = new List<CellData>();
        PlayerInfomation.treasures = new List<CellData>();
        for (int i = 0; i < Nglobal.itemName.Length; i++)
        {
            CellData tempData = new CellData();
            tempData.sName = Nglobal.itemName[i];
            tempData.property = new Property();
            tempData.property.count = Nglobal.map.keyCounts[i];
            PlayerInfomation.items.Add(tempData);
        }
        PlayerInfomation.gold = 0;
        PlayerInfomation.standardPro = Nglobal.map.playerInitStandProperty;        
    }
}
