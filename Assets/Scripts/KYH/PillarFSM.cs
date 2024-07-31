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
    //public PlayerController playerController;   // �÷��̾� ��Ʈ�ѷ� ������Ʈ
    pillarState currentState;   // ���� ���� ����

    float currentTime = 0;  // ���� �ð�
    public float eatTime;   // ��� �ð�
    public float damagedTime;  // �ν����µ� �ɸ��� �ð�
    public float repairTime;    // ���� �ð�

    void Start()
    {
        currentState = pillarState.NoSacrifice;
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

    

    private void WaitSacrifice()    // ������ �ɸ��� ���¸� �������� ���� �¼����� ��ȯ ���/ �÷��̾� ü�� & ���� ���� �� �ش� ������ �Ǵ� **********************
    {                                                                                       // �ϴ� player hp, state�� ǥ��
        //if (playerState == 4)   // �÷��̾ �ɷ����� ü�� Ȯ��!
        //{
        //    if (playerController.hp > 50.0f)
        //    {
        //        ChangeState(pillarState.SacrificeLV1);
        //    }
        //    else if (1.0f < playerController.hp && playerController.hp < 50.0f
        //    || playerController.alreadyHooked) playerController�� bool alreadyHooked �߰�!
        //    {
        //        ChangeState(pillarState.SacrificeLV2);
        //    }
        //}
    }
    private void GiveFalseHope()    // ������ϴ� ���    [SacrificeLV1]
    {
        // Ż�� �õ� ��ȸ�� ��
        // ���� Ȯ���� 4%
        // ���� �� ���� ü�¿��� 16.666% ü�� ����

    }

    private void TryToAbsorb()      // ��Ƹ��� �õ��� �ϴ� ���   [SacrificeLV2]
    {
        // ���� �ð����� ���� �й�
        // ��ų üũ �� �� ���� ���� �� ó��
        ChangeState(pillarState.AbsorbSacrifice);
    }

    private void DisappearAfterMeal()        // �� �԰� ������� ���    [AbsorbSacrifice]
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

    private void SelfRepair()   // �÷��̾ �����߸��� ������ �����ϴ� ���  [Damaged]
    {
        currentTime += Time.deltaTime;
        if (currentTime > repairTime)
        {
            ChangeState(pillarState.NoSacrifice);
        }
    }

    // ���� �μ���, ������ Ʈ���� �־�� �� **********************************
    private void OnTriggerStay(Collider other)  // �÷��̾ ������ �ɸ��� ���� ���·� ��ȣ�ۿ� ������ ���� �� ��� ������ ���
    {
        //if (other.CompareTag("Player") && playerState < 2)    // �÷��̾� ���� ���� �� �׿� �°� ���� *********************
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
