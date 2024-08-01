using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPsystem : MonoBehaviour
{

    PlayerFSM playerfsm;
    float currentTime;
 



    void Start()
    {
        playerfsm = GetComponent<PlayerFSM>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerfsm.pyState == PlayerFSM.PlayerState.Dying)
        {
            DyingHP(1.0f);

        }


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

}



    
