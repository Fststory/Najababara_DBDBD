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
        AbsorbSacrifice,  // 제물 먹음
        Damaged   // 망가짐
    }

    public GameObject player;   // 플레이어 오브젝트
    pillarState currentState;   // 현재 상태

    float currentTime = 0;  // 현재 시간
    public float eatTime;   // 흡수 시간
    public float damagedTime;  // 부숴지는데 걸리는 시간
    public float repairTime;    // 수리 시간

    void Start()
    {
        
    }

    void Update()
    {
        switch (currentState)
        {
            case pillarState.Damaged:
                SelfRepair();
                break;
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

    private void SelfRepair()   // 플레이어가 망가뜨리면 저절로 수리하는 기능
    {
        currentTime += Time.deltaTime;
        if (currentTime > repairTime)
        {
            ChangeState(pillarState.NoSacrifice);
        }
    }

    private void WaitSacrifice()    // 제물이 걸리면 상태를 기준으로 다음 태세로의 전환 기능/ 플레이어 체력 & 상태 구현 시 해당 값으로 판단 **********************
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

    public void TakeDamage()    // 상대방이 나에게 데미지를 입히는 기능
    {
        ChangeState(pillarState.Damaged);
    }

    private void OnTriggerStay(Collider other)  // 플레이어가 갈고리에 걸리지 않은 상태로 상호작용 범위에 있을 때 사용 가능한 기능
    {
        //if (other.CompareTag("Player") && playerState != playerState.Hooked)    // 플레이어 상태 구현 시 그에 맞게 적용 *********************
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentTime += Time.deltaTime;
                if (currentTime > damagedTime)
                {
                    TakeDamage();
                }
            }
        }
    }
}
