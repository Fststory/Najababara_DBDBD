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
    Rigidbody rb;
    Vector3 knockBackDir;
    public float knockBackPow;

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
    public GameObject[] generators; // �� ���� ��� ������
    int currentGeneratorIndex = 0;  // ���� Ž������ ������ ��ȣ

    public int rushToken = 5;  // ���� ���� ��ū 5�� ����
    bool rushing = false;


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
        currentState = EnemyState.NoEvidence;   // �ʱ� ���´� "��ȸ" ���� => �̰� ���� �ڵ� �ؿ� ���� ������ ���� �߰��� �ڵ� ���� ���� **********
        hang = GetComponent<HangPlayerHookInteraction>();
        playerFSM = player.GetComponent<PlayerFSM>();
        NMA = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
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
                Stuned(2);
                break;
            case EnemyState.Rush:
                Rush();
                break;
        }
    }

    // [EnemyState.NoEvidence] ���ƴٴѴ�.   
    void Wander()
    {
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
        else if (ISaw("Player", degree, maxDistance))   // ������� ���� ���� �÷��̾� �߰� ��
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
        if (targetTransform != null)    // �߰��� ������ ������� �ʾҴٸ�
        {
            NMA.SetDestination(targetTransform.position);   // �������� ����.
            if (Vector3.Distance(transform.position, targetTransform.position) < 3.0f)  // ��ó�� ����������...
            {
                if (ISaw("Trace", degree, maxDistance)) // �ٸ� ������ ���� ã�´�.
                {
                    return;
                }
            }
        }
        if (ISaw("Player", degree, maxDistance))   // ������ �Ѵ� ���� �÷��̾ �߰��ϸ�
        {
            ChangeState(EnemyState.FindPlayer);     // �÷��̾ �߰��Ѵ�.
        }
        else if (targetTransform == null)           // �÷��̾ �� ã�Ҵµ� �������� ��ġ��
        {
            ChangeState(EnemyState.NoEvidence);     // ���� ���Ű� ���⿡ ��������� �ٽ� ���ƴٴѴ�.
        }
    }

    // [EnemyState.FindPlayer] �÷��̾ �Ѵ� ����
    void ChasePlayer()
    {        
        NMA.SetDestination(targetTransform.position);
        print("�÷��̾�� ����!");

        float distance = Vector3.Distance(transform.position, targetTransform.position);
        
        // ���� ������ �����Ѵٸ�...(���� ��ū�� 5���̰�, ���� 27.6m �̳� �浹�� ���� �ִ�)
        if (rushToken == 5 && ISaw("Player", degree, 27.6f) && CanIRush())
        {
            ChangeState(EnemyState.Rush);   // ���� ���·� ��ȯ
        }
        else if ((int)playerFSM.pyState <= 1 && distance < attackRange)   // ���� ������ ���� �÷��̾ �ǰ� or �λ� ���¸� ������ �õ�
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

    // [EnemyState.OnGroggy] ���� ���� �� �׷α� �ɸ��� ���
    public void Stuned(float stunTime)
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
    void RushTokenManage()
    {
        // ���� ��ū�� 2�ʿ� 1���� ���� �ִ� 5�� ���� �����ϴ�.
        if (rushToken != 5)
        {
            currentTime += Time.deltaTime;
            if (currentTime > 2.0f)
            {
                rushToken++;
                currentTime = 0;
            }
        }
    }    

    // [EnemyState.Rush] ���� ���¿��� ����Ǵ� ���
    void Rush()
    {
        if (rushing) return;    // �̹� ���� ���̸� �߰� ȣ�� ����       [gpt �ߺ� ȣ�� ����]
        rushing = true;

        NMA.speed = 9.2f;   // �ӵ��� 9.2 (m/s) 3�ʰ� �������� ����
        print("����!");

        Vector3 destination = NMA.destination;
        destination.y = 1;
        float distance = Vector3.Distance(destination, transform.position);

        if (distance < 1.6f)
        {
            NMA.ResetPath();
            print("��� ����!");
            currentTime += Time.deltaTime;
            if (currentTime <= 1)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + knockBackDir * knockBackPow, currentTime);
            }
            else
            {
                NMA.speed = 4.6f;   // ���� ���� �� ���� �ӵ��� ���ƿ´�.
                print("�˹� ����!");
                rushing = false;
                currentTime = 0;
                if (rushToken != 0 && CanIRush())
                {
                    Rush();
                    rushToken--;
                }
            }
        }       
        print(NMA.remainingDistance);
        print(NMA.destination);
        print(distance);
    }

    bool CanIRush() // ���� ���� �Ǵ� (�̱���) **************************************************************
    {
        Vector3 dir = targetTransform.position - transform.position;
        dir.y = transform.position.y;
        Ray rushRay = new Ray(transform.position, dir);
        RaycastHit hitInfo;
        if (Physics.Raycast(rushRay, out hitInfo, 27.6f, ~(1<<8)))
        {
            NMA.SetDestination(hitInfo.point);
            knockBackDir = (transform.position - hitInfo.point).normalized;
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

    private void OnDrawGizmos()
    {
        Vector3 dir = targetTransform.position - transform.position;
        dir.y = transform.position.y;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,dir);
    }
}
