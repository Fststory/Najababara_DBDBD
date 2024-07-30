using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarFSM : MonoBehaviour
{
    enum pillarState
    {
        NoSacrifice,    // ���� ����
        SacrificeLV1,   // �ش� ���� ���൵ 1
        SacrificeLV2,   // �ش� ���� ���൵ 2
        AbsorbSacrifice,  // ���� ����
        Damaged   // ������
    }

    public GameObject player;   // �÷��̾� ������Ʈ
    pillarState currentState;   // ���� ����

    float currentTime = 0;  // ���� �ð�
    public float eatTime;   // ��� �ð�
    public float damagedTime;  // �ν����µ� �ɸ��� �ð�
    public float repairTime;    // ���� �ð�

    void Start()
    {
        
    }

    void Update()
    {
        switch (currentState)
        {
            case pillarState.Damaged:
                SelfRepair();
                break;
            case pillarState.NoSacrifice:
                WaitSacrifice();
                break;
            case pillarState.SacrificeLV1:
                GiveFalseHope();
                break;
            case pillarState.SacrificeLV2:
                TryToAbsorb();
                break;
            case pillarState.AbsorbSacrifice:
                DisappearAfterMeal();
                break;
        }
    }

    private void SelfRepair()   // �÷��̾ �����߸��� ������ �����ϴ� ���
    {
        currentTime += Time.deltaTime;
        if (currentTime > repairTime)
        {
            ChangeState(pillarState.NoSacrifice);
        }
    }

    private void WaitSacrifice()    // ������ �ɸ��� ���¸� �������� ���� �¼����� ��ȯ ���/ �÷��̾� ü�� & ���� ���� �� �ش� ������ �Ǵ� **********************
    {                                                                                       // �ϴ� player hp, state�� ǥ��
        //if (player.GetComponent<>().hp > 50.0f)
        //{
        //    ChangeState(pillarState.SacrificeLV1);
        //}
        //else if (1.0f < player.GetComponent<>().hp %% player.GetComponent<>().hp < 50.0f)
        //{
        //    ChangeState(pillarState.SacrificeLV2);
        //}else if( player.GetComponent<>().hp < 1.0f || player.GetComponent<>().state == 2)
    }
    private void GiveFalseHope()    // ������ϴ� ���
    {
        // Ż�� �õ� ��ȸ�� ��
        // ���� Ȯ���� 4%
        // ���� �� ���� ü�¿��� 16.666% ü�� ����

    }

    private void TryToAbsorb()      // ��Ƹ��� �õ��� �ϴ� ���
    {
        // ���� �ð����� ���� �й�
        // ��ų üũ �� �� ���� ���� �� ó��
        ChangeState(pillarState.AbsorbSacrifice);
    }

    private void DisappearAfterMeal()        // �� �԰� ������� ���
    {
        Destroy(player.gameObject);

        currentTime += Time.deltaTime;
        if (currentTime > eatTime)
        {
            Destroy(gameObject);
        }
    }

    void ChangeState(pillarState newState)      // ���� ��ȯ ���
    {
        currentState = newState;
    }

    public void TakeDamage()    // ������ ������ �������� ������ ���
    {
        ChangeState(pillarState.Damaged);
    }

    private void OnTriggerStay(Collider other)  // �÷��̾ ������ �ɸ��� ���� ���·� ��ȣ�ۿ� ������ ���� �� ��� ������ ���
    {
        //if (other.CompareTag("Player") && playerState != playerState.Hooked)    // �÷��̾� ���� ���� �� �׿� �°� ���� *********************
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentTime += Time.deltaTime;
                if (currentTime > damagedTime)
                {
                    TakeDamage();
                }
            }
        }
    }
}
