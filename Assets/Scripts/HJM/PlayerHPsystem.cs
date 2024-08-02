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
        healGuide.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        if (playerfsm.pyState == PlayerFSM.PlayerState.Dying)
        {
            DyingHP(1.0f);
        }

        if (playerfsm.pyState == PlayerFSM.PlayerState.Injured)
        {
            healGuide.SetActive(true);
            if(Input.GetMouseButton(0)) // ����ȸ�� â ���� ������� ü���̶� ������
            {
                print("�� ������");
                SelfHealHP(1.0f);

            }
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
    void SelfHealHP(float delayTime)
    {
        currentTime += Time.deltaTime;
        if (currentTime > delayTime)
        {
            print("ü�� 1�ø�");
            playerfsm.currentHp += 1;
            currentTime = 0;
        }


    }
}



    
