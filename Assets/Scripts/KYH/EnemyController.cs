using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // ���ʹ� ��Ʈ�ѷ�.

    /*
        �� �̵��� ���� ��ȯ�� ���� Ÿ���� �ʿ��ϴ�(Ʈ������ ������Ʈ�� �޾ƿ´�)
        �þ߿� ���� ���ŵ� �߿� �켱������ ������ Ÿ������ ��´�.        
    */

    public Transform testCube;

    public NavMeshAgent NMA;

    public Animator enemyAnim;
    bool stunned = false;

    public GameObject player;
    public Transform targetTransform;
    public float moveSpeed = 7.0f;

    HangPlayerHookInteraction hang;
    public PlayerFSM playerFSM;         // 0 - �ǰ�, 1 - �λ�, 2 - ���, 3 - Ư�� �ൿ(����, ����, ����, â�� �ѱ� ��), 4 - �ɸ�

    public float attackRange = 2.0f;    // ���� ���� �Ÿ�
    float attackDelay = 2.0f;       // ���� ��Ÿ��

    public float currentTime = 0;

    public float degree = 45.0f;    // ���ʹ� �þ߰�
    public float maxDistance;       // ���ʹ� ���ðŸ�
    public GameObject[] generators; // �� ���� ��� ������
    int currentGeneratorIndex = 0;  // ���� Ž������ ������ ��ȣ

    public int rushToken = 5;  // ���� ���� ��ū 5�� ����
    float chargingTime = 0;
    //bool rushing = false;
    Vector3 knockBackDir;   // ���� �� �浹 �˹� ����
    public float knockBackPow;     // �˹� �Ŀ�
    RushCollision rushCollision;

    List<GameObject> pallet;

    float currentSpeed;
    bool vaulting = false;

    public enum EnemyState
    {
        NoEvidence = 0, //���� ����
        FindAura = 1,   // ����(�ƿ��) �߰�
        FindTrace = 2,  // ���� �߰�
        FindPlayer = 3, // �÷��̾� �߰�
        GetPlayer = 4,  // �÷��̾� ����
        OnGroggy = 5,   // ���� �Ҵ�(����)
        Rush = 6   // ���� �Ǵ�
    }

    public EnemyState currentState;    // ���� ����


    void Start()
    {
        currentState = EnemyState.NoEvidence;   // �ʱ� ���´� "��ȸ" ����
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
        // ���� ���µ� �ƴϰ� ���� ���µ� �ƴ� �� ���� ��ū�� �����ȴ�.
        if (currentState != EnemyState.Rush && currentState != EnemyState.OnGroggy)
        {
            RushTokenManage();
        }

        //if (NMA.isOnOffMeshLink)
        //{
        //    if (NMA.currentOffMeshLinkData.activated && !vaulting)
        //    {
        //        currentSpeed = NMA.speed;
        //        NMA.speed = 1;
        //        enemyAnim.SetTrigger("Vault");
        //        vaulting = true;
        //    }
            
        //    if(!NMA.currentOffMeshLinkData.activated)
        //    {
        //        vaulting = false;
        //        NMA.speed = currentSpeed;
        //    }
        //}
    }

    // [EnemyState.NoEvidence] ���ƴٴѴ�.   
    void Wander()
    {
        enemyAnim.SetBool("Walk", true);
        // ���Ű� ������ ���ƴٴѴ�.(���� ����� ��������� �������.. �ϰ� �;����� �׳� ������ �������)
        // �̶� ���� ����� �����⸦ ã�� �� Ÿ������ ��´�. ��ó�� �����ϸ� ���� �����⸦ Ÿ������
        targetTransform = generators[currentGeneratorIndex].transform;
        NMA.SetDestination(targetTransform.position);

        // ������ ��ó�� �Դٸ�...
        if (Vector3.Distance(transform.position, targetTransform.position) < 2.0f)
        {
            // ��� �����⸦ ���� �ʾҴٸ�..
            if (currentGeneratorIndex < generators.Length - 1)
            {
                currentGeneratorIndex++;    // ���� �������!
                print("���� �������!");
            }
            else // ��� �����⸦ ���Ҵٸ�..
            {
                currentGeneratorIndex = 0;  // ó������ �ٽ�!
            }
        }
        else if (playerFSM.pyState != PlayerFSM.PlayerState.Hooked)
        {
            if (ISaw("Player", degree, maxDistance) && playerFSM.pyState != PlayerFSM.PlayerState.Hooked)   // ������� ���� ���� �÷��̾� �߰� ��
            {
                ChangeState(EnemyState.FindPlayer); // �÷��̾ �Ѵ´�.
            }
            else if (ISaw("Trace", degree, maxDistance))    // ������� ���� ���� �÷��̾�� �� ������ ������ �ôٸ�
            {
                ChangeState(EnemyState.FindTrace);  // ������ �Ѵ´�.
            }
            //else if (ISaw("Aura", 180.0f, 1000))
            //{
            //    ChangeState(EnemyState.FindAura);
            //}
        }
    }

    // [EnemyState.FindAura] �ƿ��(����)�� �Ѵ� ����
    // Ÿ���� �ƿ�� �߻��� ������! (�ش� �����⿡ ���� �������� Ÿ���� ������� �ʴ´�. Ÿ�� ������ ������ ����)
    void ChaseAura()
    {        
        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);

        if (ISaw("Player", degree, maxDistance) && playerFSM.pyState != PlayerFSM.PlayerState.Hooked)  // �÷��̾ �߰��ϸ� (Ÿ�� = �÷��̾� ����)
        {
            ChangeState(EnemyState.FindPlayer);     // �÷��̾� �߰� ���·� ��ȯ
        }
        else if (ISaw("Trace", degree, maxDistance))  // �÷��̾�� �� �ôµ� ������ �߰��ϸ� (Ÿ�� = ���� ����)
        {
            ChangeState(EnemyState.FindTrace);      // ���� �߰� ���·� ��ȯ
        }
        else if (distance < 0.1f)       // �ƿ�� �߻� ������ ��ó�� �����ߴٸ�
        {
            targetTransform = null;                 // Ÿ�ٿ��� ������ ��
            ChangeState(EnemyState.NoEvidence);     // �ٽ� ���� ���� ���·� ���ư���.
        }
    }

    // [EnemyState.FindTrace] ������ �Ѵ� ����
    void ChaseTrace()
    {
        if (targetTransform != null)    // �߰��� ������ ������� �ʾҴٸ�
        {
            NMA.SetDestination(targetTransform.position);   // �������� ����.
            if (Vector3.Distance(transform.position, targetTransform.position) < 1.0f)  // ��ó�� ����������...
            {
                if (ISaw("Trace", degree, maxDistance)) // �ٸ� ������ ���� ã�´�.
                {
                    return;
                }
            }
        }
        if(playerFSM.pyState != PlayerFSM.PlayerState.Hooked)
        {
            if (ISaw("Player", degree, maxDistance))   // ������ �Ѵ� ���� �÷��̾ �߰��ϸ�
            {
                ChangeState(EnemyState.FindPlayer);     // �÷��̾ �߰��Ѵ�.
            }
            else if (targetTransform == null)           // �÷��̾ �� ã�Ҵµ� �������� ��ġ��
            {
                ChangeState(EnemyState.NoEvidence);     // ���� ���Ű� ���⿡ ��������� �ٽ� ���ƴٴѴ�.
            }
        }
    }

    // [EnemyState.FindPlayer] �÷��̾ �Ѵ� ����
    void ChasePlayer()
    {
        NMA.SetDestination(targetTransform.position);
        print("�÷��̾�� ����!");
        float distance = Vector3.Distance(transform.position, targetTransform.position);

        // �Ÿ��� �յ�(���� �Ÿ� �̻�) ���� ������ �����Ѵٸ�...(���� ��ū�� 5���̰�, ���� 27.6m �̳� �浹�� ���� �ִ�)
        if (rushToken == 5 && ISaw("Player", degree, 27.6f) && CanIRush() && distance >= 9.6f)
        {
            ChangeState(EnemyState.Rush);   // ���� ���·� ��ȯ
            return;
        }

        if (distance < attackRange)
        {
            if ((int)playerFSM.pyState <= 1)   // ���� ������ ���� �÷��̾ �ǰ� or �λ� ���¸� ������ �õ�
            {
                Attack();
            }
            else if ((int)playerFSM.pyState > 1)    // ���� ������ �÷��̾ ��� or Ư���ൿ ���¸� ���� �� �ִ�
            {
                ChangeState(EnemyState.GetPlayer);      // ���� ���·� ��ȯ
            }
            else if (!ISaw("Player", degree, maxDistance))      // �þ߿��� �÷��̾ ��ġ��
            {
                ChangeState(EnemyState.NoEvidence);     // �ٽ� ���� ���� ���·� ���ư���.
            }
        }
    }

    // [EnemyState.FindPlayer] ���� �Ÿ� ���� �÷��̾ ������ ����
    void Attack()
    {
        float dir = Vector3.Distance(player.transform.position, transform.position);
        if (dir < attackRange)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                enemyAnim.SetTrigger("Attack");
                currentTime = 0;
            }
        }
    }

    // [EnemyState.GetPlayer] �÷��̾ ������ �� ������ ���Ѵ�.
    void GoToHang()
    {
        targetTransform = hang.pillarTransform;

        hang.HangPlayerOnMe();
        print("Player�� ������!");

        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance < 2.0f)
        {
            hang.HangPlayerOnHook();
        }
    }

    // [EnemyState.OnGroggy] ���� ���� �� �׷α� �ɸ��� ���
    public void Stuned(float stunTime)
    {
        if (!stunned)
        {
            enemyAnim.SetTrigger("Stunned");
            print("����!");
        }

        stunned = true;
        targetTransform = null;        
        
        currentTime += Time.deltaTime;
        // ���� �ð� ���� �������� ���� ���� �ð��� ������ �ٽ� ���Ÿ� ã�� �ٴ�
        if (currentTime > stunTime)
        {
            stunned = false;
            ChangeState(EnemyState.NoEvidence);
            print("���� ����!");
            BreakPallet();
        }
    }

    void BreakPallet()
    {       
        if (pallet != null)
        {
            for(int i = 0; i < pallet.Count; i++)
            {
                if (pallet[i].GetComponent<PalletFSM>().palState == PalletFSM.PalletState.FallDown)
                {
                    enemyAnim.SetTrigger("BreakPallet");
                    if (currentTime > 2)    // ���� �μ��� �� �ɸ��� �ð� (�ִϸ��̼� ������ �ð�)
                    {
                        Destroy(pallet[i]);
                        pallet.Remove(pallet[i]);
                    }
                    return;
                }
            }
        }
    }

    void RushTokenManage()
    {
        // ���� ��ū�� 2�ʿ� 1���� ���� �ִ� 5�� ���� �����ϴ�.
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

    // [EnemyState.Rush] ���� ���¿��� ����Ǵ� ���
    void Rush()
    {
        enemyAnim.SetBool("Walk", false);
        enemyAnim.SetBool("Rush", true);
        //print("���� ���� �Ÿ�: " + NMA.remainingDistance);

        NMA.speed = 9.2f;   // �ӵ��� 9.2 (m/s) 3�ʰ� �������� ����
        //print("����!");

        if (rushCollision.crashed)
        {
            NMA.speed = 4.6f;   // �浹 �� �̵� �ӵ� ����ȭ
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
                print("�˹� ����!");
                if (rushToken != 0 && CanIRush())
                {
                    print("�ѹ� �� �޸���");
                    enemyAnim.SetTrigger("Rush Again");
                    Rush();
                    currentTime = 0;
                }
                else if(currentTime > 1.25f)
                {
                    enemyAnim.SetBool("Rush", false);
                    print("���� �� �޸���");
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

    bool CanIRush() // ���� ���� �Ǵ� (�Ϻ� ����) **************************************************************
    {
        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        Vector3 halfOffset = new Vector3(0, NMA.baseOffset / 2, 0);
        Ray rushRay = new Ray(transform.position - halfOffset, dir);
        //print("�� ��ġ: " + transform.position);
        RaycastHit hitInfo;
        if (Physics.Raycast(rushRay, out hitInfo, 50, ~(1 << 8)))
        {
            print("�浹 ����Ʈ: " + hitInfo.transform.gameObject.name);
            
            transform.eulerAngles = player.transform.position - transform.position; // ���� �� ���� �������� ȸ�� => ������ SetDestination���� �̵��� �� �߻��ϴ� ȸ���� ���� ����� �浹 �ذ�
            NMA.SetDestination(hitInfo.point);
            knockBackDir = (transform.position - hitInfo.point).normalized;
            rushToken--;
            print("���� ���� ��ū ����: " + rushToken);
            return true;
        }
        return false;
    }

    /// <summary>
    /// �þ� �� ���� �߰� �� Ÿ������ �����ϰ� true ��ȯ, �þ� ���� ���� �� Ÿ���� null�� �����ϰ� false ��ȯ
    /// </summary>
    /// <param name="evidence">Ÿ���� �±�(string)</param>
    /// <param name="degree">�þ߰�: ���� ���� Ž�� ����</param>
    /// <param name="maxDistance">���ðŸ�: �ִ� ���� ���� �Ÿ�</param>
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

                float cosTheta = Vector3.Dot(lookVector, transform.forward);    // �� ���� ���͸� �����ؼ� �ڻ��� ���� ����
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg; // �ڻ��� ���� ��ũ �ڻ��ο� �־� ���̰��� ����

                if (theta < degree)
                {
                    targetTransform = evidences[i].transform;
                    print(evidence + "�� ã��!");
                    return true;
                }
            }
        }
        //targetTransform = null;
        return false;
    }

    public void ChangeState(EnemyState newState)   // ���� ��ȭ ���
    {
        currentState = newState;
        currentTime = 0;
        print("���º�ȭ: " + newState.ToString());
    }
}
