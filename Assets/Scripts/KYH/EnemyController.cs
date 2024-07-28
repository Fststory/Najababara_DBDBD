using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // ���ʹ� ��Ʈ�ѷ�.

    /*
        �� �̵��� ���� ��ȯ�� ���� Ÿ���� �ʿ��ϴ�(Ʈ������ ������Ʈ�� �޾ƿ´�)
        
    */

    Rigidbody rb;
    Vector3 dir;
    Transform targetTransform;
    public float moveSpeed = 7.0f;
    HangPlayerHookInteraction hang;
    public short playerState=2;  // �÷��̾� ���� ���� �� ���� ����*************
                                 // 0 - �ǰ�, 1 - �λ�, 2 - ���

    public float currentTime = 0;
    float delay = 2.0f;


    enum EnemyState
    {
        NoEvidence, //���� ����
        FindAura,   // ����(�ƿ��) �߰�
        FindTrace,  // ���� �߰�
        FindPlayer, // �÷��̾� �߰�
        GetPlayer,  // �÷��̾� ����
    }

    EnemyState currentState;    // ���� ����
    Transform playerTransform;   // �÷��̾��� Ʈ������(���� �߰� �� ���)


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // ������ٵ� ĳ��
        //currentState = EnemyState.NoEvidence;   // �ʱ� ���´� "��ȸ" ���� => �̰� ���� �ڵ� �ؿ� ���� ������ ���� �߰��� �ڵ� ���� ���� **********
        currentState = EnemyState.FindPlayer;   // ����� ���� ������ ���� **************************************************************************
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // �� ���� "Player" �±׸� ���� ������Ʈ�� Ʈ������ ĳ��
        hang = GetComponent<HangPlayerHookInteraction>();
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
        dir.Normalize();
        rb.velocity = dir * moveSpeed;
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
        dir = hang.pillarTransform.position - transform.position;
        dir.Normalize();
        dir.y = 0;
        rb.velocity = dir * moveSpeed;
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
