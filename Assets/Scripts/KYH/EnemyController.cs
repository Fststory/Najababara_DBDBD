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

    public Transform targetTransform;
    public float moveSpeed = 7.0f;
    HangPlayerHookInteraction hang;
    public int playerState = 2;  // �÷��̾� ���� ���� �� ���� ����, playerState�� �̿��� ������ �Ǵ��ϴ� �κ� ���� �ʿ� *************
                               // 0 - �ǰ�, 1 - �λ�, 2 - ���, 3 - ����, 4 - �ɸ�
    public float attackRange = 2.0f;    // ���� ���� �Ÿ�

    public float currentTime = 0;
    float attackDelay = 2.0f;
    public float degree = 45.0f;    // ���ʹ� �þ߰�
    public float maxDistance;       // ���ʹ� ���ðŸ�
    public float stunTime = 2.0f;
    public GameObject[] generators;


    public enum EnemyState
    {
        NoEvidence, //���� ����
        FindAura,   // ����(�ƿ��) �߰�
        FindTrace,  // ���� �߰�
        FindPlayer, // �÷��̾� �߰�
        GetPlayer,  // �÷��̾� ����
        OnGroggy    // ���� �Ҵ�(����)
    }

    public EnemyState currentState;    // ���� ����


    void Start()
    {
        currentState = EnemyState.NoEvidence;   // �ʱ� ���´� "��ȸ" ���� => �̰� ���� �ڵ� �ؿ� ���� ������ ���� �߰��� �ڵ� ���� ���� **********
        hang = GetComponent<HangPlayerHookInteraction>();
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
        targetTransform = generators[0].transform;
        NMA.SetDestination(targetTransform.position);

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
            else if (targetTransform == null)
            {
                ChangeState(EnemyState.NoEvidence);
            }
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
        //if (ISaw("Player", degree, maxDistance))    // �þ� ���� �÷��̾ ������ (Ÿ�� = �÷��̾� ������ ���ÿ� if�� ����)
        //{
            NMA.SetDestination(targetTransform.position);
            print("�÷��̾�� ����!");

            float distance = Vector3.Distance(transform.position, targetTransform.position);
            if (playerState <= 1 && distance < attackRange)   // ���� ������ ���� �÷��̾ �ǰ� or �λ� ���¸� ������ �õ�
            {
                Attack();
            }
            else if (playerState > 1 && distance < attackRange)    // ���� ������ �÷��̾ ��� or Ư���ൿ ���¸� ���� �� �ִ�
            {
                ChangeState(EnemyState.GetPlayer);
            }
        //}
        else if (targetTransform == null)           // �þ߿��� �÷��̾ ��ġ��
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
            playerState++;
            currentTime = 0;
            print("Attack");
        }
    }

    // [EnemyState.GetPlayer] �÷��̾ ������ �� ������ ���Ѵ�.
    void GoToHang()
    {
        hang.HangPlayerOnMe();
        print("Player�� ������!");

        targetTransform = hang.pillarTransform;
        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance < 2.0f)
        {
            hang.HangPlayerOnHook();
        }
    }

    #region �þ� �� (�÷��̾�, ����, ����) Ž�� �Լ��� ver.~240731
    /// <summary>
    /// �þ� �� �÷��̾� Ž��, �߰� �� targetTransform�� �÷��̾ ��� true ��ȯ
    /// </summary>
    /// <param name = "degree" > �þ߰�: ���� ���� Ž�� ����</param>
    /// <param name = "maxDistance" > ���ðŸ�: �ִ� ���� ���� �Ÿ�</param>
    /// <returns></returns>
    //bool SeePlayerInSight(float degree, float maxDistance)
    //{
    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //    for (int i = 0; i < players.Length; i++)
    //    {
    //        float distance = Vector3.Distance(players[i].transform.position, transform.position);
    //        if (distance < maxDistance)
    //        {
    //            Vector3 lookVector = players[i].transform.position - transform.position;
    //            lookVector.Normalize();

    //            float cosTheta = Vector3.Dot(lookVector, transform.forward);
    //            float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

    //            if (theta < degree)
    //            {
    //                targetTransform = players[i].transform;
    //                return true;    // �߰������� �˸��� ����
    //            }
    //        }
    //    }
    //    return false;
    //}

    //bool SeeTraceInSight(float degree, float maxDistance)   // �þ� �� ���� Ž�� ���. �þ߰�:degree, ���ðŸ�:maxDistance
    //{                                                           // ���ðŸ��� �ִ��� Ȯ�� -> Ÿ�ٰ� �� ������ ������ �þ߰� ��
    //    GameObject[] traces = GameObject.FindGameObjectsWithTag("Trace");
    //    for (int i = 0; i < traces.Length; i++)
    //    {
    //        float distance = Vector3.Distance(traces[i].transform.position, transform.position);
    //        if (distance < maxDistance)
    //        {
    //            Vector3 lookVector = traces[i].transform.position - transform.position;
    //            lookVector.Normalize();

    //            float cosTheta = Vector3.Dot(lookVector, transform.forward);
    //            float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

    //            if (theta < degree)
    //            {
    //                targetTransform = traces[i].transform;
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}

    //bool SeeAuraInSight(float degree, float maxDistance)   // �þ� �� ���� Ž�� ���. �þ߰�:degree, ���ðŸ�:maxDistance
    //{                                                           // ���ðŸ��� �ִ��� Ȯ�� -> Ÿ�ٰ� �� ������ ������ �þ߰� ��
    //    GameObject[] auras = GameObject.FindGameObjectsWithTag("Aura");
    //    for (int i = 0; i < auras.Length; i++)
    //    {
    //        float distance = Vector3.Distance(auras[i].transform.position, transform.position);
    //        if (distance < maxDistance)
    //        {
    //            Vector3 lookVector = auras[i].transform.position - transform.position;
    //            lookVector.Normalize();

    //            float cosTheta = Vector3.Dot(lookVector, transform.forward);
    //            float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

    //            if (theta < degree)
    //            {
    //                targetTransform = auras[i].transform;
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}
    #endregion

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

    void Stuned() // ���� ���� �� �׷α� �ɸ��� ���
    {
        currentTime += Time.deltaTime;
        // ���� �ð� ���� �������� ���� ���� �ð��� ������ �ٽ� ���Ÿ� ã�� �ٴ�
        if (currentTime > stunTime)
        {
            ChangeState(EnemyState.NoEvidence);
        }
    }

    void ChangeState(EnemyState newState)   // ���� ��ȭ ���
    {
        currentState = newState;
        currentTime = 0;
        print("���º�ȭ: " + newState.ToString());
    }
}
