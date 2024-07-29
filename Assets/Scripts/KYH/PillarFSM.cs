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
            case pillarState.HaveSacrifice:
                TryToAbsorb();
                break;
            case pillarState.AbsorbSacrifice:
                Disappear();
                break;
        }
    }

    private void WaitSacrifice()    // 제물이 걸리면 먹을 준비하는 상태로 전환 기능
    {
        if (player.GetComponent<EnemyController>().currentState == EnemyController.EnemyState.GetPlayer)
        {
            ChangeState(pillarState.HaveSacrifice);
        }
    }

    private void TryToAbsorb()      // 본격적으로 먹을 시도를 하는 기능
    {
        // 일정 시간마다 제물 압박
        // 스킬 체크 실패 시 체력 감소
        ChangeState(pillarState.AbsorbSacrifice);
    }

    private void Disappear()        // 다 먹고 사라지는 기능
    {
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
