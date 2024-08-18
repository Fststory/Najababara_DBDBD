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
        Dead = 5
    }

    public PlayerState pyState;

    public float playerMaxHp = 100.0f;
    public float currentHp;
    public float moveSpeed;
    public float runSpeed;
    PlayerController playerCtrl;
    public Slider playerHpSLD;

    public GameObject img_Normal;
    public GameObject img_Injured;
    public GameObject img_Dying;
    public GameObject img_Hooked;
    public GameObject img_Hang;
    public GameObject img_Dead;


    void Start()
    {
        playerCtrl = GetComponent<PlayerController>();
        currentHp = playerMaxHp;

        img_Normal.SetActive(false);
        img_Injured.SetActive(false);
        img_Dying.SetActive(false);
        img_Hooked.SetActive(false);
        img_Hang.SetActive(false);
        img_Dead.SetActive(false);

    }

    void Update()
    {
        // 현재 체력을 최대체력으로 나눈값을 슬라이더 값에 넣는다.
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
            case PlayerState.Dead:
                Dead();
                break;
        }
    }

    public void Normal()
    {        
        moveSpeed = 2.26f;          // 08.15 수정!
        runSpeed = 4.0f;

        img_Normal.SetActive(true);

        img_Injured.SetActive(false);
        img_Dying.SetActive(false);
        img_Hooked.SetActive(false);
        img_Hang.SetActive(false);
        img_Dead.SetActive(false);
    }

    public void Injured()
    {
        moveSpeed = 2.26f;           // 08.15 수정!
        runSpeed = 4.0f;

        img_Injured.SetActive(true);

        img_Normal.SetActive(false);
        img_Dying.SetActive(false);
        img_Hooked.SetActive(false);
        img_Hang.SetActive(false);
        img_Dead.SetActive(false);
    }

    public void Dying()
    {
        moveSpeed = 0.5f;
        runSpeed = 0.7f;

        
        img_Dying.SetActive(true);

        img_Normal.SetActive(false);
        img_Injured.SetActive(false);
        img_Hooked.SetActive(false);
        //img_Hang.SetActive(false);
        img_Dead.SetActive(false);

    }

    public void InAction()
    {
        moveSpeed = 0.0f;
        runSpeed = 0.0f;
    }

    public void Hooked()
    {
        img_Hooked.SetActive(true);

        img_Normal.SetActive(false);
        img_Injured.SetActive(false);
        img_Dying.SetActive(false);
        img_Hang.SetActive(false);
        img_Dead.SetActive(false);
    }

    public void Dead()
    {
        img_Dead.SetActive(true);

        img_Normal.SetActive(false);
        img_Injured.SetActive(false);
        img_Dying.SetActive(false);
        img_Hang.SetActive(false);
        img_Hooked.SetActive(false);
    }

}