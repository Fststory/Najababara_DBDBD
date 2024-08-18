using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPsystem : MonoBehaviour
{

    PlayerFSM playerfsm;
    float currentTime;
    public GameObject healGuide;



    void Start()
    {
        playerfsm = GetComponent<PlayerFSM>();
        //healGuide.SetActive(false);


    }

    
    void Update()
    {
        //if (playerfsm.pyState == PlayerFSM.PlayerState.Dying)
        //{
        
        //    healGuide.SetActive(true);
        //    if(Input.GetMouseButton(0)) // 상태회복 창 따로 파줘야함 체력이랑 별개라
        //    {
                
        //        print("힐 시작함");
        //        SelfHealHP(1.0f);

        //    }
        //    else
        //    {
        //        DyingHP(1.0f);
        //    }
        //}


    }



    // Dying 상태라면 1초가 지날 때 마다 체력이 2씩 감소하게 한다.
    // 체력이 다 사라질 때 까지
    void DyingHP(float delayTime)
    {
        
        currentTime += Time.deltaTime;
        if(currentTime > delayTime)
        {
            print("체력 2깎음");
            playerfsm.currentHp -= 2;
            currentTime = 0;
        }

    }
    void SelfHealHP(float delayTime)
    {
        currentTime += Time.deltaTime;
        if (currentTime > delayTime)
        {
            print("체력 1올림");
            playerfsm.currentHp += 1;
            currentTime = 0;
        }


    }
}



    
