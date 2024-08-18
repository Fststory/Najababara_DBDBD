using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarFSM : MonoBehaviour
{
    public enum pillarState
    {
        NoSacrifice = 0,    // 제물 없음
        SacrificeLV1 = 1,   // 해당 제물 진행도 1
        SacrificeLV2 = 2,   // 해당 제물 진행도 2
        AbsorbSacrifice = 3,  // 제물 먹음
        Damaged = 4   // 망가짐
    }

    public GameObject player;   // 플레이어 오브젝트
    PlayerController playerController;   // 플레이어 컨트롤러
    PlayerFSM playerFSM;   // 플레이어 상태
    Animator playerAnim;
    public pillarState currentState;   // 갈고리 현재 상태

    public float currentTime = 0;  // 현재 시간 (여러개의 타이머에 쓰임 흡수, 수리 등)
    public float reduceHPTime;  // 체력을 깎는 주기
    public float escapePercetage;   // 탈출 확률
    public float eatTime;   // 흡수 시간
    public float repairTime;    // 수리 시간

    public GameObject entity;
    public GameObject smoke;

    void Start()
    {
        currentState = pillarState.NoSacrifice;
        playerController = player.GetComponent<PlayerController>();
        playerFSM = player.GetComponent<PlayerFSM>();
        playerAnim = player.GetComponent<Animator>();
        entity.SetActive(false);
        smoke.SetActive(false);
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
            case pillarState.Damaged:
                SelfRepair();
                break;
        }
    }    

    private void WaitSacrifice()    // 제물이 걸리면 상태를 기준으로 다음 태세로의 전환 기능
    {
        if (playerFSM.pyState == PlayerFSM.PlayerState.Hooked)   // 플레이어가 걸렸으면 체력 확인!
        {
            if (!playerController.alraedyHooked)    // 플레어가 처음 걸린다면
            {
                ChangeState(pillarState.SacrificeLV1);  // 1단계 진행
                playerController.alraedyHooked = true;
            }
            else if (playerController.alraedyHooked)  // 플레이어가 이미 한번 걸렸었다면
            {
                ChangeState(pillarState.SacrificeLV2);    // 2단계 진행
            }
        }
    }

    private void GiveFalseHope()    // 희망고문하는 기능    [SacrificeLV1]
    {
        // 플레이어 체력을 조금씩 깎음
        currentTime += Time.deltaTime;
        if (currentTime > reduceHPTime)
        {
            playerFSM.currentHp--;
            currentTime = 0;
        }
        // 탈출 시도 기회를 줌
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= escapePercetage)   // 성공 확률은 4%
            {
                playerController.enabled = playerController.enabled;   // 플레이어 컨트롤러 활성화!
                playerFSM.pyState = PlayerFSM.PlayerState.Injured;  // 플레이어를 부상 상태로 변경
                ChangeState(pillarState.NoSacrifice);   // 갈고리를 제물 없음 상태로 전환
                print("탈출 성공!");
                playerAnim.SetTrigger("escape");
            }
            else
            {
                // 실패 시 현재 체력에서 16.666% 체력 감소
                playerFSM.currentHp -= playerFSM.currentHp * 0.16666f;
                print("탈출 실패!");
            }    
        }
        if (playerFSM.currentHp <= 50)  // 체력이 50 이하가 된다면...
        {
            ChangeState(pillarState.SacrificeLV2);  // 2단계 진행
        }
    }

    private void TryToAbsorb()      // 잡아먹을 시도를 하는 기능   [SacrificeLV2]
    {
        entity.SetActive(true);
        smoke.SetActive(true);

        // 플레이어 체력을 조금씩 깎음
        currentTime += Time.deltaTime;
        if (currentTime > reduceHPTime)
        {
            playerFSM.currentHp--;
            currentTime = 0;
        }
        // 일정 시간마다 제물 압박
        // 스킬 체크 두 번 연속 실패 시 처형
        if (playerFSM.currentHp <= 0)
        {
            ChangeState(pillarState.AbsorbSacrifice);
        }
    }

    private void DisappearAfterMeal()        // 다 먹고 사라지는 기능    [AbsorbSacrifice]
    {
        Destroy(player.gameObject);

        currentTime += Time.deltaTime;
        if (currentTime > eatTime)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage()    // 상대방이 나에게 데미지를 입히는 기능
    {
        ChangeState(pillarState.Damaged);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, transform.localEulerAngles + new Vector3(0, 0, 90), 1);
        print("갈고리 망가짐!");
    }

    private void SelfRepair()   // 플레이어가 망가뜨리면 저절로 수리하는 기능  [Damaged]
    {
        currentTime += Time.deltaTime;
        
        if (currentTime > repairTime)
        {
            ChangeState(pillarState.NoSacrifice);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles,
                                                  transform.localEulerAngles + new Vector3(0, 0, -90), 1);
            currentTime = 0;
            print("갈고리 셀프 수리 완료!");
        }
    }

    void ChangeState(pillarState newState)      // 상태 전환 기능
    {
        currentState = newState;
    }       
}
