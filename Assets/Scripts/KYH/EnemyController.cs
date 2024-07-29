using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 에너미 컨트롤러.

    /*
        매 이동시 방향 전환을 위한 타겟이 필요하다(트랜스폼 컴포넌트를 받아온다)
        
    */

    Rigidbody rb;
    Vector3 dir;
    Transform targetTransform;
    public float moveSpeed = 7.0f;
    HangPlayerHookInteraction hang;
    public short playerState=2;  // 플레이어 상태 구현 시 지울 예정*************
                                 // 0 - 건강, 1 - 부상, 2 - 빈사

    public float currentTime = 0;
    float delay = 2.0f;


    public enum EnemyState
    {
        NoEvidence, //증거 없음
        FindAura,   // 오라(아우라) 발견
        FindTrace,  // 흔적 발견
        FindPlayer, // 플레이어 발견
        GetPlayer,  // 플레이어 업음
    }

    public EnemyState currentState;    // 현재 상태
    Transform playerTransform;   // 플레이어의 트랜스폼(직접 추격 때 사용)


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // 리지드바디 캐싱
        //currentState = EnemyState.NoEvidence;   // 초기 상태는 "배회" 상태 => 이게 기존 코드 밑에 줄은 실험을 위해 추가한 코드 이후 삭제 **********
        currentState = EnemyState.FindPlayer;   // 실험용 이후 삭제할 것임 **************************************************************************
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // 씬 상의 "Player" 태그를 가진 오브젝트의 트랜스폼 캐싱
        hang = GetComponent<HangPlayerHookInteraction>();
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.NoEvidence:
                Wander();
                break;
            case EnemyState.FindAura:
                ChaseAura();
                break;
            case EnemyState.FindTrace:
                ChaseTrace();
                break;
            case EnemyState.FindPlayer:
                ChasePlayer();
                break;
            case EnemyState.GetPlayer:
                GetPlayer();
                break;
        }
    }

    void Wander()
    {

    }

    void ChaseAura()
    {

    }

    void ChaseTrace()
    {

    }

    void ChasePlayer()
    {
        dir = playerTransform.position - transform.position;
        dir.Normalize();
        rb.velocity = dir * moveSpeed;
        float distance = Vector3.Distance(transform.position, hang.playerObject.transform.position);
        if (playerState <= 1 && distance < 2)   // 범위 내에서 아직 플레이어가 빈사 상태가 아니면 공격을 시도
        {
            Attack();
        }
        if (playerState > 1 && distance < 2)    // 범위 내에서 플레이어가 빈사 상태면 업을 수 있다
        {
            hang.HangPlayerOnMe();
            ChangeState(EnemyState.GetPlayer);
        }
    }

    void GetPlayer()    // 플레이어를 업었을 때 갈고리로 향하는 기능
    {
        dir = hang.pillarTransform.position - transform.position;
        dir.Normalize();
        dir.y = 0;
        rb.velocity = dir * moveSpeed;
        float distance = Vector3.Distance(transform.position, hang.pillarTransform.position);
        if (distance < 2.0f)
        {
            hang.HangPlayerOnHook();
        }
    }

    void Attack()   // 공격 기능
    {
        currentTime += Time.deltaTime;
        if (currentTime > delay)
        {
            playerState++;
            currentTime = 0;
        }
    }

    void ChangeState(EnemyState newState)   // 상태 변화 기능
    {
        currentState = newState;
    }

}
