using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
//using UnityEditor.Experimental.GraphView;
//using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFSM : MonoBehaviour
{   



    public enum PlayerState
    {
        Normal = 0,
        Injured = 1,
        Dying = 2,
        InAction = 3,
        Hooked = 4,
        

    }

    public PlayerState pyState;

    public float playerMaxHp = 100.0f;
    public float currentHp;
    public float moveSpeed;
    public float runSpeed;
    PlayerController playerCtrl;
    public Slider playerHpSLD;


    void Start()
    {
        playerCtrl = GetComponent<PlayerController>();
        currentHp = playerMaxHp;


    }

    void Update()
    {
        // 현재 체력을 최대체력으로 나눈값을 슬라이더 값에 넣는다.
        //playerHpSLD.value = currentHp/ playerMaxHp;

        switch (pyState)
        {
            case PlayerState.Normal:
                Normal();
                break;
            case PlayerState.Injured:
                Injured();
                break;
            case PlayerState.Dying:
                Dying();
                break;
            case PlayerState.InAction:
                InAction();
                break;
            case PlayerState.Hooked:
                Hooked();
                break; 
            
            

        }




    }

    public void Normal()
    {        
        moveSpeed = 2.26f;          // 08.15 수정!
        runSpeed = 4.0f;       

    }

    public void Injured()
    {
        moveSpeed = 2.26f;           // 08.15 수정!
        runSpeed = 4.0f;        

    }

    public void Dying()
    {
        moveSpeed = 0.7f;
        runSpeed = 0.0f;

    }

    public void InAction()
    {
        moveSpeed = 0.0f;
        runSpeed = 0.0f;
           
    }

    public void Hooked()
    {
    } 
    
    
    

}