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
    public Transform targetTransform;
    public float moveSpeed = 7.0f;
    HangPlayerHookInteraction hang;
    public int playerState=2;  // 플레이어 상태 구현 시 지울 예정, playerState를 이용해 조건을 판단하는 부분 수정 필요 *************
                               // 0 - 건강, 1 - 부상, 2 - 빈사
    public float attackRange = 2.0f;    // 공격 사정 거리

    public float currentTime = 0;
    float attackDelay = 2.0f;
    public float degree = 45.0f;    // 에너미 시야각
    public float maxDistance;       // 에너미 가시거리
    public float stunTime = 2.0f;


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


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // 리지드바디 캐싱
        currentState = EnemyState.NoEvidence;   // 초기 상태는 "배회" 상태 => 이게 기존 코드 밑에 줄은 실험을 위해 추가한 코드 이후 삭제 **********
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
        

    void Wander()   // 돌아다닌다.
    {
        // 증거가 없으면 돌아다닌다.(가장 가까운 발전기부터 순서대로)
        //targetTransform = generator.transform;

        // 증거 발견 시 우선 순위에 따라 특정 증거를 타겟으로 지정하고 상태를 변경
        if (SeePlayerInSight(degree, maxDistance))
        {
            ChangeState(EnemyState.FindPlayer);
        }
        else if (SeeTraceInSight(degree, maxDistance))
        {
            ChangeState(EnemyState.FindTrace);
        }
        else if (SeeAuraInSight(degree, maxDistance))
        {
            ChangeState(EnemyState.FindAura);
        }
    }

    void ChaseAura()    // 아우라(오라)를 쫓는다.
    {
        NMA.SetDestination(targetTransform.position);

        if (SeePlayerInSight(degree, maxDistance))  // 플레이어를 발견하면
        {
            ChangeState(EnemyState.FindPlayer);     // 플레이어를 쫓는다.
        }
        else if (SeeTraceInSight(degree, maxDistance))  // 흔적을 발견하면
        {
            ChangeState(EnemyState.FindTrace);      // 흔적을 쫓는다.
        }
        else if (targetTransform == null)           // 플레이어 or 흔적을 못 찾았는데 아우라마저 놓치면
        {
            ChangeState(EnemyState.NoEvidence);     // 다시 증거 없는 상태로 돌아간다.
        }
    }

    void ChaseTrace()   // 흔적을 쫓는다.
    {
        NMA.SetDestination(targetTransform.position);

        if (SeePlayerInSight(degree,maxDistance))   // 플레이어를 발견하면
        {
            ChangeState(EnemyState.FindPlayer);     // 플레이어를 쫓는다.
        }
        else if (targetTransform == null)           // 플레이어를 못 찾았는데 흔적마저 놓치면
        {
            ChangeState(EnemyState.NoEvidence);     // 다시 증거 없는 상태로 돌아간다.
        }
    }

    void ChasePlayer()
    {
        NMA.SetDestination(targetTransform.position);
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
        if (currentTime > attackDelay)
        {
            playerState++;
            currentTime = 0;
        }
    }

    bool SeePlayerInSight(float degree, float maxDistance)   // 시야 내 플레이어 탐지 기능. 시야각:degree, 가시거리:maxDistance
    {                                                           // 가시거리에 있는지 확인 -> 타겟과 나 사이의 각도와 시야각 비교
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
                    return true;
                }
            }
        }
        return false;
    }

    bool SeeTraceInSight(float degree, float maxDistance)   // 시야 내 흔적 탐지 기능. 시야각:degree, 가시거리:maxDistance
    {                                                           // 가시거리에 있는지 확인 -> 타겟과 나 사이의 각도와 시야각 비교
        targetTransform = null;
        GameObject[] traces = GameObject.FindGameObjectsWithTag("Trace");
        for (int i = 0; i < traces.Length; i++)
        {
            float distance = Vector3.Distance(traces[i].transform.position, transform.position);
            if (distance < maxDistance)
            {
                Vector3 lookVector = traces[i].transform.position - transform.position;
                lookVector.Normalize();

                float cosTheta = Vector3.Dot(lookVector, transform.forward);
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

                if (theta < degree)
                {
                    targetTransform = traces[i].transform;
                    return true;
                }
            }
        }
        return false;
    }

    bool SeeAuraInSight(float degree, float maxDistance)   // 시야 내 흔적 탐지 기능. 시야각:degree, 가시거리:maxDistance
    {                                                           // 가시거리에 있는지 확인 -> 타겟과 나 사이의 각도와 시야각 비교
        targetTransform = null;
        GameObject[] auras = GameObject.FindGameObjectsWithTag("Aura");
        for (int i = 0; i < auras.Length; i++)
        {
            float distance = Vector3.Distance(auras[i].transform.position, transform.position);
            if (distance < maxDistance)
            {
                Vector3 lookVector = auras[i].transform.position - transform.position;
                lookVector.Normalize();

                float cosTheta = Vector3.Dot(lookVector, transform.forward);
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

                if (theta < degree)
                {
                    targetTransform = auras[i].transform;
                    return true;
                }
            }
        }
        return false;
    }

    void Stuned() // 판자 맞을 때 그로기 걸리는 기능
    {
        currentTime += Time.deltaTime;
        // 스턴 시간 동안 움직임이 없고 스턴 시간이 끝나면 다시 증거를 찾아 다님
        if (currentTime > stunTime)
        {
            ChangeState(EnemyState.NoEvidence);
        }
    }

    void ChangeState(EnemyState newState)   // 상태 변화 기능
    {
        currentState = newState;
        currentTime = 0;
    }
}
