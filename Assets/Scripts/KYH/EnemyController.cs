using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // 에너미 컨트롤러.

    /*
        매 이동시 방향 전환을 위한 타겟이 필요하다(트랜스폼 컴포넌트를 받아온다)
        
    */

    NavMeshAgent NMA;

    Rigidbody rb;
    Vector3 dir;
    Transform targetTransform;
    public float moveSpeed = 7.0f;
    HangPlayerHookInteraction hang;
    public int playerState=2;  // 플레이어 상태 구현 시 지울 예정, playerState를 이용해 조건을 판단하는 부분 수정 필요 *************
                               // 0 - 건강, 1 - 부상, 2 - 빈사
    public float attackRange = 2.0f;

    public float currentTime = 0;
    float delay = 2.0f;


    public enum EnemyState
    {
        NoEvidence, //증거 없음
        FindAura,   // 오라(아우라) 발견
        FindTrace,  // 흔적 발견
        FindPlayer, // 플레이어 발견
        GetPlayer,  // 플레이어 업음
        OnGroggy    // 조작 불능(스턴)
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
        NMA = GetComponent<NavMeshAgent>();
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
            case EnemyState.OnGroggy:
                Stuned();
                break;
        }
    }

    void Wander()
    {
        // 증거가 없으면 돌아다닌다.

        // 증거 발견 시 우선 순위에 따라 특정 증거를 타겟으로 지정하고 상태를 변경
        //if (seePlayer)
        //{
        //    ChangeState(ChasePlayer);
        //}
        //else if (seeTrace)
        //{
        //    ChangeState(ChaseTrace);
        //}
        //else if (seeAura)
        //{
        //    ChangeState(ChaseAura);
        //}
    }

    void ChaseAura()
    {
        //targetTransform = aura.transform;
        //NMA.SetDestination(targetTransform.position);

        //if (seePlayer)
        //{
        //    ChangeState(ChasePlayer);
        //}
        //else if (seeTrace)
        //{
        //    ChangeState(ChaseTrace);
        //}
    }

    void ChaseTrace()
    {
        //targetTransform = trace.transform;
        //NMA.SetDestination(targetTransform.position);

        //if (seePlayer)
        //{
        //    ChangeState(ChasePlayer);
        //}
    }

    void ChasePlayer()
    {
        //dir = playerTransform.position - transform.position;
        //dir.Normalize();
        //rb.velocity = dir * moveSpeed;
        NMA.SetDestination(playerTransform.position);
        float distance = Vector3.Distance(transform.position, hang.playerObject.transform.position);
        if (playerState <= 1 && distance < attackRange)   // 범위 내에서 아직 플레이어가 빈사 상태가 아니면 공격을 시도
        {
            Attack();
        }
        if (playerState > 1 && distance < attackRange)    // 범위 내에서 플레이어가 빈사 상태면 업을 수 있다
        {
            hang.HangPlayerOnMe();
            ChangeState(EnemyState.GetPlayer);
        }
    }

    void GetPlayer()    // 플레이어를 업었을 때 갈고리로 향하는 기능
    {
        targetTransform = hang.pillarTransform;
        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);
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

    void CheckPlayerInSight(float degree, float maxDistance)   // 시야 내 플레이어 탐지 기능. 시야각:degree, 시야 거리:maxDistance
    {                                                           // 시야 거리에 있는지 확인 -> 타겟과 나 사이의 각도와 시야각 비교
        targetTransform = null;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++)
        {
            float distance = Vector3.Distance(players[i].transform.position, transform.position);
            if (distance < maxDistance)
            {
                Vector3 lookVector = players[i].transform.position - transform.position;
                lookVector.Normalize();

                float cosTheta = Vector3.Dot(lookVector, transform.forward);
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

                if (theta < degree)
                {
                    targetTransform = players[i].transform;
                    ChangeState(EnemyState.FindPlayer);
                }
            }
        }
    }

    void Stuned() // 판자 맞을 때 그로기 걸리는 기능
    {
        // 스턴 시간 동안 움직임이 없고 스턴 시간이 끝나면 다시 증거를 찾아 다님
    }

    void ChangeState(EnemyState newState)   // 상태 변화 기능
    {
        currentState = newState;
    }
}
