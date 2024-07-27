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

    enum EnemyState
    {
        NoEvidence, //증거 없음
        FindAura,   // 오라(아우라) 발견
        FindTrace,  // 흔적 발견
        FindPlayer, // 플레이어 발견
        Attack  // 공격
    }

    EnemyState currentState;    // 현재 상태
    Transform playerTransform;   // 플레이어의 트랜스폼(직접 추격 때 사용)


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // 리지드바디 캐싱
        //currentState = EnemyState.NoEvidence;   // 초기 상태는 "배회" 상태 => 이게 기존 코드 밑에 줄은 실험을 위해 추가한 코드 이후 삭제 **********
        currentState = EnemyState.FindPlayer;   // 실험용 이후 삭제할 것임 **************************************************************************
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // 씬 상의 "Player" 태그를 가진 오브젝트의 트랜스폼 캐싱
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
            case EnemyState.Attack:
                Attack();
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
        rb.velocity += dir * moveSpeed * Time.deltaTime;
    }

    void Attack()
    {

    }

    void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

}
