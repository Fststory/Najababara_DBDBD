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
        
    */

    NavMeshAgent NMA;

    Rigidbody rb;
    Vector3 dir;
    public Transform targetTransform;
    public float moveSpeed = 7.0f;
    HangPlayerHookInteraction hang;
    public int playerState=2;  // �÷��̾� ���� ���� �� ���� ����, playerState�� �̿��� ������ �Ǵ��ϴ� �κ� ���� �ʿ� *************
                               // 0 - �ǰ�, 1 - �λ�, 2 - ���
    public float attackRange = 2.0f;    // ���� ���� �Ÿ�

    public float currentTime = 0;
    float attackDelay = 2.0f;
    public float degree = 45.0f;    // ���ʹ� �þ߰�
    public float maxDistance;       // ���ʹ� ���ðŸ�
    public float stunTime = 2.0f;


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
        rb = GetComponent<Rigidbody>(); // ������ٵ� ĳ��
        currentState = EnemyState.NoEvidence;   // �ʱ� ���´� "��ȸ" ���� => �̰� ���� �ڵ� �ؿ� ���� ������ ���� �߰��� �ڵ� ���� ���� **********
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
        

    void Wander()   // ���ƴٴѴ�.
    {
        // ���Ű� ������ ���ƴٴѴ�.(���� ����� ��������� �������)
        //targetTransform = generator.transform;

        // ���� �߰� �� �켱 ������ ���� Ư�� ���Ÿ� Ÿ������ �����ϰ� ���¸� ����
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

    void ChaseAura()    // �ƿ��(����)�� �Ѵ´�.
    {
        NMA.SetDestination(targetTransform.position);

        if (SeePlayerInSight(degree, maxDistance))  // �÷��̾ �߰��ϸ�
        {
            ChangeState(EnemyState.FindPlayer);     // �÷��̾ �Ѵ´�.
        }
        else if (SeeTraceInSight(degree, maxDistance))  // ������ �߰��ϸ�
        {
            ChangeState(EnemyState.FindTrace);      // ������ �Ѵ´�.
        }
        else if (targetTransform == null)           // �÷��̾� or ������ �� ã�Ҵµ� �ƿ���� ��ġ��
        {
            ChangeState(EnemyState.NoEvidence);     // �ٽ� ���� ���� ���·� ���ư���.
        }
    }

    void ChaseTrace()   // ������ �Ѵ´�.
    {
        NMA.SetDestination(targetTransform.position);

        if (SeePlayerInSight(degree,maxDistance))   // �÷��̾ �߰��ϸ�
        {
            ChangeState(EnemyState.FindPlayer);     // �÷��̾ �Ѵ´�.
        }
        else if (targetTransform == null)           // �÷��̾ �� ã�Ҵµ� �������� ��ġ��
        {
            ChangeState(EnemyState.NoEvidence);     // �ٽ� ���� ���� ���·� ���ư���.
        }
    }

    void ChasePlayer()
    {
        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, hang.playerObject.transform.position);
        if (playerState <= 1 && distance < attackRange)   // ���� ������ ���� �÷��̾ ��� ���°� �ƴϸ� ������ �õ�
        {
            Attack();
        }
        if (playerState > 1 && distance < attackRange)    // ���� ������ �÷��̾ ��� ���¸� ���� �� �ִ�
        {
            hang.HangPlayerOnMe();
            ChangeState(EnemyState.GetPlayer);
        }
    }

    void GetPlayer()    // �÷��̾ ������ �� ������ ���ϴ� ���
    {
        targetTransform = hang.pillarTransform;
        NMA.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance < 2.0f)
        {
            hang.HangPlayerOnHook();
        }
    }

    void Attack()   // ���� ���
    {
        currentTime += Time.deltaTime;
        if (currentTime > attackDelay)
        {
            playerState++;
            currentTime = 0;
        }
    }

    bool SeePlayerInSight(float degree, float maxDistance)   // �þ� �� �÷��̾� Ž�� ���. �þ߰�:degree, ���ðŸ�:maxDistance
    {                                                           // ���ðŸ��� �ִ��� Ȯ�� -> Ÿ�ٰ� �� ������ ������ �þ߰� ��
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

    bool SeeTraceInSight(float degree, float maxDistance)   // �þ� �� ���� Ž�� ���. �þ߰�:degree, ���ðŸ�:maxDistance
    {                                                           // ���ðŸ��� �ִ��� Ȯ�� -> Ÿ�ٰ� �� ������ ������ �þ߰� ��
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

    bool SeeAuraInSight(float degree, float maxDistance)   // �þ� �� ���� Ž�� ���. �þ߰�:degree, ���ðŸ�:maxDistance
    {                                                           // ���ðŸ��� �ִ��� Ȯ�� -> Ÿ�ٰ� �� ������ ������ �þ߰� ��
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
    }
}
