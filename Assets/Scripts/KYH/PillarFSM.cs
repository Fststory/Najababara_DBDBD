using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarFSM : MonoBehaviour
{
    public enum pillarState
    {
        NoSacrifice = 0,    // ���� ����
        SacrificeLV1 = 1,   // �ش� ���� ���൵ 1
        SacrificeLV2 = 2,   // �ش� ���� ���൵ 2
        AbsorbSacrifice = 3,  // ���� ����
        Damaged = 4   // ������
    }

    public GameObject player;   // �÷��̾� ������Ʈ
    PlayerController playerController;   // �÷��̾� ��Ʈ�ѷ�
    PlayerFSM playerFSM;   // �÷��̾� ����
    Animator playerAnim;
    public pillarState currentState;   // ���� ���� ����

    public float currentTime = 0;  // ���� �ð� (�������� Ÿ�̸ӿ� ���� ���, ���� ��)
    public float reduceHPTime;  // ü���� ��� �ֱ�
    public float escapePercetage;   // Ż�� Ȯ��
    public float eatTime;   // ��� �ð�
    public float repairTime;    // ���� �ð�

    public GameObject entity;
    public GameObject smoke;

    void Start()
    {
        currentState = pillarState.NoSacrifice;
        playerController = player.GetComponent<PlayerController>();
        playerFSM = player.GetComponent<PlayerFSM>();
        playerAnim = player.GetComponent<Animator>();
        entity.SetActive(false);
        smoke.SetActive(false);
    }

    void Update()
    {
        switch (currentState)
        {            
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
            case pillarState.Damaged:
                SelfRepair();
                break;
        }
    }    

    private void WaitSacrifice()    // ������ �ɸ��� ���¸� �������� ���� �¼����� ��ȯ ���
    {
        if (playerFSM.pyState == PlayerFSM.PlayerState.Hooked)   // �÷��̾ �ɷ����� ü�� Ȯ��!
        {
            if (!playerController.alraedyHooked)    // �÷�� ó�� �ɸ��ٸ�
            {
                ChangeState(pillarState.SacrificeLV1);  // 1�ܰ� ����
                playerController.alraedyHooked = true;
            }
            else if (playerController.alraedyHooked)  // �÷��̾ �̹� �ѹ� �ɷȾ��ٸ�
            {
                ChangeState(pillarState.SacrificeLV2);    // 2�ܰ� ����
            }
        }
    }

    private void GiveFalseHope()    // ������ϴ� ���    [SacrificeLV1]
    {
        // �÷��̾� ü���� ���ݾ� ����
        currentTime += Time.deltaTime;
        if (currentTime > reduceHPTime)
        {
            playerFSM.currentHp--;
            currentTime = 0;
        }
        // Ż�� �õ� ��ȸ�� ��
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= escapePercetage)   // ���� Ȯ���� 4%
            {
                playerController.enabled = playerController.enabled;   // �÷��̾� ��Ʈ�ѷ� Ȱ��ȭ!
                playerFSM.pyState = PlayerFSM.PlayerState.Injured;  // �÷��̾ �λ� ���·� ����
                ChangeState(pillarState.NoSacrifice);   // ������ ���� ���� ���·� ��ȯ
                print("Ż�� ����!");
                playerAnim.SetTrigger("escape");
            }
            else
            {
                // ���� �� ���� ü�¿��� 16.666% ü�� ����
                playerFSM.currentHp -= playerFSM.currentHp * 0.16666f;
                print("Ż�� ����!");
            }    
        }
        if (playerFSM.currentHp <= 50)  // ü���� 50 ���ϰ� �ȴٸ�...
        {
            ChangeState(pillarState.SacrificeLV2);  // 2�ܰ� ����
        }
    }

    private void TryToAbsorb()      // ��Ƹ��� �õ��� �ϴ� ���   [SacrificeLV2]
    {
        entity.SetActive(true);
        smoke.SetActive(true);

        // �÷��̾� ü���� ���ݾ� ����
        currentTime += Time.deltaTime;
        if (currentTime > reduceHPTime)
        {
            playerFSM.currentHp--;
            currentTime = 0;
        }
        // ���� �ð����� ���� �й�
        // ��ų üũ �� �� ���� ���� �� ó��
        if (playerFSM.currentHp <= 0)
        {
            ChangeState(pillarState.AbsorbSacrifice);
        }
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

    public void TakeDamage()    // ������ ������ �������� ������ ���
    {
        ChangeState(pillarState.Damaged);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, transform.localEulerAngles + new Vector3(0, 0, 90), 1);
        print("���� ������!");
    }

    private void SelfRepair()   // �÷��̾ �����߸��� ������ �����ϴ� ���  [Damaged]
    {
        currentTime += Time.deltaTime;
        
        if (currentTime > repairTime)
        {
            ChangeState(pillarState.NoSacrifice);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles,
                                                  transform.localEulerAngles + new Vector3(0, 0, -90), 1);
            currentTime = 0;
            print("���� ���� ���� �Ϸ�!");
        }
    }

    void ChangeState(pillarState newState)      // ���� ��ȯ ���
    {
        currentState = newState;
    }       
}
