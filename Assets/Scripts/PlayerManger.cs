using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManger : MonoBehaviour {

    public float moveSpeed;
    private int type;
    private PlayerInfo playerInfo;

    public PlayerInfo PlayerInfomation { private set; get; }
    public int Type { set; get; }

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
                Type = 1;
                Nglobal.map.MoveCharacter(Type);
            }

            if (Input.GetKeyDown(KeyCode.S) || downPressed)
            {
                Type = 2;
                Nglobal.map.MoveCharacter(Type);
            }

            if (Input.GetKeyDown(KeyCode.A) || leftPressed)
            {
                Type = 3;
                Nglobal.map.MoveCharacter(Type);
            }

            if (Input.GetKeyDown(KeyCode.D) || rightPressed)
            {
                Type = 4;
                Nglobal.map.MoveCharacter(Type);
            }
            time = Time.time;
        }     
    }

    public void Up()
    {
        if(!downPressed & !leftPressed & !rightPressed)
            upPressed = true;
    }

    public void Down()
    {
        if(!upPressed & !leftPressed & !rightPressed)
            downPressed = true;   
    }

    public void Left()
    {
        if (!upPressed & !downPressed & !rightPressed)
            leftPressed = true;
    }

    public void Right()
    {
        if (!upPressed & !downPressed & !leftPressed)
            rightPressed = true;
    }

    public void ReleaseButton()
    {
        upPressed = false;
        downPressed = false;
        leftPressed = false;
        rightPressed = false;
    }
}
