using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarFSM : MonoBehaviour
{
    enum pillarState
    {
        NoSacrifice,    // 제물 없음
        SacrificeLV1,   // 해당 제물 진행도 1
        SacrificeLV2,   // 해당 제물 진행도 2
        AbsorbSacrifice  // 제물 먹음
    }

    public GameObject player;
    pillarState currentState;

    float currentTime = 0;
    public float eatTime;

    void Start()
    {
        
    }

    void Update()
    {
        switch (currentState)
        {
            case pillarState.NoSacrifice:
                WaitSacrifice();
                break;
            case pillarState.SacrificeLV1:
                GiveFalseHope();
                break;
            case pillarState.SacrificeLV2:
                TryToAbsorb();
                break;
            case pillarState.AbsorbSacrifice:
                DisappearAfterMeal();
                break;
        }
    }

    private void WaitSacrifice()    // 제물이 걸리면 상태를 기준으로 다음 태세로의 전환 기능/ 플레이어 체력& 구현 시 해당 값으로 판단 **********************
    {                                                                                       // 일단 player hp, state로 표시
        //if (player.GetComponent<>().hp > 50.0f)
        //{
        //    ChangeState(pillarState.SacrificeLV1);
        //}
        //else if (1.0f < player.GetComponent<>().hp %% player.GetComponent<>().hp < 50.0f)
        //{
        //    ChangeState(pillarState.SacrificeLV2);
        //}else if( player.GetComponent<>().hp < 1.0f || player.GetComponent<>().state == 2)
    }
    private void GiveFalseHope()    // 희망고문하는 기능
    {
        // 탈출 시도 기회를 줌
        // 성공 확률은 4%
        // 실패 시 현재 체력에서 16.666% 체력 감소

    }

    private void TryToAbsorb()      // 잡아먹을 시도를 하는 기능
    {
        // 일정 시간마다 제물 압박
        // 스킬 체크 두 번 연속 실패 시 처형
        ChangeState(pillarState.AbsorbSacrifice);
    }

    private void DisappearAfterMeal()        // 다 먹고 사라지는 기능
    {
        Destroy(player.gameObject);


        currentTime += Time.deltaTime;
        if (currentTime > eatTime)
        {
            Destroy(gameObject);
        }
    }

    void ChangeState(pillarState newState)      // 상태 전환 기능
    {
        currentState = newState;
    }
}
