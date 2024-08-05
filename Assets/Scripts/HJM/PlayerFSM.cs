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
        Hooked = 4

    }

    public PlayerState pyState;

    public float playerMaxHp = 100.0f;
    public float currentHp;
    public float moveSpeed;
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
        // �ȴ� �ӵ� 5.0f , �޸��� �ӵ� 8.0f
        playerCtrl.InputMovement(5.0f, 8.0f);
    }

    public void Injured()
    {
        // �ȴ� �ӵ� 4.0f , �޸��� �ӵ� 7.0f
        playerCtrl.InputMovement(4.0f, 7.0f);

    }

    public void Dying()
    {
        // �ȴ� �ӵ� 1.0f , �޸��� �ӵ� 0.0f �޸� �� ����
        playerCtrl.InputMovement(1.0f, 0.0f);

    }

    public void InAction()
    {
    }

    public void Hooked()
    {
    }
    



}