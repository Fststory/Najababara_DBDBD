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
    Transform targetTransform;
    public float moveSpeed = 7.0f;
    HangPlayerHookInteraction hang;
    public int playerState=2;  // �÷��̾� ���� ���� �� ���� ����, playerState�� �̿��� ������ �Ǵ��ϴ� �κ� ���� �ʿ� *************
                                 // 0 - �ǰ�, 1 - �λ�, 2 - ���

    public float currentTime = 0;
    float delay = 2.0f;


    public enum EnemyState
    {
        NoEvidence, //���� ����
        FindAura,   // ����(�ƿ��) �߰�
        FindTrace,  // ���� �߰�
        FindPlayer, // �÷��̾� �߰�
        GetPlayer,  // �÷��̾� ����
    }

    public EnemyState currentState;    // ���� ����
    Transform playerTransform;   // �÷��̾��� Ʈ������(���� �߰� �� ���)


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // ������ٵ� ĳ��
        //currentState = EnemyState.NoEvidence;   // �ʱ� ���´� "��ȸ" ���� => �̰� ���� �ڵ� �ؿ� ���� ������ ���� �߰��� �ڵ� ���� ���� **********
        currentState = EnemyState.FindPlayer;   // ����� ���� ������ ���� **************************************************************************
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // �� ���� "Player" �±׸� ���� ������Ʈ�� Ʈ������ ĳ��
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
        }
    }

    void Wander()
    {
        // ���Ű� ������ ���ƴٴѴ�.

        // ���� �߰� �� �켱 ������ ���� Ư�� ���Ÿ� Ÿ������ �����ϰ� ���¸� ����
        if (seePlayer)
        {
            ChangeState(ChasePlayer);
        }
        else if (seeTrace)
        {
            ChangeState(ChaseTrace);
        }
        else if (seeAura)
        {
            ChangeState(ChaseAura);
        }
    }

    void ChaseAura()
    {
        targetTransform = aura.transform;
        NMA.SetDestination(targetTransform.position);

        if (seePlayer)
        {
            ChangeState(ChasePlayer);
        }
        else if (seeTrace)
        {
            ChangeState(ChaseTrace);
        }
    }

    void ChaseTrace()
    {
        targetTransform = trace.transform;
        NMA.SetDestination(targetTransform.position);

        if (seePlayer)
        {
            ChangeState(ChasePlayer);
        }
    }

    void ChasePlayer()
    {
        //dir = playerTransform.position - transform.position;
        //dir.Normalize();
        //rb.velocity = dir * moveSpeed;
        NMA.SetDestination(playerTransform.position);
        float distance = Vector3.Distance(transform.position, hang.playerObject.transform.position);
        if (playerState <= 1 && distance < 2)   // ���� ������ ���� �÷��̾ ��� ���°� �ƴϸ� ������ �õ�
        {
            Attack();
        }
        if (playerState > 1 && distance < 2)    // ���� ������ �÷��̾ ��� ���¸� ���� �� �ִ�
        {
            hang.HangPlayerOnMe();
            ChangeState(EnemyState.GetPlayer);
        }
    }

    void GetPlayer()    // �÷��̾ ������ �� ������ ���ϴ� ���
    {
        //dir = hang.pillarTransform.position - transform.position;
        //dir.Normalize();
        //dir.y = 0;
        //rb.velocity = dir * moveSpeed;
        NMA.SetDestination(hang.pillarTransform.position);
        float distance = Vector3.Distance(transform.position, hang.pillarTransform.position);
        if (distance < 2.0f)
        {
            hang.HangPlayerOnHook();
        }
    }

    void Attack()   // ���� ���
    {
        currentTime += Time.deltaTime;
        if (currentTime > delay)
        {
            playerState++;
            currentTime = 0;
        }
    }

    void ChangeState(EnemyState newState)   // ���� ��ȭ ���
    {
        currentState = newState;
    }

}
