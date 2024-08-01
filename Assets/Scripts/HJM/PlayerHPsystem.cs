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



    // Dying ���¶�� 1�ʰ� ���� �� ���� ü���� 2�� �����ϰ� �Ѵ�.
    // ü���� �� ����� �� ����
    void DyingHP(float delayTime)
    {
        currentTime += Time.deltaTime;
        if(currentTime > delayTime)
        {
            print("ü�� 2����");
            playerfsm.currentHp -= 2;
            currentTime = 0;
        }



    }

}



    
