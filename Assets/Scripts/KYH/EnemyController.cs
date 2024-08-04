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

    NavMeshAgent NMA;

    public GameObject player;
    public Transform targetTransform;
    public float moveSpeed = 7.0f;
    HangPlayerHookInteraction hang;
    public PlayerFSM playerFSM;         // 0 - �ǰ�, 1 - �λ�, 2 - ���, 3 - Ư�� �ൿ(����, ����, ����, â�� �ѱ� ��), 4 - �ɸ�
    public float attackRange = 2.0f;    // ���� ���� �Ÿ�

    public float currentTime = 0;
    float attackDelay = 2.0f;
    public float degree = 45.0f;    // ���ʹ� �þ߰�
    public float maxDistance;       // ���ʹ� ���ðŸ�
    public float stunTime = 2.0f;
    public GameObject[] generators;
    int currentGeneratorIndex = 0;


    public enum EnemyState
    {
        NoEvidence = 0, //���� ����
        FindAura = 1,   // ����(�ƿ��) �߰�
        FindTrace = 2,  // ���� �߰�
        FindPlayer = 3, // �÷��̾� �߰�
        GetPlayer = 4,  // �÷��̾� ����
        OnGroggy = 5   // ���� �Ҵ�(����)
    }

    public EnemyState currentState;    // ���� ����


    void Start()
    {
        currentState = EnemyState.NoEvidence;   // �ʱ� ���´� "��ȸ" ���� => �̰� ���� �ڵ� �ؿ� ���� ������ ���� �߰��� �ڵ� ���� ���� **********
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

    // [EnemyState.NoEvidence] ���ƴٴѴ�.   
    void Wander()
    {
        // ���Ű� ������ ���ƴٴѴ�.(���� ����� ��������� �������.. �ϰ� �;����� �׳� ������ �������)
        // �̶� ���� ����� �����⸦ ã�� �� Ÿ������ ��´�. ��ó�� �����ϸ� ���� �����⸦ Ÿ������
        //for(int i = 0; i < generators.Length; i++)
        //{
        targetTransform = generators[currentGeneratorIndex].transform;
        NMA.SetDestination(targetTransform.position);
        if (Vector3.Distance(transform.position, targetTransform.position) < 2.0f)
        {
            if (currentGeneratorIndex < generators.Length - 1)
            {
                currentGeneratorIndex++;
                print("���� �������!");
            }
            else
            {
                currentGeneratorIndex = 0;
            }
        }

        // ���� �߰� �� �켱 ������ ���� Ư�� ���Ÿ� Ÿ������ �����ϰ� ���¸� ����
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

    // [EnemyState.FindAura] �ƿ��(����)�� �Ѵ� ����
    // Ÿ���� �ƿ�� �߻��� ������! (�ش� �����⿡ ���� �������� Ÿ���� ������� �ʴ´�. Ÿ�� ������ ������ ����)
    void ChaseAura()
    {        
        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);

        if (ISaw("Player", degree, maxDistance))  // �÷��̾ �߰��ϸ� (Ÿ�� = �÷��̾� ����)
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
        if (targetTransform != null)    // �߰��� ������ �����ִٸ�
        {
            NMA.SetDestination(targetTransform.position);   // �������� ����.
        }

        if (ISaw("Player", degree, maxDistance))   // �÷��̾ �߰��ϸ� (Ÿ�� = �÷��̾� ����)
        {
            ChangeState(EnemyState.FindPlayer);     // �÷��̾� �߰ݻ��·� ��ȯ
        }
        else if (targetTransform == null)           // �÷��̾ �� ã�Ҵµ� �������� ��ġ��
        {
            ChangeState(EnemyState.NoEvidence);     // �ٽ� ���� ���� ���·� ���ư���.
        }
    }

    // [EnemyState.FindPlayer] �÷��̾ �Ѵ� ����
    void ChasePlayer()
    {        
        NMA.SetDestination(targetTransform.position);
        print("�÷��̾�� ����!");

        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if ((int)playerFSM.pyState <= 1 && distance < attackRange)   // ���� ������ ���� �÷��̾ �ǰ� or �λ� ���¸� ������ �õ�
        {
            Attack();
        }
        else if ((int)playerFSM.pyState > 1 && distance < attackRange)    // ���� ������ �÷��̾ ��� or Ư���ൿ ���¸� ���� �� �ִ�
        {
            ChangeState(EnemyState.GetPlayer);      // ���� ���·� ��ȯ
        }
        else if (!ISaw("Player", degree, maxDistance))      // �þ߿��� �÷��̾ ��ġ��
        {
            ChangeState(EnemyState.NoEvidence);     // �ٽ� ���� ���� ���·� ���ư���.
        }
    }

    // [EnemyState.FindPlayer] ���� �Ÿ� ���� �÷��̾ ������ ����
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

    /// <summary>
    /// �þ� �� ���� �߰� �� Ÿ������ �����ϰ� true ��ȯ, �þ� ���� ���� �� false ��ȯ
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

                float cosTheta = Vector3.Dot(lookVector, transform.forward);
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

                if (theta < degree)
                {
                    targetTransform = evidences[i].transform;
                    print(evidence + "�� ã��!");
                    return true;
                }
            }
        }
        targetTransform = null;
        return false;
    }

    // [EnemyState.OnGroggy] ���� ���� �� �׷α� �ɸ��� ���
    public void Stuned()
    {
        print("����!");
        targetTransform = null;
        currentTime += Time.deltaTime;
        // ���� �ð� ���� �������� ���� ���� �ð��� ������ �ٽ� ���Ÿ� ã�� �ٴ�
        if (currentTime > stunTime)
        {
            ChangeState(EnemyState.NoEvidence);
            print("���� ����!");
        }
    }

    public void ChangeState(EnemyState newState)   // ���� ��ȭ ���
    {
        currentState = newState;
        currentTime = 0;
        print("���º�ȭ: " + newState.ToString());
    }
}
