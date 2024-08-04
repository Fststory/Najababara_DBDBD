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
        시야에 들어온 증거들 중에 우선순위를 따져서 타겟으로 잡는다.        
    */

    NavMeshAgent NMA;

    public GameObject player;
    public Transform targetTransform;
    public float moveSpeed = 7.0f;
    HangPlayerHookInteraction hang;
    public PlayerFSM playerFSM;         // 0 - 건강, 1 - 부상, 2 - 빈사, 3 - 특수 행동(업힘, 수리, 구조, 창문 넘기 등), 4 - 걸림
    public float attackRange = 2.0f;    // 공격 사정 거리

    public float currentTime = 0;
    float attackDelay = 2.0f;
    public float degree = 45.0f;    // 에너미 시야각
    public float maxDistance;       // 에너미 가시거리
    public float stunTime = 2.0f;
    public GameObject[] generators;
    int currentGeneratorIndex = 0;


    public enum EnemyState
    {
        NoEvidence = 0, //증거 없음
        FindAura = 1,   // 오라(아우라) 발견
        FindTrace = 2,  // 흔적 발견
        FindPlayer = 3, // 플레이어 발견
        GetPlayer = 4,  // 플레이어 업음
        OnGroggy = 5   // 조작 불능(스턴)
    }

    public EnemyState currentState;    // 현재 상태


    void Start()
    {
        currentState = EnemyState.NoEvidence;   // 초기 상태는 "배회" 상태 => 이게 기존 코드 밑에 줄은 실험을 위해 추가한 코드 이후 삭제 **********
        hang = GetComponent<HangPlayerHookInteraction>();
        playerFSM = player.GetComponent<PlayerFSM>();
        NMA = GetComponent<NavMeshAgent>();
        generators = GameObject.FindGameObjectsWithTag("Generator");
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
                GoToHang();
                break;
            case EnemyState.OnGroggy:
                Stuned();
                break;
        }
    }

    // [EnemyState.NoEvidence] 돌아다닌다.   
    void Wander()
    {
        // 증거가 없으면 돌아다닌다.(가장 가까운 발전기부터 순서대로.. 하고 싶었지만 그냥 씬에서 순서대로)
        // 이때 가장 가까운 발전기를 찾은 뒤 타겟으로 잡는다. 근처에 도달하면 다음 발전기를 타겟으로
        //for(int i = 0; i < generators.Length; i++)
        //{
        targetTransform = generators[currentGeneratorIndex].transform;
        NMA.SetDestination(targetTransform.position);
        if (Vector3.Distance(transform.position, targetTransform.position) < 2.0f)
        {
            if (currentGeneratorIndex < generators.Length - 1)
            {
                currentGeneratorIndex++;
                print("다음 발전기로!");
            }
            else
            {
                currentGeneratorIndex = 0;
            }
        }

        // 증거 발견 시 우선 순위에 따라 특정 증거를 타겟으로 지정하고 상태를 변경
            if (ISaw("Player", degree, maxDistance))
            {
                ChangeState(EnemyState.FindPlayer);
            }
            else if (ISaw("Trace", degree, maxDistance))
            {
                ChangeState(EnemyState.FindTrace);
            }
            //else if (ISaw("Aura", 180.0f, 1000))
            //{
            //    ChangeState(EnemyState.FindAura);
            //}           
        //}
    }

    // [EnemyState.FindAura] 아우라(오라)를 쫓는 상태
    // 타겟은 아우라가 발생한 발전기! (해당 발전기에 가기 전까지는 타겟이 사라지지 않는다. 타겟 변경은 언제든 가능)
    void ChaseAura()
    {        
        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);

        if (ISaw("Player", degree, maxDistance))  // 플레이어를 발견하면 (타겟 = 플레이어 설정)
        {
            ChangeState(EnemyState.FindPlayer);     // 플레이어 추격 상태로 전환
        }
        else if (ISaw("Trace", degree, maxDistance))  // 플레이어는 못 봤는데 흔적을 발견하면 (타겟 = 흔적 설정)
        {
            ChangeState(EnemyState.FindTrace);      // 흔적 추격 상태로 전환
        }
        else if (distance < 0.1f)       // 아우라 발생 발전기 근처에 도달했다면
        {
            targetTransform = null;                 // 타겟에서 제외한 후
            ChangeState(EnemyState.NoEvidence);     // 다시 증거 없는 상태로 돌아간다.
        }
    }

    // [EnemyState.FindTrace] 흔적을 쫓는 상태
    void ChaseTrace()
    {
        if (targetTransform != null)    // 발견한 흔적이 남아있다면
        {
            NMA.SetDestination(targetTransform.position);   // 그쪽으로 간다.
        }

        if (ISaw("Player", degree, maxDistance))   // 플레이어를 발견하면 (타겟 = 플레이어 설정)
        {
            ChangeState(EnemyState.FindPlayer);     // 플레이어 추격상태로 전환
        }
        else if (targetTransform == null)           // 플레이어를 못 찾았는데 흔적마저 놓치면
        {
            ChangeState(EnemyState.NoEvidence);     // 다시 증거 없는 상태로 돌아간다.
        }
    }

    // [EnemyState.FindPlayer] 플레이어를 쫓는 상태
    void ChasePlayer()
    {        
        NMA.SetDestination(targetTransform.position);
        print("플레이어에게 간다!");

        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if ((int)playerFSM.pyState <= 1 && distance < attackRange)   // 범위 내에서 아직 플레이어가 건강 or 부상 상태면 공격을 시도
        {
            Attack();
        }
        else if ((int)playerFSM.pyState > 1 && distance < attackRange)    // 범위 내에서 플레이어가 빈사 or 특수행동 상태면 업을 수 있다
        {
            ChangeState(EnemyState.GetPlayer);      // 업은 상태로 전환
        }
        else if (!ISaw("Player", degree, maxDistance))      // 시야에서 플레이어를 놓치면
        {
            ChangeState(EnemyState.NoEvidence);     // 다시 증거 없는 상태로 돌아간다.
        }
    }

    // [EnemyState.FindPlayer] 사정 거리 내에 플레이어가 들어오면 공격
    void Attack()
    {
        currentTime += Time.deltaTime;
        if (currentTime > attackDelay)
        {
            if (playerFSM.pyState == PlayerFSM.PlayerState.Normal)
            {
                playerFSM.pyState = PlayerFSM.PlayerState.Injured;
                currentTime = 0;
                print("Attack");
            }
            else if (playerFSM.pyState == PlayerFSM.PlayerState.Injured)
            {
                playerFSM.pyState = PlayerFSM.PlayerState.Dying;
                currentTime = 0;
                print("Attack");
            }
        }
    }

    // [EnemyState.GetPlayer] 플레이어를 업었을 때 갈고리로 향한다.
    void GoToHang()
    {
        targetTransform = hang.pillarTransform;

        hang.HangPlayerOnMe();
        print("Player를 업었다!");

        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance < 2.0f)
        {
            hang.HangPlayerOnHook();
        }
    }

    /// <summary>
    /// 시야 내 증거 발견 시 타겟으로 설정하고 true 반환, 시야 내에 없을 시 false 반환
    /// </summary>
    /// <param name="evidence">타겟의 태그(string)</param>
    /// <param name="degree">시야각: 전방 기준 탐지 각도</param>
    /// <param name="maxDistance">가시거리: 최대 감지 가능 거리</param>
    /// <returns></returns>
    bool ISaw(string evidence, float degree, float maxDistance)
    {
        GameObject[] evidences = GameObject.FindGameObjectsWithTag(evidence);
        for (int i = 0; i < evidences.Length; i++)
        {
            float distance = Vector3.Distance(evidences[i].transform.position, transform.position);
            if (distance < maxDistance)
            {
                Vector3 lookVector = evidences[i].transform.position - transform.position;
                lookVector.Normalize();

                float cosTheta = Vector3.Dot(lookVector, transform.forward);
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

                if (theta < degree)
                {
                    targetTransform = evidences[i].transform;
                    print(evidence + "를 찾음!");
                    return true;
                }
            }
        }
        targetTransform = null;
        return false;
    }

    // [EnemyState.OnGroggy] 판자 맞을 때 그로기 걸리는 기능
    public void Stuned()
    {
        print("스턴!");
        targetTransform = null;
        currentTime += Time.deltaTime;
        // 스턴 시간 동안 움직임이 없고 스턴 시간이 끝나면 다시 증거를 찾아 다님
        if (currentTime > stunTime)
        {
            ChangeState(EnemyState.NoEvidence);
            print("스턴 해제!");
        }
    }

    public void ChangeState(EnemyState newState)   // 상태 변화 기능
    {
        currentState = newState;
        currentTime = 0;
        print("상태변화: " + newState.ToString());
    }
}
