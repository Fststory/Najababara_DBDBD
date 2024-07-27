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

    enum EnemyState
    {
        NoEvidence, //���� ����
        FindAura,   // ����(�ƿ��) �߰�
        FindTrace,  // ���� �߰�
        FindPlayer, // �÷��̾� �߰�
        Attack  // ����
    }

    EnemyState currentState;    // ���� ����
    Transform playerTransform;   // �÷��̾��� Ʈ������(���� �߰� �� ���)


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // ������ٵ� ĳ��
        //currentState = EnemyState.NoEvidence;   // �ʱ� ���´� "��ȸ" ���� => �̰� ���� �ڵ� �ؿ� ���� ������ ���� �߰��� �ڵ� ���� ���� **********
        currentState = EnemyState.FindPlayer;   // ����� ���� ������ ���� **************************************************************************
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // �� ���� "Player" �±׸� ���� ������Ʈ�� Ʈ������ ĳ��
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
