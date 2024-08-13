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
        // ���� ü���� �ִ�ü������ �������� �����̴� ���� �ִ´�.
        playerHpSLD.value = currentHp/ playerMaxHp;

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
        moveSpeed = 4.0f;
        runSpeed = 6.0f;
    }

    public void Injured()
    {
        moveSpeed = 3.5f;
        runSpeed = 5.5f;

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