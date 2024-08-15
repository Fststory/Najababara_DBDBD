using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // 에너미 컨트롤러.

    /*
        매 이동시 방향 전환을 위한 타겟이 필요하다(트랜스폼 컴포넌트를 받아온다)
        시야에 들어온 증거들 중에 우선순위를 따져서 타겟으로 잡는다.        
    */

    public Transform testCube;

    public NavMeshAgent NMA;

    public Animator enemyAnim;
    bool stunned = false;

    public GameObject player;
    public Transform targetTransform;

    HangPlayerHookInteraction hang;
    public bool hooking = false;

    public PlayerFSM playerFSM;         // 0 - 건강, 1 - 부상, 2 - 빈사, 3 - 특수 행동(업힘, 수리, 구조, 창문 넘기 등), 4 - 걸림

    public float attackRange = 2.0f;    // 공격 사정 거리
    float attackDelay = 2.0f;       // 공격 쿨타임
    bool cooldown = false;      // 쿨타임이면 트루

    public float currentTime = 0;

    AnimationClip animClip;

    public float degree;    // 에너미 시야각
    public float maxDistance;       // 에너미 가시거리
    public GameObject[] generators; // 맵 상의 모든 발전기
    int currentGeneratorIndex;  // 현재 탐색중인 발전기 번호 (첫 시작은 0번)

    public int rushToken = 5;  // 최초 질주 토큰 5개 보유
    float chargingTime = 0;
    //bool rushing = false;
    Vector3 knockBackDir;   // 질주 후 충돌 넉백 방향
    public float knockBackPow;     // 넉백 파워
    RushCollision rushCollision;

    List<GameObject> pallet;

    float currentSpeed;
    bool vaulting = false;

    public enum EnemyState
    {
        NoEvidence = 0, //증거 없음
        FindAura = 1,   // 오라(아우라) 발견
        FindTrace = 2,  // 흔적 발견
        FindPlayer = 3, // 플레이어 발견
        GetPlayer = 4,  // 플레이어 업음
        OnGroggy = 5,   // 조작 불능(스턴)
        Rush = 6   // 질주 판단
    }

    public EnemyState currentState;    // 현재 상태


    void Start()
    {
        currentState = EnemyState.NoEvidence;   // 초기 상태는 "배회" 상태
        hang = GetComponent<HangPlayerHookInteraction>();
        playerFSM = player.GetComponent<PlayerFSM>();
        NMA = GetComponent<NavMeshAgent>();
        generators = GameObject.FindGameObjectsWithTag("Generator");
        rushCollision = GetComponentInChildren<RushCollision>();
        pallet = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pallet"));
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
                Stuned(2);
                break;
            case EnemyState.Rush:
                Rush();
                break;
        }
    }

    private void Update()
    {
        // 질주 상태도 아니고 스턴 상태도 아닐 때 질주 토큰이 충전된다.
        if (currentState != EnemyState.Rush && currentState != EnemyState.OnGroggy)
        {
            RushTokenManage();
        }
        
        if (NMA.currentOffMeshLinkData.activated && !vaulting)
        {
            currentSpeed = NMA.speed;
            NMA.speed = 1;
            enemyAnim.SetTrigger("Vault");
            vaulting = true;
            StartCoroutine(AfterVault());
        }
    }
    
    IEnumerator AfterVault()
    {
        yield return new WaitForSeconds(0.9f);
        vaulting = false;
        NMA.speed = currentSpeed;
    }


    // [EnemyState.NoEvidence] 돌아다닌다.   
    void Wander()
    {
        enemyAnim.SetBool("Walk", true);
        // 증거가 없으면 돌아다닌다.(가장 가까운 발전기부터 순서대로.. 하고 싶었지만 그냥 씬에서 순서대로)
        // 이때 가장 가까운 발전기를 찾은 뒤 타겟으로 잡는다. 근처에 도달하면 다음 발전기를 타겟으로
        targetTransform = generators[currentGeneratorIndex].transform;
        NMA.SetDestination(targetTransform.position);

        // 발전기 근처에 왔다면...
        if (Vector3.Distance(transform.position, targetTransform.position) < 2.0f)
        {
            // 모든 발전기를 돌지 않았다면..
            if (currentGeneratorIndex < generators.Length - 1)
            {
                currentGeneratorIndex++;    // 다음 발전기로!
                print("다음 발전기로!");
            }
            else // 모든 발전기를 돌았다면..
            {
                currentGeneratorIndex = 0;  // 처음부터 다시!
            }
        }
        else if (playerFSM.pyState != PlayerFSM.PlayerState.Hooked)
        {
            if (ISaw("Player", degree, maxDistance) && playerFSM.pyState != PlayerFSM.PlayerState.Hooked)   // 발전기로 가던 도중 플레이어 발견 시
            {
                ChangeState(EnemyState.FindPlayer); // 플레이어를 쫓는다.
            }
            else if (ISaw("Trace", degree, maxDistance))    // 발전기로 가던 도중 플레이어는 못 봤지만 흔적을 봤다면
            {
                ChangeState(EnemyState.FindTrace);  // 흔적을 쫓는다.
            }
            //else if (ISaw("Aura", 180.0f, 1000))
            //{
            //    ChangeState(EnemyState.FindAura);
            //}
        }
    }

    // [EnemyState.FindAura] 아우라(오라)를 쫓는 상태
    // 타겟은 아우라가 발생한 발전기! (해당 발전기에 가기 전까지는 타겟이 사라지지 않는다. 타겟 변경은 언제든 가능)
    void ChaseAura()
    {        
        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);

        if (ISaw("Player", degree, maxDistance) && playerFSM.pyState != PlayerFSM.PlayerState.Hooked)  // 플레이어를 발견하면 (타겟 = 플레이어 설정)
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
        if (targetTransform != null)    // 발견한 흔적이 사라지지 않았다면
        {
            NMA.SetDestination(targetTransform.position);   // 그쪽으로 간다.
            if (Vector3.Distance(transform.position, targetTransform.position) < 1.0f)  // 근처에 도착했으면...
            {
                if (ISaw("Trace", degree, maxDistance)) // 다른 흔적이 없나 찾는다.
                {
                    return;
                }
            }
        }
        if(playerFSM.pyState != PlayerFSM.PlayerState.Hooked)
        {
            if (ISaw("Player", degree, maxDistance))   // 흔적을 쫓는 도중 플레이어를 발견하면
            {
                ChangeState(EnemyState.FindPlayer);     // 플레이어를 추격한다.
            }
            else if (targetTransform == null)           // 플레이어를 못 찾았는데 흔적마저 놓치면
            {
                ChangeState(EnemyState.NoEvidence);     // 남은 증거가 없기에 발전기부터 다시 돌아다닌다.
            }
        }
    }

    // [EnemyState.FindPlayer] 플레이어를 쫓는 상태
    void ChasePlayer()
    {
        NMA.SetDestination(targetTransform.position);
        print("플레이어에게 간다!");
        float distance = Vector3.Distance(transform.position, targetTransform.position);

        // 거리가 먼데(질주 거리 이상) 질주 조건을 만족한다면...(질주 토큰이 5개이고, 전방 27.6m 이내 충돌할 곳이 있다)
        if (rushToken == 5 && ISaw("Player", degree, 27.6f) && CanIRush() && distance >= 9.6f)
        {
            ChangeState(EnemyState.Rush);   // 질주 상태로 전환
            return;
        }

        if (distance < attackRange)
        {
            if ((int)playerFSM.pyState <= 1)   // 범위 내에서 아직 플레이어가 건강 or 부상 상태면 공격을 시도
            {
                Attack();
            }
            else if ((int)playerFSM.pyState > 1 && !cooldown)    // 범위 내에서 플레이어가 빈사 or 특수행동 상태면 업을 수 있다 && 공격 쿨다운이 끝났을 때 말이지!
            {
                ChangeState(EnemyState.GetPlayer);      // 업은 상태로 전환
            }
            else if (!ISaw("Player", degree, maxDistance))      // 시야에서 플레이어를 놓치면
            {
                ChangeState(EnemyState.NoEvidence);     // 다시 증거 없는 상태로 돌아간다.
            }
        }
    }

    // [EnemyState.FindPlayer] 사정 거리 내에 플레이어가 들어오면 공격
    void Attack()
    {
        animClip = Resources.Load<AnimationClip>("Attack");
        if (!cooldown)
        {
            enemyAnim.SetTrigger("Attack");
            cooldown = true;
            StartCoroutine(CheckCoolDown(animClip.length + attackDelay));
        }
    }

    IEnumerator CheckCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldown = false;
    }

    // [EnemyState.GetPlayer] 플레이어를 업었을 때 갈고리로 향한다.
    void GoToHang()
    {
        targetTransform = hang.pillarTransform;

        hang.HangPlayerOnMe();
        print("Player를 업었다!");

        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance < 2.0f && !hooking)
        {
            hooking = true;
            enemyAnim.SetTrigger("Hook");
            hang.HangPlayerOnHook();
        }
    }

    // [EnemyState.OnGroggy] 판자 맞을 때 그로기 걸리는 기능
    public void Stuned(float stunTime)
    {
        if (!stunned)
        {
            enemyAnim.SetTrigger("Stunned");
            print("스턴!");
        }

        stunned = true;
        targetTransform = null;        
        
        currentTime += Time.deltaTime;
        // 스턴 시간 동안 움직임이 없고 스턴 시간이 끝나면 다시 증거를 찾아 다님
        if (currentTime > stunTime)
        {
            stunned = false;
            ChangeState(EnemyState.NoEvidence);
            print("스턴 해제!");
            BreakPallet();
        }
    }

    // 판자 부수기
    void BreakPallet()
    {       
        if (pallet != null)
        {
            for(int i = 0; i < pallet.Count; i++)
            {
                if (pallet[i].GetComponent<PalletFSM>().palState == PalletFSM.PalletState.FallDown)
                {
                    enemyAnim.SetTrigger("BreakPallet");
                    if (currentTime > 2)    // 판자 부수는 데 걸리는 시간 (애니메이션 끝나는 시간)
                    {
                        Destroy(pallet[i]);
                        pallet.Remove(pallet[i]);
                    }
                    return;
                }
            }
        }
    }

    void RushTokenManage()  // 질주 토큰은 2초에 1개씩 차고 최대 5개 보유 가능하다.
    {        
        if (rushToken != 5)
        {            
            chargingTime += Time.deltaTime;
            if (chargingTime > 2.0f)
            {
                rushToken++;
                chargingTime = 0;
            }
        }
    }    

    // [EnemyState.Rush] 질주 상태에서 진행되는 기능
    void Rush()
    {
        enemyAnim.SetBool("Walk", false);
        enemyAnim.SetBool("Rush", true);
        //print("질주 남은 거리: " + NMA.remainingDistance);

        NMA.speed = 9.2f;   // 속도는 9.2 (m/s) 3초간 전방으로 돌진

        if (rushCollision.crashed)
        {
            NMA.speed = 4.6f;   // 충돌 시 이동 속도 정상화
            currentTime += Time.deltaTime;
            if (currentTime <= 0.2f)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + knockBackDir * knockBackPow, currentTime * 5);
                enemyAnim.SetBool("Rush", false);
                enemyAnim.SetBool("Walk", true);
            }
            else
            {
                rushCollision.crashed = false;
                print("넉백 끝남!");
                if (rushToken != 0 && CanIRush())
                {
                    print("한번 더 달린다");
                    enemyAnim.SetTrigger("Rush Again");
                    Rush();
                    currentTime = 0;
                }
                else if(currentTime > 1.25f)
                {
                    enemyAnim.SetBool("Rush", false);
                    print("이제 못 달린다");
                    ChangeState(EnemyState.OnGroggy);
                    currentTime = 0;
                }
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime > 3.0f)
            {
                NMA.speed = 4.6f;
                NMA.isStopped = true;
                NMA.isStopped = false;
                enemyAnim.SetBool("Rush", false);
                ChangeState(EnemyState.OnGroggy);
            }
        }
        testCube.position = NMA.destination + new Vector3(0, NMA.baseOffset, 0);
    }

    bool CanIRush() // 질주 조건 판단 (일부 구현) **************************************************************
    {
        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        Vector3 halfOffset = new Vector3(0, NMA.baseOffset / 2, 0);
        Ray rushRay = new Ray(transform.position - halfOffset, dir);
        //print("내 위치: " + transform.position);
        RaycastHit hitInfo;
        if (Physics.Raycast(rushRay, out hitInfo, 50, ~(1 << 8)))
        {
            print("충돌 포인트: " + hitInfo.transform.gameObject.name);
            
            transform.eulerAngles = player.transform.position - transform.position; // 질주 전 질주 방향으로 회전 => 기존에 SetDestination으로 이동할 때 발생하던 회전에 의한 어색한 충돌 해결
            NMA.SetDestination(hitInfo.point);
            knockBackDir = (transform.position - hitInfo.point).normalized;
            rushToken--;
            print("남은 질주 토큰 갯수: " + rushToken);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 시야 내 증거 발견 시 타겟으로 설정하고 true 반환, 시야 내에 없을 시 타겟을 null로 설정하고 false 반환
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

                float cosTheta = Vector3.Dot(lookVector, transform.forward);    // 두 단위 벡터를 내적해서 코사인 값을 구함
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg; // 코사인 값을 아크 코사인에 넣어 사이각을 구함

                if (theta < degree)
                {
                    targetTransform = evidences[i].transform;
                    print(evidence + "를 찾음!");
                    return true;
                }
            }
        }
        //targetTransform = null;
        return false;
    }

    public void ChangeState(EnemyState newState)   // 상태 변화 기능
    {
        currentState = newState;
        currentTime = 0;
        print("상태변화: " + newState.ToString());
    }
}
